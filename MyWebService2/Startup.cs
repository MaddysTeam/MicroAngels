using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using NLog.Extensions.Logging;
using NLog.Web;
using Ocelot.JwtAuthorize;
using Swashbuckle.AspNetCore.Swagger;

namespace MyWebService2
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddTokenJwtAuthorize();

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc(Configuration["Swagger:Name"], new Info { Title = Configuration["Swagger:Title"], Version = Configuration["Swagger:Version"] });
                //var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                //var xmlPath = Path.Combine(basePath, "Qka.UsersApi.xml");
                //opt.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifeTime, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddNLog();

            env.ConfigureNLog("NLog.config");

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

            app.RegisterConsul(lifeTime, new Models.ServiceEntityModel
            {
                IP = Configuration["Service:Ip"],
                Port = Convert.ToInt32(Configuration["Service:Port"]),
                ServiceName = Configuration["Service:Name"],
                ConsulIP = Configuration["Consul:IP"],
                ConsulPort = Convert.ToInt32(Configuration["Consul:Port"])
            });

            app.RegisterMysqlBySugar(lifeTime, Configuration);

            // app.RegisterZipkin(loggerFactory, lifeTime, Configuration);
        }
    }
}
