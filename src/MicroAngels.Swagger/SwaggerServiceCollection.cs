using MicroAngels.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Swagger
{
    public static class SwaggerServiceCollection
    {

        public static IServiceCollection AddSwaggerService(this IServiceCollection services, SwaggerService swaggerService)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc(swaggerService.Name, new Info
                {
                    Title = swaggerService.Title,
                    Version = swaggerService.Version,
                    Description = swaggerService.Doc
                });

                if (!swaggerService.XMLPath.IsNullOrEmpty())
                {
                    opt.IncludeXmlComments(swaggerService.XMLPath);
                }

            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerService(this IApplicationBuilder builder, SwaggerService swaggerService, SwaggerUIOptions uiOptions)
        {
            builder.UseSwagger(options =>
            {
                options.RouteTemplate = "{documentName}/swagger.json";
            })
             .UseSwaggerUI(options =>
             {
                 if (uiOptions.IsShowExtensions)
                     options.ShowExtensions();
                 options.SwaggerEndpoint($"/{swaggerService.Name}/swagger.json", swaggerService.Name);
             });

            return builder;
        }
    }



}
