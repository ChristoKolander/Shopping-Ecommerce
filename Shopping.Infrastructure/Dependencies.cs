using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Shopping.Infrastructure.Data;
using Shopping.Infrastructure.Identity;

namespace Shopping.Infrastructure
{
    public static class Dependencies
    {
        public static void ConfigureDBServices(IConfiguration configuration, IServiceCollection services)
        {
            var useOnlyInMemoryDatabase = false;
            if (configuration["UseOnlyInMemoryDatabase"] != null)
            {
                useOnlyInMemoryDatabase = bool.Parse(configuration["UseOnlyInMemoryDatabase"]);
            }

            if (useOnlyInMemoryDatabase)
            {
                services.AddDbContext<ProductContext>(c =>
                   c.UseInMemoryDatabase("Product"));

                services.AddDbContext<AppIdentityDbContext>(options =>
                    options.UseInMemoryDatabase("Identity"));
            }
            else
            {
                // use real database
                services.AddDbContext<ProductContext>(c =>
                    c.UseSqlServer(configuration.GetConnectionString("ProductConnectionShoppingGit")));

                // Add Identity DbContext
                services.AddDbContext<AppIdentityDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("IdentityConnectionShoppingGit")));
            }

        }
    }
}
