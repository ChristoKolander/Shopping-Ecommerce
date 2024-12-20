using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Shopping.Core;
using Shopping.Core.Interfaces;
using Shopping.Infrastructure.Data;
using Shopping.Infrastructure.Identity;
using Shopping.Shared;
using Shopping.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.ResponseCompression;
using Shopping.Api.Attributes;
using NLog;
using NLog.Web;
using Shopping.Api.Extensions;
using Shopping.Api.SwaggerOptions;
using MediatR;
using Shopping.Core.Entities.People;


// Note 1: Extracting too many services outside this container, using extensionmethods did not work out as expected.
// Note 2: In some cases, order in here DID matter!

# region Builder and NLog

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseNLog();

# endregion


# region EF, Identity and BaseUrl

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddDefaultTokenProviders();

var configSection = builder.Configuration.GetRequiredSection(BaseUrlConfiguration.CONFIG_NAME);
builder.Services.Configure<BaseUrlConfiguration>(configSection);
var baseUrlConfig = configSection.Get<BaseUrlConfiguration>();


Shopping.Infrastructure.Dependencies.ConfigureDBServices(builder.Configuration, builder.Services);

//builder.Logging.AddConsole();

# endregion


#region Adding ServiceRegistrations

//See folder Extensions for additional injected stuff!
builder.Services.AddServiceRegistration(builder.Configuration);

#endregion


# region Cors

//Configure CORS in Production...
const string CORS_POLICY = "CorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CORS_POLICY,
       builder =>
       {
           builder.AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowAnyOrigin()
                  .WithExposedHeaders("X-Pagination");
       });
});
# endregion


# region Injected Services

builder.Services.AddScoped<ValidationFilterAttribute>();

// Could be or should be Transient?
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();  // Not used at the moment
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.Configure<ProductSettings>(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMediatR(typeof(Program));

builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
});

# endregion


# region SwaggerStuff...

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SchemaFilter<CustomSchemaFilters>();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                          Enter 'Bearer' [space] and then your token in the text input below.
                          Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
            });
});

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

//Just for Versioning.
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

//Needed to work with Swagger.
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

# endregion


# region Request PipeLine

var app = builder.Build();

app.Logger.LogInformation("Shopping.Api App created...");

app.Logger.LogInformation("Seeding database if needed...");

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        var productContext = scopedProvider.GetRequiredService<ProductContext>();
        await ProductContextSeed.SeedAsync(productContext, app.Logger);

        var userManager = scopedProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scopedProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var identityContext = scopedProvider.GetRequiredService<AppIdentityDbContext>();
        
        // Seed Users
        await AppIdentityDbContextSeed.SeedAsync(identityContext, userManager, roleManager);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseConfigureExceptionHandler();

app.UseHttpsRedirection();

app.UseRouting();

app.UseResponseCompression();

app.UseCors(CORS_POLICY);

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"StaticFiles")),
    RequestPath = new PathString("/StaticFiles")
});

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.ApiVersion.ToString()
                );
    }

});

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Logger.LogInformation("LAUNCHING Shopping.Api");
app.Run();

//public partial class Program { }

# endregion





