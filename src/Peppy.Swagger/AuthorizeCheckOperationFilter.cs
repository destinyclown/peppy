using System.Collections.Generic;
using System.Linq;
using Peppy.Core;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Peppy.Swagger
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        private readonly IApiInfo _apiInfo;

        public AuthorizeCheckOperationFilter(IApiInfo apiInfo)
        {
            _apiInfo = apiInfo;
        }

        public void Apply(
            Operation operation,
            OperationFilterContext context
        )
        {
            if (!context.HasAuthorize()) return;

            operation.Responses.Add("401", new Response { Description = "未授权访问" });
            operation.Responses.Add("403", new Response { Description = "不允许访问" });

            operation.Security = new List<IDictionary<string, IEnumerable<string>>>
            {
                new Dictionary<string, IEnumerable<string>>
                {
                    {"oauth2", _apiInfo.Scopes }
                }
            };
        }
    }
}
