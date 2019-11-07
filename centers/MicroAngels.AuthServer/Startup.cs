using Business;
using MicroAngels.AuthServer.Services;
using MicroAngels.Bus.CAP;
using MicroAngels.Cache;
using MicroAngels.Cache.Redis;
using MicroAngels.Core.Plugins;
using MicroAngels.IdentityServer.Extensions;
using MicroAngels.IdentityServer.Models;
using MicroAngels.IdentityServer.Providers.Redis;
using MicroAngels.IdentityServer.Services;
using MicroAngels.IdentityServer.Validators;
using MicroAngels.Logger.ExceptionLess;
using MicroAngels.ORM.Suger;
using MicroAngels.ServiceDiscovery.Consul;
using MicroAngels.Swagger;
using MicroAngels.Trace.Jaeger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

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
			// for business service
			services.AddTransient<ISystemService, SystemService>();
			services.AddTransient<IAssetsService, AssetsService>();
			services.AddTransient<IRoleService, RoleService>();
			services.AddTransient<IUserService, UserService>();

			// add cap service
			services.AddCAPService(new CAPService
			{
				Host = Configuration["Queues:Kafka:Host"],
				ConnectString = Configuration["Queues:Kafka:DbConn"]
			});


			// add swagger service
			services.AddSwaggerService(new SwaggerService
			{
				Name = Configuration["swagger:name"],
				Title = Configuration["swagger:title"],
				Version = Configuration["swagger:version"],
				XMLPath = Path.Combine(
						Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationBasePath,
						$"{ Configuration["swagger:name"]}.xml"
					)
			}, opt =>
			{
				// set document or operation filter
				//opt.documentfilter<>
				//opt.operationfilter<>
				//opt.addsecuritydefinition()
			});

			// add mvc,include filters and etc..
			services.AddMvcCore()
			.AddApiExplorer()
			.AddAuthorization()
			.AddJsonFormatters();

			// add jaeger trace
			services.AddJaegerTrace(options => {
				options.ServiceName = Configuration["Jaeger:Service"];
				options.ReporterOptions.RemoteHost = Configuration["Jaeger:Reporter:Host"];
				options.ReporterOptions.RemotePort = Convert.ToInt32(Configuration["Jaeger:Reporter:Port"]);
			});

			// add exceptionless logger
			services.AddLessLog();


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
				.AddProfileService<UserClaimsProfileService>()// add claims into user profile （such as context）
				.UseRedisGrantStore(new RedisOperationalStoreOptions($"{Configuration["Redis:Host"]}:{Configuration["Redis:Port"]}") ); // use redis grant strore instead

			//token authentication
			services.AddIdsAuthentication(new IdentityAuthenticationOptions
			{
				Scheme = Configuration["IdentityService:DefaultScheme"],
				Authority = Configuration["IdentityService:Uri"],
				RequireHttps = Convert.ToBoolean(Configuration["IdentityService:UseHttps"]),
				ApiSecret = "secreta",
				ApiName = "MessageCenter"
			});

			
			//service finder
			services.AddServiceFinder(
			new ConsulHostConfiguration
			{
				Host = Configuration["Consul:Host"],
				Port = Convert.ToInt32(Configuration["Consul:Port"])
			});

			// balancer
			services.AddTransient<ILoadBalancer, WeightRoundBalancer>();

			var serviceProvider = services.AddCacheInterceptorContainer()
				.AddInterceptor<UserInfo>(typeof(IUserCaching))
				.AddInterceptor<System.Collections.Generic.IEnumerable<UserInfo>>(typeof(IUserCaching))
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

			// use swagger service
			app.UseSwaggerService(
			 new SwaggerService { Name = Configuration["Swagger:Name"] },
			 new SwaggerUIOptions { IsShowExtensions = true });

			app.UseMvc();

			app.UseIdentityServer();

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
