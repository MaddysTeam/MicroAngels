using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace MicroAngels.Bus.CAP
{

    public static class CAPConfiguration
    {

        public static IServiceCollection AddCapService(this IServiceCollection services, CapService capService)
        {
            services.AddSingleton(capService);
            services.AddDbContext<CAPMysqlDbContext>()  
                    .AddCap(x =>
                    {
                        x.UseEntityFramework<CAPMysqlDbContext>()
                         .UseKafka(capService.Host)
                         .UseDashboard();
                    });

            return services;
        }

    }

    public class CAPMysqlDbContext : DbContext
    {
        private CapService _capService;

        public CAPMysqlDbContext(CapService capService)
        {
            _capService = capService;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_capService.ConnectString);
        }
    }

}
