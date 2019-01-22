using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace OcelotGateway
{
    public class Program
    {

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
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
