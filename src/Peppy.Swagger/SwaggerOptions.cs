using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace Peppy.Swagger
{
    public class SwaggerOptions
    {
        public IEnumerable<OpenApiInfo> OpenApiInfos { get; set; }

        public IEnumerable<string> Files { get; set; }
    }
}