using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Shopping.Api.AutoMapper;
using Shopping.Api.Data;
using Shopping.Api.Entities;
using Shopping.Api.Extensions;
using Shopping.Api.LoggerService;
using Shopping.Api.Repositories;
using Shopping.Api.Repositories.Interfaces;
using Shopping.Api.Security;
using Shopping.Api.SwaggerOpt;
using Shopping.Api.TokenHelpers;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shopping.Api
{
    public class Startup
    {

        #region Fields and Configuration

            private static string _policyName = "CorsPolicy";

            public IConfiguration Configuration { get; }

            public Startup(IConfiguration configuration)
            {
                Configuration = configuration;
            }

        #endregion

        public void ConfigureServices(IServiceCollection services)
        {

        #region Controllers EF DB

            services.AddIdentity<ApplicationUser, IdentityRole>()
                      .AddEntityFrameworkStores<ShoppingDbContext>();

            services.AddControllers()
                               .AddJsonOptions(options =>
                               {
                                   options.JsonSerializerOptions.IgnoreNullValues = true;
                                   options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                                   options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                                   options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                                   options.JsonSerializerOptions.WriteIndented = true;
                               });

            services.AddDbContextPool<ShoppingDbContext>(options =>
                                   options.UseSqlServer
                                       (Configuration.GetConnectionString("ShoppingConnection3")));

        #endregion  

        #region Auth and Auth

            var jwtSettings = Configuration.GetSection("JWTSettings");

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


        }
      
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
                              ILoggerManager logger)

        #region Request PipeLine

        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                                $"/swagger/{description.GroupName}/swagger.json",
                                description.ApiVersion.ToString()
                                );
                    }

                });
                
            }

            app.UseConfigureExceptionHandler(logger);

            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"StaticFiles")),
            //    RequestPath = new PathString("/StaticFiles")
            //});

            app.UseHttpsRedirection();         
            app.UseCors("CorsPolicy");
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        #endregion

    }
}
