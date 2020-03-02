using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Peppy.Swagger.Filters
{
    internal sealed class LowerCaseDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            //swaggerDoc.Paths = swaggerDoc.Paths.ToDictionary(x => x.Key.ToLower(), x => x.Value);
        }
    }
}