﻿using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Shopping.Api.SwaggerOptions

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
                TermsOfService = new Uri("https://cnn.com"),
                Contact = new OpenApiContact
                {
                    Name = "CK",
                    Email = string.Empty,
                    Url = new Uri("https://microsoft.com"),
                },
                License = new OpenApiLicense
                {
                    Name = "Use under LICX",
                    Url = new Uri("https://swagger.io"),
                }
            };

            return info;
        }
    }
}