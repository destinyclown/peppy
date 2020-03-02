using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System;
using System.Linq;
using Peppy.Swagger.Filters;

namespace Peppy.Swagger
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwagger(
            this IServiceCollection services, Action<SwaggerOptions> options, Action<SecurityOptions> action = null, Type codeEnumType = null
        ) => services
            .AddSwaggerGen(c =>
            {
                if (options == null)
                {
                    throw new ArgumentNullException(nameof(options));
                }
                var swaggerOptions = new SwaggerOptions();
                options(swaggerOptions);
                services.Configure(options);
                swaggerOptions.OpenApiInfos
                    .ToList().
                    ForEach(info =>
                    {
                        c.SwaggerDoc(info.Version, info);
                    });
                swaggerOptions.Files
                    .ToList()
                    .ForEach(xmlFile =>
                    {
                        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                        if (File.Exists(xmlPath))
                        {
                            c.IncludeXmlComments(xmlPath);
                        }
                    });
                c.DocumentFilter<LowerCaseDocumentFilter>();
                c.OperationFilter<StatusCodeOperationFilter>(codeEnumType);
                if (action == null) return;
                var securityOptions = new SecurityOptions();
                action(securityOptions);
                c.AddSecurityDefinition(securityOptions.Name, securityOptions.OpenApiSecurityScheme);
                c.OperationFilter<AuthorizeCheckOperationFilter>(securityOptions);
            });
    }
}