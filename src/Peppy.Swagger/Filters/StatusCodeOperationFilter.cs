using System;
using System.ComponentModel;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Peppy.Swagger.Filters
{
    internal class StatusCodeOperationFilter : IOperationFilter
    {
        private readonly Type _type;

        public StatusCodeOperationFilter(Type type)
        {
            _type = type;
        }

        public void Apply(
            OpenApiOperation operation,
            OperationFilterContext context
        )
        {
            if (!_type.IsEnum)
            {
                throw new TypeLoadException("The status code must be an enumerated type");
            }
            foreach (int value in Enum.GetValues(_type))
            {
                var name = Enum.GetName(_type, value);
                // 获取枚举字段。
                var fieldInfo = _type.GetField(name);
                if (fieldInfo == null) return;
                // 获取描述的属性。
                var attr = Attribute.GetCustomAttribute(fieldInfo,
                    typeof(DescriptionAttribute), false) as DescriptionAttribute;
                if (!operation.Responses.ContainsKey(value.ToString()))
                {
                    operation.Responses.Add(value.ToString(), new OpenApiResponse { Description = attr?.Description ?? value.ToString() });
                }
            }
        }
    }
}