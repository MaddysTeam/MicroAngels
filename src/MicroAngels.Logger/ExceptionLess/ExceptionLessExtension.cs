using Exceptionless;
using MicroAngels.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MicroAngels.Logger.ExceptionLess
{

	public static class ExceptionLessExtension
	{

		public static IServiceCollection AddLessLog(this IServiceCollection services)
		{
			services.AddSingleton<ILogger, ExceptionLessLogger>();
			services.AddTransient<IFilterLogExecutor, ExcepitonLessExecutor>();
			services.AddScoped<LoggerAttribute>();

			return services;
		}

		public static IApplicationBuilder UseLessLog(this IApplicationBuilder builder, ExcepitonLessOptions options)
		{
			options.EnsureNotNull(() => new AngleExceptions("options cannot be null"));

			ExceptionlessClient.Default.SubmittingEvent += Default_SubmittingEvent;

			builder.UseExceptionless(options.Appkey);

			return builder;
		}

		private static void Default_SubmittingEvent(object sender, EventSubmittingEventArgs e)
		{
			// global submit event
		}
	}

}
