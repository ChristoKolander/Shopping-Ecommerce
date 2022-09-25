using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace Shopping.Api.SwaggerOpt
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {

        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }


        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
            }
        }

        private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo

            {
                Version = description.ApiVersion.ToString(),
                Title = "Shopping API Test Site",
                Description = "An ASP.NET Core Web API for Testing/LAB purposes",
                TermsOfService = new Uri("https://localhost:44386/api/v1/product"),
                Contact = new OpenApiContact
                {
                    Name = "CK",
                    Email = string.Empty,
                    Url = new Uri("https://twitter.com/CodeSweder1"),
                },
                License = new OpenApiLicense
                {
                    Name = "Use under LICX",
                    Url = new Uri("https://localhost:44386/api/v1/product"),
                }
            };

            return info;
        }
    }
}