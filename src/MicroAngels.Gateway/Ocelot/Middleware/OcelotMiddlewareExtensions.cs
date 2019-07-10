using Microsoft.AspNetCore.Builder;
using Ocelot.Configuration;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Creator;
using Ocelot.Configuration.Repository;
using Ocelot.Middleware;
using Ocelot.Middleware.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using Ocelot.Logging;
using Ocelot.Responses;
using System.Linq;

namespace MicroAngels.Gateway.Ocelot
{

	public static class OcelotMiddlewareExtensions
	{
	 
		public static async Task<IApplicationBuilder> UseAngleOcelot(this IApplicationBuilder builder)
		{
			return await builder.UseAngleOcelot(new OcelotPipelineConfiguration());
		}

		public static async Task<IApplicationBuilder> UseAngleOcelot(this IApplicationBuilder builder,Action<OcelotPipelineConfiguration> pipelineConfiguration)
		{
			var configuration = new OcelotPipelineConfiguration();
			pipelineConfiguration?.Invoke(configuration);

			return await builder.UseAngleOcelot(configuration);
		}

		public static async Task<IApplicationBuilder> UseAngleOcelot(this IApplicationBuilder builder,OcelotPipelineConfiguration pipelineConfiguration)
		{
			//重写创建配置方法
			var configuration = await CreateConfiguration(builder);

			ConfigureDiagnosticListener(builder);

			return  builder.CreateOcelotPipeline(pipelineConfiguration);
		}

		private static async Task<IInternalConfiguration> CreateConfiguration(IApplicationBuilder builder)
		{
			//提取文件配置信息
			var fileConfig = await builder.ApplicationServices.GetService<IFileConfigurationRepository>().Get();
			var internalConfigCreator = builder.ApplicationServices.GetService<IInternalConfigurationCreator>();
			var internalConfig = await internalConfigCreator.Create(fileConfig.Data);
			//如果配置文件错误直接抛出异常
			if (internalConfig.IsError)
			{
				ThrowToStopOcelotStarting(internalConfig);
			}

			//TODO:配置信息缓存
			var internalConfigRepo = builder.ApplicationServices.GetService<IInternalConfigurationRepository>();
			internalConfigRepo.AddOrReplace(internalConfig.Data);
			return GetOcelotConfigAndReturn(internalConfigRepo);
		}

		public static IApplicationBuilder CreateOcelotPipeline(this IApplicationBuilder builder, OcelotPipelineConfiguration pipelineConfiguration)
		{
			var pipelineBuilder = new OcelotPipelineBuilder(builder.ApplicationServices);

			pipelineBuilder.BuildOcelotPipeline(pipelineConfiguration);

			var firstDelegate = pipelineBuilder.Build();

			/*
            inject first delegate into first piece of asp.net middleware..maybe not like this
            then because we are updating the http context in ocelot it comes out correct for
            rest of asp.net..
            */

			builder.Properties["analysis.NextMiddlewareName"] = "TransitionToOcelotMiddleware";

			builder.Use(async (context, task) =>
			{
				var downstreamContext = new DownstreamContext(context);
				await firstDelegate.Invoke(downstreamContext);
			});

			return builder;
		}

		private static IInternalConfiguration GetOcelotConfigAndReturn(IInternalConfigurationRepository provider)
		{
			var ocelotConfiguration = provider.Get();

			if (ocelotConfiguration?.Data == null || ocelotConfiguration.IsError)
			{
				ThrowToStopOcelotStarting(ocelotConfiguration);
			}

			return ocelotConfiguration.Data;
		}

		private static void ThrowToStopOcelotStarting(Response config)
		{
			throw new Exception($"Unable to start Ocelot, errors are: {string.Join(",", config.Errors.Select(x => x.ToString()))}");
		}

		private static void ConfigureDiagnosticListener(IApplicationBuilder builder)
		{
			var env = builder.ApplicationServices.GetService<IHostingEnvironment>();
			var listener = builder.ApplicationServices.GetService<OcelotDiagnosticListener>();
			var diagnosticListener = builder.ApplicationServices.GetService<DiagnosticListener>();
			diagnosticListener.SubscribeWithAdapter(listener);
		}

	}

}
