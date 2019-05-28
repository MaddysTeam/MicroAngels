using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace MicroAngels.ORM.Suger
{

	public static class SugarServiceBuilder
	{

		public static IApplicationBuilder RegisterMysqlBySugar(this IApplicationBuilder app, IApplicationLifetime lifetime, IConfiguration configuration)
		{
			MySqlContext.Current.Initial(configuration);

			//lifetime.ApplicationStopped.Register(() =>
			//{
			//	MySqlContext.Current.Stop();
			//});

			return app;
		}

	}

}
