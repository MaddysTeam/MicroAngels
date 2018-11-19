using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

namespace OcelotGateway
{
    public class Program
    {

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
            //new WebHostBuilder()
            // .UseUrls("http://*:5000")
            // .UseKestrel()
            // .UseContentRoot(Directory.GetCurrentDirectory())
            // .ConfigureAppConfiguration((hostingContext, config) =>
            // {
            //     config
            //         .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
            //         .AddJsonFile("appsettings.json", true, true)
            //         .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
            //         .AddJsonFile("ocelot.json")
            //         .AddEnvironmentVariables();
            // })
            // .ConfigureServices(s => {
            //     s.AddOcelot()
            //      .AddConsul()
            //      .AddPolly();
            // })
            // .ConfigureLogging((hostingContext, logging) =>
            // {
            //    //add your logging
            //})
            // .UseIISIntegration()
            // .Configure(app =>
            // {
            //     app.UseOcelot().Wait();
            // })
            // .Build()
            // .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseUrls($"http://*:5000")
                            .ConfigureAppConfiguration((hostingContext, builder) =>
                            {
                                builder.AddJsonFile("ocelot.json", false, true);
                            })
                .UseStartup<Startup>();
    }
}
