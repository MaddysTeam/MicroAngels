﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.EventBusRabbitMQ.Configurations;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyClient
{
    public class Startup
    {
        private ContainerBuilder _builder;
        public Startup(IConfigurationBuilder config)
        {
            ConfigureEventBus(config);
            //ConfigureCache(config);
        }

        public IContainer ConfigureServices(ContainerBuilder builder)
        {
            var services = new ServiceCollection();
            //ConfigureLogging(services);
            builder.Populate(services);
            _builder = builder;
            ServiceLocator.Current = builder.Build();
            return ServiceLocator.Current;
        }

        public void Configure(IContainer app)
        {

        }

        #region 私有方法
        /// <summary>
        /// 配置日志服务
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureLogging(IServiceCollection services)
        {
            services.AddLogging();
        }

        private static void ConfigureEventBus(IConfigurationBuilder build)
        {
            build
            .AddEventBusFile("eventBusSettings.json", optional: false);
        }

        /// <summary>
        /// 配置缓存服务
        /// </summary>
        //private void ConfigureCache(IConfigurationBuilder build)
        //{
        //    build
        //      .AddCacheFile("cacheSettings.json", optional: false);
        //}

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="serviceProxyFactory"></param>
        public static void Test(IServiceProxyProvider provider)
        {
            Task.Run(async () =>
            {
                var model = new Dictionary<string, object> {
                    { "para", new { PageIndex = 1, PageSize = 2 } }
                };

                var path = "api/user/getAllUserInfo";
                var serviceKey = "User";
                var proxy = await provider.Invoke<object>(model,path,serviceKey);

                //var userProxy = serviceProxyFactory.CreateProxy<IUserService>("User");
                //var v = userProxy.GetUserId("fanly").GetAwaiter().GetResult();
                //var fa = userProxy.GetUserName(1).GetAwaiter().GetResult();
                //var v1 = userProxy.GetUserLastSignInTime(1).Result;
                //var apiResult = userProxy.GetApiResult().GetAwaiter().GetResult();
                //userProxy.PublishThroughEventBusAsync(new UserEvent
                //{
                //    UserId = 1,
                //    Name = "fanly"
                //}).Wait();

                //userProxy.PublishThroughEventBusAsync(new UserEvent
                //{
                //    UserId = 1,
                //    Name = "fanly"
                //}).Wait();

                //var r = await userProxy.GetDictionary();
                //var serviceProxyProvider = ServiceLocator.GetService<IServiceProxyProvider>();

                //do
                //{
                //    Console.WriteLine("正在循环 1w次调用 GetUser.....");

                //    //1w次调用
                //    var watch = Stopwatch.StartNew();
                //    for (var i = 0; i < 10000; i++)
                //    {
                //        //var a = userProxy.GetDictionary().Result;
                //        var a = userProxy.GetDictionary().Result;
                //        //var result = serviceProxyProvider.Invoke<object>(new Dictionary<string, object>(), "api/user/GetDictionary", "User").Result;
                //    }
                //    watch.Stop();
                //    Console.WriteLine($"1w次调用结束，执行时间：{watch.ElapsedMilliseconds}ms");
                //    Console.WriteLine("Press any key to continue, q to exit the loop...");
                //    var key = Console.ReadLine();
                //    if (key.ToLower() == "q")
                //        break;
                //} while (true);
                Console.WriteLine("please enter key to exit");
                Console.ReadLine();
            }).Wait();
        }

        //public static void TestRabbitMq(IServiceProxyFactory serviceProxyFactory)
        //{
        //    serviceProxyFactory.CreateProxy<IUserService>("User").PublishThroughEventBusAsync(new UserEvent()
        //    {
        //        Age = 18,
        //        Name = "fanly",
        //        UserId = 1
        //    });
        //    Console.WriteLine("Press any key to exit...");
        //    Console.ReadLine();
        //}

        //public static void TestForRoutePath(IServiceProxyProvider serviceProxyProvider)
        //{
        //    Dictionary<string, object> model = new Dictionary<string, object>();
        //    model.Add("user", JsonConvert.SerializeObject(new
        //    {
        //        Name = "fanly",
        //        Age = 18,
        //        UserId = 1
        //    }));
        //    string path = "api/user/getuser";
        //    string serviceKey = "User";

        //    var userProxy = serviceProxyProvider.Invoke<object>(model, path, serviceKey);
        //    var s = userProxy.Result;
        //    Console.WriteLine("Press any key to exit...");
        //    Console.ReadLine();
        //}
        #endregion

    }
}
