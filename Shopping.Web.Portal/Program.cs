using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Shopping.Web.Portal;
using Shopping.Web.Portal.Pages.Auth;
using Shopping.Web.Portal.Services;
using Shopping.Web.Portal.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("  https://localhost:7200/") });

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, ShoppingCartService>();
builder.Services.AddScoped<IManageCartItemsLocalStorageService, ManageCartItemsLocalStorageService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<RefreshTokenService>();

builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("AdminRolePolicy",
         policy => policy.RequireRole("Administrators"));

    options.AddPolicy("DeleteRolePolicy",
         policy => policy.RequireClaim("Delete Role", "true"));
});
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped(sp => (AuthStateProvider)sp.GetRequiredService<AuthenticationStateProvider>());


builder.Services.AddScoped<IAdministrationService, AdministrationService>();


await builder.Build().RunAsync();
