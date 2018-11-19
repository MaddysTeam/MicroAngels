using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Surging.Core.Caching;
using Surging.Core.Caching.Configurations;
using Surging.Core.Consul;
using Surging.Core.Consul.Configurations;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Cache;
using Surging.Core.CPlatform.Configurations;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.DotNetty;
using Surging.Core.ProxyGenerator;
using Surging.Core.System.Intercept;

namespace MyWebClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration,IHostingEnvironment env)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder()
           .SetBasePath(env.ContentRootPath)
           .AddCacheFile("cacheSettings.json", optional: false)
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddCPlatformFile("${surgingpath}|surgingSettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return RegisterAutofac(services);
        }

        private IServiceProvider RegisterAutofac(IServiceCollection services)
        {
           // var registerConfig = ApiGateWayConfig.Register;
            services.AddMvc(options => {
              //  options.Filters.Add(typeof(CustomExceptionFilterAttribute));
            }).AddJsonOptions(options => {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
            services.AddLogging();
            services.AddCors();
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.AddMicroService(options =>
            {
                options.AddServiceRuntime();
                options.AddRelateService();
                //option.AddClient();
                //option.AddCache();
                //option.AddClientIntercepted(typeof(CacheProviderInterceptor));
                options.UseConsulManager(new ConfigInfo("192.168.1.8:8500", enableChildrenMonitor: true));
                options.UseDotNettyTransport();

                //option.AddFilter(new ServiceExceptionFilter());
                //option.UseProtoBufferCodec();
                // option.UseMessagePackCodec();
                builder.Register(m => new CPlatformContainer(ServiceLocator.Current));
            });
            ServiceLocator.Current = builder.Build();
            return new AutofacServiceProvider(ServiceLocator.Current);

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            var serviceCacheProvider = ServiceLocator.Current.Resolve<ICacheNodeProvider>();
            var addressDescriptors = serviceCacheProvider.GetServiceCaches().ToList();
            ServiceLocator.Current.Resolve<IServiceCacheManager>().SetCachesAsync(addressDescriptors);
            ServiceLocator.Current.Resolve<IConfigurationWatchProvider>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            //app.UseCors(builder =>
            //{
            //    var policy = Core.ApiGateWay.AppConfig.Policy;
            //    builder.WithOrigins(policy.Origins);
            //    if (policy.AllowAnyHeader)
            //        builder.AllowAnyHeader();
            //    if (policy.AllowAnyMethod)
            //        builder.AllowAnyMethod();
            //    if (policy.AllowAnyOrigin)
            //        builder.AllowAnyOrigin();
            //    if (policy.AllowCredentials)
            //        builder.AllowCredentials();
            //});
            var myProvider = new FileExtensionContentTypeProvider();
            myProvider.Mappings.Add(".tpl", "text/plain");
            app.UseStaticFiles(new StaticFileOptions() { ContentTypeProvider = myProvider });
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                "Path",
                "{*path}",
                new { controller = "Services", action = "Path" });
            });
        }
    }



    [ServiceBundle("api/{Service}")]
    public interface IDemo2Service : IServiceKey
    {
        [Service(Date = "2018-11-10")]
        Task<Dictionary<string, object>> GetAllWebThings();
    }

    [ModuleName("Demo")]
    public class DemoService : ProxyServiceBase, IDemo2Service
    {
        public DemoService()
        {

        }


        public async Task<Dictionary<string, object>> GetAllWebThings()
        {
            return await Task.FromResult(new Dictionary<string, object> { { "aaa", 12 } });
        }
    }
}
