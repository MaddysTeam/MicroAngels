using Business.Services;
using Infrastructure;
using Infrastructure.Orms.Sugar;
using Infrastructure.ServiceRegistration.Consul;
using MicroAngels.Bus.CAP;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using SkyWalking.AspNetCore;
using SkyWalking.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using System;

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

            // token authentication
            services.AddAuthentication(Configuration["IdentityService:DefaultScheme"])
                   .AddIdentityServerAuthentication(options => {
                        options.Authority = Configuration["IdentityService:Uri"];
                        options.RequireHttpsMetadata = Convert.ToBoolean(Configuration["IdentityService:UseHttps"]);
            });

            //services.AddSkyWalking(option =>
            //{
            //    option.ApplicationCode = Configuration["Service:Name"];
            //    option.DirectServers = Configuration["WatchDogs:SkyWalking:Host"];
            //});

            // add CAP
            services.AddCapWithMySQLAndRabbit(Configuration);

            // add mvc
            services.AddMvc(options=> {
                options.Filters.Add<ExcepitonFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //services.AddTokenJwtAuthorize();

            // add swagger
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc(Configuration["Swagger:Name"], new Info { Title = Configuration["Swagger:Title"], Version = Configuration["Swagger:Version"] });

                #region swagger xml settings
                //var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                //var xmlPath = Path.Combine(basePath, "Qka.UsersApi.xml");
                //opt.IncludeXmlComments(xmlPath);
                #endregion
            });

            // add cross domain policy
            services.AddCors(options => {
                options.AddPolicy("CORS",
                  builder => builder.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials());
            });

            // add service version
            services.AddApiVersioning(v => 
            {
                v.ReportApiVersions = true;//return versions in a response header
                v.DefaultApiVersion = new ApiVersion(1, 0);//default version select 
                v.AssumeDefaultVersionWhenUnspecified = true;//if not specifying an api version,show the default version
            });
         
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifeTime, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // add nlog
            loggerFactory.AddNLog();

            // config nlog
            env.ConfigureNLog("NLog.config");

            // use cross domain policy
            app.UseCors("CORS");

            // use mvc with swagger
            app.UseMvc()
               .UseSwagger(options =>
               {
                   options.RouteTemplate = "{documentName}/swagger.json";
               })
               .UseSwaggerUI(options =>
               {
                   options.ShowExtensions();
                   options.SwaggerEndpoint($"/{Configuration["Swagger:Name"]}/swagger.json", Configuration["Swagger:Name"]);
               });

            // regsiter consul
            app.RegisterConsul(lifeTime, new ServiceEntityModel
            {
                IP = Configuration["Service:Ip"],
                Port = Convert.ToInt32(Configuration["Service:Port"]),
                ServiceName = Configuration["Service:Name"],
                ConsulIP = Configuration["Consul:IP"],
                ConsulPort = Convert.ToInt32(Configuration["Consul:Port"])
            });
            
            // register orm sugar
            app.RegisterMysqlBySugar(lifeTime, Configuration);

            //TODO: will use skywalking instead
            // app.RegisterZipkin(loggerFactory, lifeTime, Configuration);
        }
    }
}
