using FileService.Business;
using MicroAngels.Bus.CAP;
using MicroAngels.GRPC.Server;
using MicroAngels.IdentityServer.Extensions;
using MicroAngels.IdentityServer.Models;
using MicroAngels.ORM.Suger;
using MicroAngels.ServiceDiscovery.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FileService
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

			//add cap
			services.AddKafkaService(new CAPService
			{
				Host = Configuration["Queues:Kafka:Host"],
				ConnectString = Configuration["Queues:Kafka:DbConn"]
			});

			//add grpc server
			services.AddGRPCServer(new GRPCServerOptions
			{
				Host = Configuration["Service:Host"],
				Port = Convert.ToInt32(Configuration["Service:Port"])
			});

			//add mvc core
			services.AddMvcCore(options =>
			{
				//options.Filters.Add<ExcepitonFilter>();
			})
			.AddApiExplorer() // for swagger
			.AddAuthorization()
			.AddJsonFormatters()
			.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			//add ids authentication
			services.AddIdsAuthentication(new IdentityAuthenticationOptions
			{
				Scheme = Configuration["IdentityService:DefaultScheme"],
				Authority = Configuration["IdentityService:Uri"],
				RequireHttps = Convert.ToBoolean(Configuration["IdentityService:UseHttps"]),
				ApiSecret = Configuration["IdentityService:Client:Secret"],
				ApiName = Configuration["IdentityService:Client:Scopes"]
			});
			
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env,IApplicationLifetime lifeTime)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseAuthentication();

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

			// register orm sugar and init tables
			app.UseSugarORM(lifeTime, Configuration)
				.InitTabels(MySqlDbContext.TableTypes); 

		}
	}
}
