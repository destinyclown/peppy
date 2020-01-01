using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System;
using Peppy.Core;

namespace Peppy.Swagger
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPeppySwagger(
            this IServiceCollection services
        ) => services
            .AddSwaggerGen(options =>
            {
                var apiInfo = ApiInfo.Instance;

                options.DescribeAllEnumsAsStrings();

                options.SwaggerDoc(apiInfo.Version, new Info
                {
                    Title = apiInfo.SwaggerInfo.Title,
                    Version = apiInfo.SwaggerInfo.Version,
                    Description = apiInfo.SwaggerInfo.Description
                });

                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Bearer 授权 \"Authorization:     Bearer+空格+token\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                foreach (var xmlFile in apiInfo.SwaggerInfo.XmlFiles)
                {
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    if (File.Exists(xmlPath))
                    {
                        options.IncludeXmlComments(xmlPath);
                    }
                }
                options.DocumentFilter<LowerCaseDocumentFilter>();
                options.OperationFilter<AuthorizeCheckOperationFilter>(apiInfo);
            });

        public static IApplicationBuilder UsePeppySwagger(
            this IApplicationBuilder app,
            IApiInfo apiInfo
            ) => app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
            })
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/doc/{apiInfo.SwaggerInfo.Title}/swagger.json", $"{apiInfo.SwaggerInfo.Title} {apiInfo.SwaggerInfo.Version}");
            });
    }
}