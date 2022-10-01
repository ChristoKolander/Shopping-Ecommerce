using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Shopping.Api.AutoMapper;
using Shopping.Api.Data;
using Shopping.Api.Entities;
using Shopping.Api.Extensions;
using Shopping.Api.LoggerService;
using System.IO;
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

            //Strange behavoir if NOT added first in this "bucket"!
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
                                       (Configuration.GetConnectionString("ShoppingConnection4")));

            #endregion

            services.AddServiceRegistration(Configuration);  

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
            app.UseHttpsRedirection();         
            app.UseCors("CorsPolicy");
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"StaticFiles")),
                RequestPath = new PathString("/StaticFiles")
            });
            app.UseRouting();
            app.UseResponseCompression();
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
