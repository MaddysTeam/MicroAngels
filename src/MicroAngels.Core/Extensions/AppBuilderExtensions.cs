namespace MessageCenter
{
    public static class AppBuilderExtensions
    {

        #region zipkin
        //public static IApplicationBuilder RegisterZipkin(this IApplicationBuilder app,ILoggerFactory factory , IApplicationLifetime lifetime, IConfiguration configuration )
        //{
        //    lifetime.ApplicationStarted.Register(() => {
        //        TraceManager.SamplingRate = 1.0f;
        //        var logger = new TracingLogger(factory,"zipkin4net");
        //        var httpSender = new HttpZipkinSender($"http://{configuration["Zipkin:Host"]}", "application/json");
        //        var tracer = new ZipkinTracer(httpSender,new JSONSpanSerializer(),new Statistics());
        //        var consoleTracer = new ConsoleTracer();

        //        TraceManager.RegisterTracer(tracer);
        //        TraceManager.RegisterTracer(consoleTracer);
        //        TraceManager.Start(logger);
        //    });

        //    lifetime.ApplicationStopped.Register(()=>TraceManager.Stop());
        //    app.UseTracing("demo");

        //    return app;
        //}

        #endregion

    }
}