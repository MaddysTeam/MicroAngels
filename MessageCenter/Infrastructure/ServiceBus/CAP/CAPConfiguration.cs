using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure
{

    public static class CAPConfiguration
    {

        public static IServiceCollection AddCapWithMySQLAndRabbit(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<CAPMysqlDbContext>();

            // add cap
            services.AddCap(x =>
            {
                x.UseEntityFramework<CAPMysqlDbContext>();

                x.UseKafka(config["Queues:Kafka:Host"]);

                x.UseDashboard();
               
            });

            return services;
        }

    }


    public class CAPMysqlDbContext : DbContext
    {

        private IConfiguration _config;

        public CAPMysqlDbContext(IConfiguration config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_config.GetSection("Database:MySql:Conn").Value);
        }
    }


    //public class CAPSqlServerDbContenxt : DbContext
    //{
    //    private IConfiguration _config;

    //    public CAPSqlServerDbContenxt(IConfiguration config)
    //    {
    //        _config = config;
    //    }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        optionsBuilder.UseSqlServer(_config.GetSection("ConnectStrings:SqlServer").Value);
    //    }

    //}


}
