using System.Collections.Generic;
using Microsoft.OpenApi.Models;

namespace Peppy.Swagger
{
    public class SecurityOptions
    {
        public SecurityOptions()
        {
            Name = "Bearer";

            OpenApiSecurityScheme = new OpenApiSecurityScheme
            {
                Description = "JWT Bearer 授权 \"Authorization:     Bearer+空格+token\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            };
        }

        public string Name { get; set; }

        public OpenApiSecurityScheme OpenApiSecurityScheme { get; set; }

        public IEnumerable<string> Scopes { get; set; }
    }
}