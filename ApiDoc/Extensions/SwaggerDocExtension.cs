using ApiDoc.Config;
using ApiDoc.Filter;
using KnifeUI.Swagger.Net;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ApiDoc.Extensions
{
    /// <summary>
    /// 接口文档生成
    /// </summary>
    public static class SwaggerDocExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerDoc(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
				c.SwaggerDoc("v0", new OpenApiInfo
				{
					Title = "公用接口",
					Version = "v0",
					Description = "",
					Contact = new OpenApiContact { Name = "WebAdmin", Email = "" },
				});
				c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "浙里办接口",
                        Version = "v1",
                        Description = "",
                        Contact = new OpenApiContact { Name = "WebAdmin", Email = "" },
                    });
                c.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "浙政钉接口",
                    Description = "",
                    Contact = new OpenApiContact { Name = "WebAdmin", Email = "" },
                });
                c.SwaggerDoc("v3", new OpenApiInfo
                {
                    Version = "v3",
                    Title = "教育培训接口",
                    Description = "",
                    Contact = new OpenApiContact { Name = "WebAdmin", Email = "" },
                });
				c.SwaggerDoc("v4", new OpenApiInfo
				{
					Version = "v4",
					Title = "驾驶舱接口",
					Description = "",
					Contact = new OpenApiContact { Name = "WebAdmin", Email = "" },
				});
				c.AddServer(new OpenApiServer()
                {
                    Url = "",
                    Description = "vvv"
                });
                c.CustomOperationIds(apiDesc =>
                {
                    var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                    return controllerAction.ControllerName + "-" + controllerAction.ActionName;
                });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.OperationFilter<HttpHeaderOperationFilter>();
                c.DocumentFilter<HttpHeaderDocmentFilter>();//隐藏具体Api
                var security = new Dictionary<string, IEnumerable<string>>
                { { "", Array.Empty<string>() }};
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT授权token前面需要加上字段Bearer与一个空格,如Bearer token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.OAuth2,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                // 使用反射获取xml文件。并构造出文件的路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // 启用xml注释. 该方法第二个参数启用控制器的注释，默认为false.
                c.IncludeXmlComments(xmlPath, true);
            });
            return services;
        }

        public static IApplicationBuilder UseSwaggerDoc(this IApplicationBuilder app)
        {
            // 启用Swagger中间件
            app.UseSwagger();
            // 配置SwaggerUI
            app.UseKnife4UI(c =>
            {
                c.RoutePrefix = "";
                c.DocumentTitle = "WebAPI";
				c.SwaggerEndpoint($"v0/api-docs", $"公用接口");
				c.SwaggerEndpoint($"v1/api-docs", $"浙里办接口");
                c.SwaggerEndpoint($"v2/api-docs", $"浙政钉接口");
                c.SwaggerEndpoint($"v3/api-docs", $"教育培训接口");
				c.SwaggerEndpoint($"v4/api-docs", $"驾驶舱接口");
			});
            return app;
        }
    }
}
