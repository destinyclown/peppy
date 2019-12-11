using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Peppy.Swagger
{
    internal static class OperationFilterContextExtensions
    {
        internal static bool HasAuthorize(this OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;
            return
                apiDescription.TryGetMethodInfo(out _);

        }
    }
}
