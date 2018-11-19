﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace MyWebService2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:5002")
                .ConfigureLogging(logging =>
                 {
                     logging.ClearProviders();
                     logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Warning);
                 })
                .UseNLog()
                .UseStartup<Startup>();
    }
}
