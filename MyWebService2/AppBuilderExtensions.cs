using Business;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zipkin4net;
using zipkin4net.Middleware;
using zipkin4net.Tracers;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport.Http;

namespace MyWebService2
{
    public static class AppBuilderExtensions
    {

        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IApplicationLifetime lifetime, ServiceEntityModel serviceEntity)
        {
            var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{serviceEntity.ConsulIP}:{serviceEntity.ConsulPort}"));//请求注册的 Consul 地址
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(2),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(1),//健康检查时间间隔，或者称为心跳间隔
                HTTP = $"http://{serviceEntity.IP}:{serviceEntity.Port}/api/health",//健康检查地址
                Timeout = TimeSpan.FromSeconds(5)
            };

            // Register service with consul
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = Guid.NewGuid().ToString(),
                Name = serviceEntity.ServiceName,
                Address = serviceEntity.IP,
                Port = serviceEntity.Port,
                //Tags = new[] { $"urlprefix-/{serviceEntity.ServiceName}" }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
            };

            consulClient.Agent.ServiceRegister(registration).Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();//服务停止时取消注册
            });

            return app;
        }

        public static IApplicationBuilder RegisterZipkin(this IApplicationBuilder app,ILoggerFactory factory , IApplicationLifetime lifetime, IConfiguration configuration )
        {
            lifetime.ApplicationStarted.Register(() => {
                TraceManager.SamplingRate = 1.0f;
                var logger = new TracingLogger(factory,"zipkin4net");
                var httpSender = new HttpZipkinSender($"http://{configuration["Zipkin:Host"]}", "application/json");
                var tracer = new ZipkinTracer(httpSender,new JSONSpanSerializer(),new Statistics());
                var consoleTracer = new ConsoleTracer();

                TraceManager.RegisterTracer(tracer);
                TraceManager.RegisterTracer(consoleTracer);
                TraceManager.Start(logger);
            });

            lifetime.ApplicationStopped.Register(()=>TraceManager.Stop());
            app.UseTracing("demo");

            return app;
        }

        public static IApplicationBuilder RegisterMysqlBySugar(this IApplicationBuilder app,IApplicationLifetime lifetime, IConfiguration configuration)
        {
            MySqlDbContext.Current.Initial(configuration);

            lifetime.ApplicationStopped.Register(() => {
                MySqlDbContext.Current.Stop();
            });

            return app;
        }

    }
}