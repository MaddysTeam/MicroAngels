using MicroAngels.Gateway.Ocelot;
using MicroAngels.OcelotGateway.Services;
using MicroAngels.ServiceDiscovery.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;
using System;

namespace MicroAngels.OcelotGateway
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
			// global accesstoken for all service
			services.AddAuthentication()
				.AddIdentityServerAuthentication(ServiceAuthenticationOptions.GlobalApiAuthenticationKey, ServiceAuthenticationOptions.GlobalApiClient);

			// if you need use different token for each service , you comment following code below:
			//.AddIdentityServerAuthentication(ServiceAuthenticationOptions.OtherKey, ServiceAuthenticationOptions.OtherClient);

			services.AddOcelot(Configuration).AddAngelsOcelot(c =>
			{
				c.ConnectString = Configuration["Database:Mysql:OcelotConn"];
				c.RedisConnectStrings = new string[] { Configuration["Redis:OcelotConn"] };
				c.IsUseCustomAuthenticate = true;
			})
			.AddConsul()
			.AddPolly();

			// add cors for ocelot
			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy",
				builder => builder.WithOrigins("*")
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowCredentials());
			});

			services.AddServiceFinder(
				new ConsulHostConfiguration
				{
					Host = Configuration["Consul:Host"],
					Port=  Convert.ToInt32(Configuration["Consul:Port"])
				});

			//inject CustomAuthenticateService for gateway
			services.AddTransient<ICustomAuthenticateService, CustomAuthenticateService>();
			services.AddTransient<ICustomTokenRefreshService, CustomTokenRefreshService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseCors("CorsPolicy");

			app.UseAngleOcelot().Wait();
		}
	}
}
