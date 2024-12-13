using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Shopping.Api.Security;
using Shopping.Core.Interfaces;
using Shopping.Api.Logging;
using System.Text;

namespace Shopping.Api.Extensions
{
    public static class ServiceRegistrationExtensions
    {
   
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
                    policy => policy.RequireRole("Administrators"));

                options.AddPolicy("EditRolePolicy", policy =>
                    policy.AddRequirements(new AdminRequirement()));

                //options.AddPolicy("EditRolePolicy", policy => policy.RequireAssertion(context =>
                //    context.User.IsInRole("Administrator") &&
                //    context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true")
                //    ||
                //    context.User.IsInRole("SuperAdmins")
                //    ));

            });

            services.AddSingleton<IAuthorizationHandler, AdminHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();


            #endregion

            services.AddSingleton<ILoggerManager, LoggerManager>();

            return services;

        }
    }
}