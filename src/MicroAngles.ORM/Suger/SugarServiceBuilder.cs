using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace MicroAngels.ORM.Suger
{

	public static class SugarServiceBuilder
	{

		public static IApplicationBuilder UseSugarORM(this IApplicationBuilder app, IApplicationLifetime lifetime, IConfiguration configuration)
		{
			MySqlContext.Current.Initial(configuration);

			lifetime.ApplicationStopped.Register(() =>
			{
				//MySqlContext.Current.Stop();
			});

			return app;
		}


		public static IApplicationBuilder InitTabels(this IApplicationBuilder app, params Type[] tableTypes)
		{
			MySqlContext.Current.InitTables(tableTypes);

			return app;
		}

	}

}
