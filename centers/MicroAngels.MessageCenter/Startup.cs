using Business.Services;
using Infrastructure;
using Infrastructure.Orms.Sugar;
using MicroAngels.Bus.CAP;
using MicroAngels.ServiceDiscovery.Consul;
using MicroAngels.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using System;
using System.IO;

namespace MessageCenter
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// service injection
			services.AddTransient<ITopicService, TopicService>();
			services.AddTransient<ISubscribeService, SubscribeService>();
			services.AddTransient<IMessageService, MessageService>();

			// add cap
			services.AddKafkaService(new CapService
			{
				Host = Configuration["Queues:Kafka:Host"],
				ConnectString = Configuration["Queues:Kafka:DbConn"]
			});

			// add swagger
			services.AddSwaggerService(new SwaggerService
			{
				Name = Configuration["Swagger:Name"],
				Title = Configuration["Swagger:Title"],
				Version = Configuration["Swagger:Version"],
				XMLPath = Path.Combine(
						Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationBasePath,
						$"{ Configuration["Swagger:Name"]}.xml"
					)
			},opt=> {
				// set document or operation filter
				//opt.DocumentFilter<>
				//opt.OperationFilter<>
				//opt.AddSecurityDefinition()
			});

			//add mvc core
			services.AddMvcCore(options =>
					 {
						 options.Filters.Add<ExcepitonFilter>();
					 })
					 .AddApiExplorer() // for swagger
					 .AddAuthorization()
					 .AddJsonFormatters()
					 .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			//token authentication
			services.AddAuthentication(Configuration["IdentityService:DefaultScheme"])
				   .AddJwtBearer(options =>
				   {
					   options.Authority = Configuration["IdentityService:Uri"];
					   options.RequireHttpsMetadata = Convert.ToBoolean(Configuration["IdentityService:UseHttps"]);
					   options.Audience = Configuration["IdentityService:Audience"];
				   });

			// add cross domain policy
			services.AddCors(options =>
			{
				options.AddPolicy("CORS",
				  builder => builder.AllowAnyOrigin()
				  .AllowAnyMethod()
				  .AllowAnyHeader()
				  .AllowCredentials());
			});

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifeTime, ILoggerFactory loggerFactory)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			// use swagger service
			app.UseSwaggerService(
			 new SwaggerService { Name = Configuration["Swagger:Name"] },
			 new SwaggerUIOptions { IsShowExtensions = true });

			// add nlog
			//	loggerFactory.AddNLog();

			// config nlog
			//	env.ConfigureNLog("NLog.config"); 

			// use authentiaction
			app.UseAuthentication();

			app.UseMvc()   // use mvc with swagger
			   .UseConsul(lifeTime, new ConsulService //regsiter consul
			   {
				   Id = Configuration["Service:Id"],
				   Host = Configuration["Service:Host"],
				   Port = Convert.ToInt32(Configuration["Service:Port"]),
				   Name = Configuration["Service:Name"],
				   HostConfiguration = new ConsulHostConfiguration
				   {
					   Host = Configuration["Consul:Host"],
					   Port = Convert.ToInt32(Configuration["Consul:Port"])
				   },
				   HealthCheckOptoins = new ConsuleHealthCheckOptoins
				   {
					   HealthCheckHTTP = Configuration["Service:HealthCheck:Address"],
					   IntervalTimeSpan = TimeSpan.Parse(Configuration["Service:HealthCheck:Interval"])
				   }
			   });

			// use cross domain policy
			app.UseCors("CORS");

			app.RegisterMysqlBySugar(lifeTime, Configuration);  // register orm sugar

			//TODO: will use skywalking instead
			// app.RegisterZipkin(loggerFactory, lifeTime, Configuration);
		}
	}
}
