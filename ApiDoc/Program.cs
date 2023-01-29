using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using ApiDoc.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiDoc.Formatter;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Json.Serialization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDoc().AddControllers().AddJsonOptions(configure =>
{
    configure.JsonSerializerOptions.PropertyNamingPolicy = null;
    configure.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
})
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressConsumesConstraintForFormFileParameters = true;
        options.SuppressInferBindingSourcesForParameters = true;
        options.SuppressModelStateInvalidFilter = true;
        options.SuppressMapClientErrors = true;
        options.ClientErrorMapping[404].Link =
            "https://*/404";
    });//接口文档



// 注册Jwt服务
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters()
//    {
//        ValidateIssuer = true, //是否验证Issuer
//        ValidIssuer = configuration["Jwt:Issuer"], //发行人Issuer
//        ValidateAudience = true, //是否验证Audience
//        ValidAudience = configuration["Jwt:Audience"], //订阅人Audience
//        ValidateIssuerSigningKey = true, //是否验证SecurityKey
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])), //SecurityKey
//        ValidateLifetime = true, //是否验证失效时间
//        ClockSkew = TimeSpan.FromSeconds(30), //过期时间容错值，解决服务器端时间不同步问题（秒）
//        RequireExpirationTime = true,
//    };
//    options.Events = new JwtBearerEvents()
//    {
//        OnChallenge = context =>
//        {
//            context.HandleResponse();
//            context.Response.Clear();
//            context.Response.ContentType = "application/json";
//            context.Response.StatusCode = 401;
//            context.Response.WriteAsync(JsonConvert.SerializeObject(new { message = "授权未通过", status = false, code = 401 }));
//            return Task.CompletedTask;
//        }
//    };
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRouting();
app.UseSwaggerDoc();
app.MapSwagger("{documentName}/api-docs");
//app.MapGet("/v3/api-docs/swagger-config", async (httpContext) =>
//{
//    JsonSerializerOptions _jsonSerializerOptions = new()
//    {
//        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
//        IgnoreNullValues = true
//    };
//    _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, false));
//    SwaggerUIOptions _options = new() { };
//    await httpContext.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(_options.ConfigObject, _jsonSerializerOptions));
//});
app.Run();
