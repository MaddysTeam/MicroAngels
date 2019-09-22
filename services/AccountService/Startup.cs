using Business;
using MicroAngels.Bus.CAP;
using MicroAngels.Cache.Redis;
using MicroAngels.IdentityServer.Extensions;
using MicroAngels.IdentityServer.Models;
using MicroAngels.Logger.ExceptionLess;
using MicroAngels.ORM.Suger;
using MicroAngels.ServiceDiscovery.Consul;
using MicroAngels.Trace.Jaeger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;


namespace AccountService
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
			//business
			services.AddTransient<IAccountService, Business.AccountService>();

			// add cap service
			services.AddCAPService(new CAPService
			{
				Host = Configuration["Queues:Kafka:Host"],
				ConnectString = Configuration["Queues:Kafka:DbConn"]
			});

			// add redis cache
			services.AddRedisCache(new RedisCacheOption
				(
				 Configuration["Redis:Host"],
				 Convert.ToInt32(Configuration["Redis:Port"]),
				 0,
				 3600
				));

			//add mvc core
			services.AddMvcCore()
			.AddApiExplorer() // for swagger
			.AddAuthorization()
			.AddJsonFormatters()
			.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			//token authentication
			services.AddIdsAuthentication(new IdentityAuthenticationOptions
			{
				Scheme = Configuration["IdentityService:DefaultScheme"],
				Authority = Configuration["IdentityService:Uri"],
				RequireHttps = Convert.ToBoolean(Configuration["IdentityService:UseHttps"]),
				ApiSecret = "secreta",
				ApiName = "MessageCenter"
			});


			// add jaeger trace
			services.AddJaegerTrace(options => {
				options.ServiceName = Configuration["Jaeger:Service"];
				options.ReporterOptions.RemoteHost = Configuration["Jaeger:Reporter:Host"];
				options.ReporterOptions.RemotePort = Convert.ToInt32(Configuration["Jaeger:Reporter:Port"]);
			});

			//add exceptionless logger
			services.AddLessLog();

			

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifeTime, ILoggerFactory loggerFactory)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			

			app.UseAuthentication();

			app.UseMvc();

			app.UseLessLog(new ExcepitonLessOptions("ocBoXO0x8jdMAuqoKAQSG91nfwNGzgjT2IZ64RmM"));

			app.UseConsul(lifeTime, new ConsulService
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


			app.UseSugarORM(lifeTime, Configuration).InitTabels(MySqlDbContext.TableTypes);
		}

	}
}
