using FileService.Business;
using MicroAngels.Bus.CAP;
using MicroAngels.GRPC.Server;
using MicroAngels.Hystrix.Polly;
using MicroAngels.IdentityServer.Extensions;
using MicroAngels.IdentityServer.Models;
using MicroAngels.Logger.ExceptionLess;
using MicroAngels.ORM.Suger;
using MicroAngels.ServiceDiscovery.Consul;
using MicroAngels.Trace.Jaeger;
using MicroAngels.Trace.Prometheus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;

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
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			//add cap
			//services.AddCAPService(new CAPService
			//{
			//	Host = Configuration["Queues:Kafka:Host"],
			//	ConnectString = Configuration["Queues:Kafka:DbConn"]
			//});

			////add grpc server
			//services.AddGRPCServer(new GRPCServerOptions
			//{
			//	Host = Configuration["GrpcServer:Host"],
			//	Port = Convert.ToInt32(Configuration["GrpcServer:Port"])
			//});

			//add mvc core
			services.AddMvcCore(options =>
			{
				//options.Filters.Add<ExcepitonFilter>();
			})
			.AddApiExplorer() // for swagger
			//.AddAuthorization()
			.AddJsonFormatters()
			.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			//add ids authentication
			//services.AddIdsAuthentication(new IdentityAuthenticationOptions
			//{
			//	Scheme = Configuration["IdentityService:DefaultScheme"],
			//	Authority = Configuration["IdentityService:Uri"],
			//	RequireHttps = Convert.ToBoolean(Configuration["IdentityService:UseHttps"]),
			//	ApiSecret = Configuration["IdentityService:Client:Secret"],
			//	ApiName = Configuration["IdentityService:Client:Scopes"]
			//});

			//services.AddJaegerTrace(options=> {
			//	options.ServiceName = Configuration["Jaeger:Service"];
			//	options.ReporterOptions.RemoteHost = Configuration["Jaeger:Reporter:Host"];
			//	options.ReporterOptions.RemotePort = Convert.ToInt32(Configuration["Jaeger:Reporter:Port"]);
			//});

			//add exceptionless logger
			services.AddLessLog();

			services.AddTransient<IFileService, FileService.Business.FileService>();
			//services.AddTransient<IPrivilegeSupplier, FileService.Business.PrivilegeSupplier>();

			// regist hystrix polly aop serive
			return PollyRegister.RegisterPollyServiceInAssembly(this.GetType().Assembly, services,Configuration);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env,IApplicationLifetime lifeTime)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			//app.UsePrometheus();

			//app.UseAuthentication();

			app.UseMvc();

			app.UseLessLog(new ExcepitonLessOptions("ocBoXO0x8jdMAuqoKAQSG91nfwNGzgjT2IZ64RmM"));

			app.UseStaticFiles(new StaticFileOptions
			{
				//配置除了默认的wwwroot文件中的静态文件以外的文件夹  提供 Web 根目录外的文件  经过此配置以后，就可以访问StaticFiles文件下的文件
				FileProvider = new PhysicalFileProvider(
					  Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")),
				RequestPath = "/StaticFiles",
			});

			//app.UseConsul(lifeTime, new ConsulService
			//{
			//	Id = Configuration["Service:Id"],
			//	Host = Configuration["Service:Host"],
			//	Port = Convert.ToInt32(Configuration["Service:Port"]),
			//	Name = Configuration["Service:Name"],
			//	HostConfiguration = new ConsulHostConfiguration
			//	{
			//		Host = Configuration["Consul:Host"],
			//		Port = Convert.ToInt32(Configuration["Consul:Port"]),
			//	},
			//	HealthCheckOptoins = new ConsuleHealthCheckOptoins
			//	{
			//		HealthCheckHTTP = Configuration["Service:HealthCheck:Address"],
			//		IntervalTimeSpan = TimeSpan.Parse(Configuration["Service:HealthCheck:Interval"])
			//	}
			//});

			////register orm sugar and init tables
			//app.UseSugarORM(lifeTime, Configuration)
			//	.InitTabels(MySqlDbContext.TableTypes);

		}
	}
}
