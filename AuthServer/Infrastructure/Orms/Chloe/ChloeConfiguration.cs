//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;

//namespace Infrastructure
//{

//    public static class ChloeConfiguration 
//    {

//        public static IServiceCollection AddChloeWithMySQL(this IServiceCollection services,IConfiguration configuration)
//        {
//            services.AddScoped(provider => {
//                string connString = configuration.GetSection("ConnectStrings:MySql").Value;

//                return new Chloe.MySql.MySqlContext(new MysqlConnectionFactory(connString));
//            });

//            return services;
//        }

//    }

//}