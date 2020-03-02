using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Peppy.Swagger.Filters
{
    internal class AuthorizeCheckOperationFilter : IOperationFilter
    {
        private readonly SecurityOptions _apiInfo;

        public AuthorizeCheckOperationFilter(SecurityOptions apiInfo)
        {
            _apiInfo = apiInfo;
        }

        public void Apply(
            OpenApiOperation operation,
            OperationFilterContext context
        )
        {
            if (!context.HasAuthorize()) return;

            operation.Responses.Add("401", new OpenApiResponse() { Description = "未授权访问" });
            operation.Responses.Add("403", new OpenApiResponse { Description = "不允许访问" });

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    //{"oauth2", _apiInfo.Scopes }
                }
            };
        }
    }
}