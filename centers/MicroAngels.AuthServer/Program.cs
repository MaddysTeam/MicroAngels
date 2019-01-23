using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:2012")
                .UseStartup<Startup>()
                .Build();
    }
}
