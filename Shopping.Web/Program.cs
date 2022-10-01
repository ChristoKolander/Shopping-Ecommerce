using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Shopping.Web.Pages.Auth;
using Shopping.Web.Services;
using Shopping.Web.Services.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tewr.Blazor.FileReader;

namespace Shopping.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient 
                { BaseAddress = new Uri("https://localhost:44386/") }
                );
            
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddAuthorizationCore(options =>
                {
                  options.AddPolicy("AdminRolePolicy",
                       policy => policy.RequireRole("Administrator"));

                  options.AddPolicy("DeleteRolePolicy",
                       policy =>policy.RequireClaim("Delete Role","true"));           
                });


            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
            builder.Services.AddScoped<IManageCartItemsLocalStorageService, ManageCartItemsLocalStorageService>();

            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<RefreshTokenService>();
            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            builder.Services.AddScoped<IAdministrationService, AdministrationService>();

            builder.Services.AddFileReaderService(o => o.UseWasmSharedBuffer = true);

            await builder.Build().RunAsync();
        }
    }
}

