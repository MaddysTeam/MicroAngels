using Business;
using MicroAngels.AuthServer.Services;
using MicroAngels.Cache;
using MicroAngels.Cache.Redis;
using MicroAngels.IdentityServer.Extensions;
using MicroAngels.IdentityServer.Models;
using MicroAngels.IdentityServer.Services;
using MicroAngels.IdentityServer.Validators;
using MicroAngels.Logger.ExceptionLess;
using MicroAngels.ORM.Suger;
using MicroAngels.ServiceDiscovery.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MicroAngels.AuthServer
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			// add mvc,include filters and etc..
			services.AddMvcCore()
			.AddControllersAsServices()
			.AddAuthorization()
			.AddJsonFormatters();

			// add redis cache
			services.AddRedisCache(new RedisCacheOption
				(
				 Configuration["Redis:Host"],
				 Convert.ToInt32(Configuration["Redis:Port"]),
				 0,
				 3600
				));

			// add identity server
			services.AddIdentityServer(x =>
				{
					//x.IssuerUri = "http://identity";
					//x.PublicOrigin = "http://identity";
				})
				.AddDeveloperSigningCredential()
				.UseMysql(opt =>
				{
					opt.ConnectionStrings = Configuration["Database:Mysql:IdsConn"];
				})
				.UseValidateService<UserValidateService>()
				.UseClaimsGrantService<UserClaimGrantService>()
				.AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
				.AddProfileService<UserClaimsProfileService>(); // add claims into user profile （such as context）


			//token authentication
			services.AddIdsAuthentication(new IdentityAuthenticationOptions
			{
				Scheme = Configuration["IdentityService:DefaultScheme"],
				Authority = Configuration["IdentityService:Uri"],
				RequireHttps = Convert.ToBoolean(Configuration["IdentityService:UseHttps"]),
				ApiSecret = "secreta",
				ApiName = "MessageCenter"
			});

			// add exceptionless logger
			services.AddLessLog();

			// for business service
			services.AddTransient<ISystemService, SystemService>();
			services.AddTransient<IAssetsService, AssetsService>();
			services.AddTransient<IRoleService, RoleService>();
			services.AddTransient<IUserService, UserService>();

			var serviceProvider = services.AddCacheInterceptorContainer()
				.AddInterceptor<UserInfo>(typeof(IUserCaching))
				.BuildProvider();

			return serviceProvider;
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifeTime)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseAuthentication();

			app.UseMvc();

			app.UseIdentityServer();

			app.UseLessLog(new ExcepitonLessOptions("2KgsjbuAo0t2qTv8uuoLmEuTMOzYfoAD8VI01Elo"));

			app.UseConsul(lifeTime, new ConsulService
			{
				Id = Configuration["Service:Id"],
				Host = Configuration["Service:Host"],
				Port = Convert.ToInt32(Configuration["Service:Port"]),
				Name = Configuration["Service:Name"],
				HostConfiguration = new ConsulHostConfiguration
				{
					Host = Configuration["Consul:Host"],
					Port = Convert.ToInt32(Configuration["Consul:Port"]),
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
