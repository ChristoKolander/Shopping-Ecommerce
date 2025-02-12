﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Shopping.Infrastructure.Data;
using Shopping.Infrastructure.Identity;
using System;
using Microsoft.Extensions.Logging;

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
                services.AddDbContext<ProductContext>(options =>
                   options.UseInMemoryDatabase("Product"));

                services.AddDbContext<AppIdentityDbContext>(options =>
                    options.UseInMemoryDatabase("Identity"));

            }
            else
            {
                // use real database
                services.AddDbContext<ProductContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("ProductConnectionShoppingGit"))
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    );

                // Add Identity DbContext
                services.AddDbContext<AppIdentityDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("IdentityConnectionShoppingGit")));
         
            }

        }
    }
}
