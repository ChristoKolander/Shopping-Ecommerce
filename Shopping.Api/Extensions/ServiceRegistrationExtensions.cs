using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shopping.Api.AutoMapper;
using Shopping.Api.LoggerService;
using Shopping.Api.Repositories;
using Shopping.Api.Repositories.Interfaces;
using Shopping.Api.Security;
using Shopping.Api.SwaggerOpt;
using Shopping.Api.TokenHelpers;
using System.IO.Compression;
using System.Text;

namespace Shopping.Api.Extensions
{
    public static class ServiceRegistrationExtensions
    {
        private static string _policyName = "CorsPolicy";

        public static IServiceCollection AddServiceRegistration(this IServiceCollection services, IConfiguration config)
        {
          
            #region Auth and Auth

            var jwtSettings = config.GetSection("JWTSettings");

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtSettings["validIssuer"],
                            ValidAudience = jwtSettings["validAudience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["securityKey"]))
                        };

                    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy",
                    policy => policy.RequireClaim("Delete Role", "true"));

                options.AddPolicy("AdminRolePolicy",
                    policy => policy.RequireRole("Administrator"));

                options.AddPolicy("EditRolePolicy", policy =>
                    policy.AddRequirements(new AdminRequirement()));

                //options.AddPolicy("EditRolePolicy", policy => policy.RequireAssertion(context =>
                //    context.User.IsInRole("Administrator") &&
                //    context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true")
                //    ||
                //    context.User.IsInRole("Super Admin")
                //    ));

            });

            services.AddSingleton<IAuthorizationHandler, AdminHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();

            #endregion

            #region Cors

            services.AddCors(options =>
            {
                options.AddPolicy(name: _policyName,
                   builder =>
                   {
                       builder.AllowAnyHeader()
                              .AllowAnyMethod()
                              //.WithHeaders(HeaderNames.ContentType)
                              .AllowAnyOrigin()
                              .WithExposedHeaders("X-Pagination");
                       //.WithOrigins("http://localhost:47650", "https://localhost:44371");
                   });
            });

            #endregion

            #region Swagger

            services.AddSwaggerGen();

            services.ConfigureOptions<ConfigureSwaggerOptions>();

            //Just for Versioning.
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            //Needed to work with Swagger.
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            #endregion

            #region Injections

            services.AddScoped<ITokenService, TokenService>();

            services.AddSingleton<ILoggerManager, LoggerManager>();

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();


            #endregion

            #region Automapper and Compression 

            services.AddAutoMapper(typeof(ProductProfile));

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });

            #endregion

            return services;

        }
    }
}