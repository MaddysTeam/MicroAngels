using Business;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Infrastructure.ServiceRegistration.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pivotal.Discovery.Client;
using System;

namespace Identity
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
            // add mvc,include filters and etc..
            services.AddMvcCore()
            .AddControllersAsServices()
            .AddAuthorization()
            .AddJsonFormatters();

            var config = new Config(Configuration);
            services.AddIdentityServer(x =>
                {
                    //x.IssuerUri = "http://identity";
                    //x.PublicOrigin = "http://identity";
                })
                .AddDeveloperSigningCredential()
                //.AddTestUsers(config.GetTestUsers())
                .AddInMemoryPersistedGrants()
                .AddInMemoryApiResources(config.GetApiResources())
                .AddInMemoryClients(config.GetClients())
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();
                
            //services.AddSingleton<IPersistedGrantStore, RedisPersistedGrantStore>();
            //services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,IApplicationLifetime lifeTime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseIdentityServer();

            app.RegisterConsul(lifeTime, new ServiceEntityModel
            {
                IP = Configuration["Service:Ip"],
                Port = Convert.ToInt32(Configuration["Service:Port"]),
                ServiceName = Configuration["Service:Name"],
                ConsulIP = Configuration["Consul:IP"],
                ConsulPort = Convert.ToInt32(Configuration["Consul:Port"])
            });
        }
    }
}
