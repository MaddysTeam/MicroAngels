using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Orms.Sugar
{

    public static class SugarServiceBuilder
    {

        public static IApplicationBuilder RegisterMysqlBySugar(this IApplicationBuilder app, IApplicationLifetime lifetime, IConfiguration configuration)
        {
            MySqlDbContext.Current.Initial(configuration);

            //lifetime.ApplicationStopped.Register(() =>
            //{
            //    MySqlDbContext.Current.Stop();
            //});

            return app;
        }

    }

}
