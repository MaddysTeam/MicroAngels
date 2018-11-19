using Autofac;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Configurations;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.EventBusRabbitMQ.Configurations;
using Surging.Core.ProxyGenerator;
using Surging.Core.ServiceHosting;
using Surging.Core.ServiceHosting.Internal.Implementation;
using System;
using System.Text;

namespace MyClient
{

    class Program
    {

        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var host = new ServiceHostBuilder()
                .RegisterServices(builders =>
                {
                    builders.AddMicroService(option =>
                    {
                        option.AddClient();
                        builders.Register(p => new CPlatformContainer(ServiceLocator.Current));
                    });
                })
                .Configure(b => b.AddEventBusFile("eventBusSettings.json", optional: false))
                .Configure(b => b.AddCPlatformFile("surgingSettings.json", optional: false))
                .UseClient()
                .UseStartup<Startup>()
                .Build();

            using (host.Run())
            {
                Startup.Test(ServiceLocator.GetService<IServiceProxyProvider>());
            }
        }

        

    }

    public static class TryTest
    {
        public static void Test()
        {
            Startup.Test(ServiceLocator.GetService<IServiceProxyProvider>());
        }
    }


}
