using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure
{

    public static class CAPConfiguration
    {

        public static IServiceCollection AddCapWithMySQLAndRabbit(this IServiceCollection services, IConfiguration conifg)
        {
            services.AddDbContext<CAPSqlServerDbContenxt>();

            // add cap
            services.AddCap(x =>
            {
                x.UseEntityFramework<CAPSqlServerDbContenxt>();
                // x.UseEntityFramework<CAPDbContext>();
                // x.UseRabbitMQ("localhost");

                x.UseKafka("192.168.1.8");

                x.UseDashboard();
               
            });

            return services;
        }

    }


    public class CAPDbContext : DbContext
    {

        private IConfiguration _config;

        public CAPDbContext(IConfiguration config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           // optionsBuilder.UseMySql(_config.GetSection("ConnectStrings:MySql").Value);
        }
    }


    public class CAPSqlServerDbContenxt : DbContext
    {
        private IConfiguration _config;

        public CAPSqlServerDbContenxt(IConfiguration config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetSection("ConnectStrings:SqlServer").Value);
        }

    }


}
