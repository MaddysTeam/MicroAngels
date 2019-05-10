using MicroAngels.IdentityServer.Extensions;
using MicroAngels.IdentityServer.Models;
using MicroAngels.ServiceDiscovery.Consul;
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

			//services.AddIdentity<>

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

			// use authentiaction
			app.UseAuthentication();

			app.UseMvc()   // use mvc and consul
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
		}

	}
}
