using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace MicroAngels.Bus.CAP
{

    public static class CAPConfiguration
    {

        public static IServiceCollection AddCapService(this IServiceCollection services, CapService capService)
        {
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

        public CAPMysqlDbContext(CapService config)
        {
            _capService = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_capService.ConnectString);
        }
    }

}
