using MicroAngels.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace MicroAngels.Swagger
{
	public static class SwaggerServiceCollection
    {

        public static IServiceCollection AddSwaggerService(this IServiceCollection services, SwaggerService swaggerService,Action<SwaggerGenOptions> customOption=null)
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

				if (customOption != null)
				{
					customOption.Invoke(opt);
				}
			});

            return services;
        }

        public static IApplicationBuilder UseSwaggerService(this IApplicationBuilder builder, SwaggerService swaggerService, SwaggerUIOptions uiOptions)
        {
            builder.UseSwagger()
             .UseSwaggerUI(options =>
             {
				 if (uiOptions.IsShowExtensions)
					 options.ShowExtensions();
				 options.SwaggerEndpoint($"/swagger/{swaggerService.Name}/swagger.json", swaggerService.Name);
             });

            return builder;
        }
    }

}
