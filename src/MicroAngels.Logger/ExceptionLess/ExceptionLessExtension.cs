using Exceptionless;
using MicroAngels.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MicroAngels.Logger.ExceptionLess
{

	public static class ExceptionLessExtension
	{

		public static IServiceCollection AddLessLog(IServiceCollection services)
		{
			services.AddSingleton<ILogger, ExceptionLessLogger>();
			services.AddTransient<IFilterLogExecutor, DefaultLoggerExecutor>();

			return services;
		}

		public static IApplicationBuilder UseLessLog(this IApplicationBuilder builder, ExcepitonLessOptions options)
		{
			options.EnsureNotNull(() => new ArgumentNullException());

			ExceptionlessClient.Default.SubmittingEvent += Default_SubmittingEvent;

			builder.UseExceptionless(options.AppKey);

			return builder;
		}

		private static void Default_SubmittingEvent(object sender, EventSubmittingEventArgs e)
		{
			// global submit event
		}
	}

}
