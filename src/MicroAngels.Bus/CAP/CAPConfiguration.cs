using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace MicroAngels.Bus.CAP
{

	public static class CAPConfiguration
	{

		public static IServiceCollection AddCAPService(this IServiceCollection services, CAPService capService)
		{
			services.AddSingleton(capService);
			// add dbcontext
			services.AddDbContext<CAPMysqlDbContext>();

			// add cap and inject ICapPublisher instance
			services.AddCap(x =>
					{
						x.UseEntityFramework<CAPMysqlDbContext>()
						 .UseKafka(capService.Host)
						 .UseDashboard();
					});

			//TODO: you can replace your own publisher here
			services.AddTransient<ICAPPublisher, CAPMysqlPublisher>();

			return services;
		}


		public static IServiceCollection AddRabbitService(this IServiceCollection services, CAPService capService)
		{
			throw new System.NotImplementedException("rabbit serivce will coming soon");
		}

	}

	public class CAPMysqlDbContext : DbContext
	{
		private CAPService _capService;

		public CAPMysqlDbContext(CAPService capService)
		{
			_capService = capService;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseMySql(_capService.ConnectString);
		}
	}

}
