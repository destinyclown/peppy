using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;

namespace Peppy.Swagger
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwagger(
            this IApplicationBuilder app,
            Action<SwaggerOptions> options
        ) => app.UseSwagger()
            .UseSwaggerUI(c =>
            {
                if (options == null)
                {
                    throw new ArgumentNullException(nameof(options));
                }
                var swaggerOptions = new SwaggerOptions();
                options(swaggerOptions);
                //ApiVersions为自定义的版本枚举
                swaggerOptions.OpenApiInfos
                    .ToList()
                    .ForEach(info =>
                    {
                        //版本控制
                        c.SwaggerEndpoint($"/swagger/{info.Version}/swagger.json", $"{info.Title} {info.Version}");
                    });
            });
    }
}