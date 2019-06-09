﻿using MicroAngels.Cache.Redis;
using MicroAngels.IdentityServer.Extensions;
using MicroAngels.IdentityServer.Models;
using MicroAngels.ServiceDiscovery.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using UserService.Business;

namespace UserService
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
			// add redis cache
			services.AddRedisCache(new RedisCacheOption
				(
				 Configuration["Redis:Host"],
				 Convert.ToInt32(Configuration["Redis:Port"]),
				 0,
				 3600
				));

			//add mvc core
			services.AddMvcCore(options =>
			{
				//options.Filters.Add<ExcepitonFilter>();
			})
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

			//for business
			services.AddTransient<IUserService, UserService.Business.UserService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app,IApplicationLifetime lifeTime)
		{
			app.UseMvc();

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
		}

	}

}
