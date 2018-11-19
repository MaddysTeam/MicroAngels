using Autofac;
using Surging.Core.Caching.Configurations;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Configurations;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.EventBusRabbitMQ;
using Surging.Core.ProxyGenerator;
using Surging.Core.ServiceHosting;
using Surging.Core.ServiceHosting.Internal.Implementation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyService
{
    class Program
    {
        static void Main(string[] args)
        {
            // console only
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var host = new ServiceHostBuilder()
                .RegisterServices(builder =>
                {
                    builder.AddMicroService(options =>
                    {
                        options.AddServiceRuntime();
                        options.AddRelateService();
                        builder.Register(p => new CPlatformContainer(ServiceLocator.Current));
                    });
                })
            .SubscribeAt()
            .UseServer("127.0.0.1",98,"true")
             .UseConsoleLifetime()
                .Configure(build =>
                build.AddCacheFile("${cachepath}|cacheSettings.json", optional: false, reloadOnChange: true))
                  .Configure(build =>
                build.AddCPlatformFile("${surgingpath}|surgingSettings.json", optional: false, reloadOnChange: true))
             //.UseProxy()
             .UseStartup<Startup>()
             .Build();
            //.UseServer(options =>
            //{
            //})
            //.UseProxy()
            //.UseStartup<Startup>()
            //.Build();

            using (host.Run())
            {
                Console.WriteLine("service start successful!");
            }
        }
    }


    #region [demo service]

    [ServiceBundle("api/{Service}")]
    public interface IDemoService : IServiceKey
    {
        [Service(Date = "2018-11-10")]
        Task<Dictionary<string, object>> GetAllThings();
    }

    [ModuleName("Demo")]
    public class DemoService :ProxyServiceBase, IDemoService
    {
        public DemoService()
        {

        }

        public  Task<Dictionary<string, object>> GetAllThings()
        {
            return  Task.FromResult(new Dictionary<string, object> { { "aaa",12} });
        }
    }

    [ServiceBundle("api/{Service}")]
    public interface IUserService : IServiceKey
    {
        [Service(Date = "2018-11-10")]
        Task<Dictionary<string, object>> GetAllUserInfo();
    }

    [ModuleName("User")]
    public class UserService : ProxyServiceBase, IUserService
    {
        public UserService()
        {

        }

        public  Task<Dictionary<string, object>> GetAllUserInfo()
        {
            return  Task.FromResult(new Dictionary<string, object> { { "aaa", 12 } });
        }
    }

    #endregion

}
