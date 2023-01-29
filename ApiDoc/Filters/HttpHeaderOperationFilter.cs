using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ApiDoc.Filter
{
    /// <summary>
    /// swagger请求头
    /// </summary>
    public class HttpHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            #region 新方法
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            if (context.ApiDescription.TryGetMethodInfo(out MethodInfo methodInfo))
            {
                if (!methodInfo.CustomAttributes.Any(t => t.AttributeType == typeof(AllowAnonymousAttribute)) && !methodInfo.ReflectedType.CustomAttributes.Any(t => t.AttributeType == typeof(AllowAnonymousAttribute)))
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Required = true,
                        Description = "请输入Token，格式为bearer XXX"
                    });
                }
            }
            #endregion
        }
    }
}
