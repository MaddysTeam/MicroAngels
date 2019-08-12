﻿using Business.Services;
using MicroAngels.Bus.CAP;
using MicroAngels.IdentityServer.Extensions;
using MicroAngels.IdentityServer.Models;
using MicroAngels.ServiceDiscovery.Consul;
using MicroAngels.ORM.Suger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using MicroAngels.Logger.ExceptionLess;

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
			services.AddCAPService(new CAPService
			{
				Host = Configuration["Queues:Kafka:Host"],
				ConnectString = Configuration["Queues:Kafka:DbConn"]
			});

			// add swagger
			//services.AddSwaggerService(new SwaggerService
			//{
			//	Name = Configuration["Swagger:Name"],
			//	Title = Configuration["Swagger:Title"],
			//	Version = Configuration["Swagger:Version"],
			//	XMLPath = Path.Combine(
			//			Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationBasePath,
			//			$"{ Configuration["Swagger:Name"]}.xml"
			//		)
			//},opt=> {
			//	// set document or operation filter
			//	//opt.DocumentFilter<>
			//	//opt.OperationFilter<>
			//	//opt.AddSecurityDefinition()
			//});

			//add mvc core
			services.AddMvcCore(options =>
					 {
						 //options.Filters.Add<ExcepitonFilter>();
					 })
					 .AddApiExplorer() // for swagger
					 .AddAuthorization()
					 .AddJsonFormatters()
					 .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


			services.AddIdsAuthentication(new IdentityAuthenticationOptions
			{
				Scheme = Configuration["IdentityService:DefaultScheme"],
				Authority = Configuration["IdentityService:Uri"],
				RequireHttps = Convert.ToBoolean(Configuration["IdentityService:UseHttps"]),
				ApiSecret = "secreta",
				ApiName = "MessageCenter"
			});

			services.AddLessLog();

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifeTime, ILoggerFactory loggerFactory)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			// use swagger service
			//app.UseSwaggerService(
			// new SwaggerService { Name = Configuration["Swagger:Name"] },
			// new SwaggerUIOptions { IsShowExtensions = true });


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

			app.UseLessLog(new ExcepitonLessOptions("2KgsjbuAo0t2qTv8uuoLmEuTMOzYfoAD8VI01Elo"));

			app.UseSugarORM(lifeTime, Configuration);  // register orm sugar
		}
	}
}
