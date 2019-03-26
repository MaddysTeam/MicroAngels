﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;
using OcelotGateway.Services;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;

namespace OcelotGateway
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
            services.AddAuthentication()
                .AddIdentityServerAuthentication(ServiceAuthenticationOptions.MessageApiAuthenticationKey, ServiceAuthenticationOptions.MessageApiClient);

            services.AddOcelot(Configuration).AddConsul().AddPolly();

            //services.AddTokenJwtAuthorize();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ApiGateway", new Info { Title = "gateway service", Version = "v1" });
            });

             services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // use swagger
            app
               .UseMvc()
               .UseSwagger()
               .UseSwaggerUI(options=> {
                   new List<string> { "MyWebService2","MessageCenter"}.ForEach(x=> {
                       options.SwaggerEndpoint($"/{x}/swagger.json", x);
                   });
               });

           // app.UseMvc();

            app.UseOcelot().Wait();
        }
    }
}