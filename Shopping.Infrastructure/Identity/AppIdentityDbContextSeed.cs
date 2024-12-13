using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopping.Core.Constants;
using System.Threading.Tasks;

namespace Shopping.Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(AppIdentityDbContext identityDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            if (identityDbContext.Database.IsSqlServer())
            {
                identityDbContext.Database.Migrate();
            }

            await roleManager.CreateAsync(new IdentityRole(Shopping.Shared.Authorization.Constants.Roles.ADMINISTRATORS));
            await roleManager.CreateAsync(new IdentityRole(Shopping.Shared.Authorization.Constants.Roles.STANDARDUSERS));
            await roleManager.CreateAsync(new IdentityRole(Shopping.Shared.Authorization.Constants.Roles.MANAGERS));
          //await roleManager.CreateAsync(new IdentityRole(Shopping.Shared.Authorization.Constants.Roles.SUPERADMINS));


            string defaultUserName = "demouser@microsoft.com";
            var defaultUser = new ApplicationUser { UserName = defaultUserName, Email = defaultUserName };
            await userManager.CreateAsync(defaultUser, AuthorizationSettings.DEFAULT_PASSWORD);
            defaultUser = await userManager.FindByNameAsync(defaultUserName);
            await userManager.AddToRoleAsync(defaultUser, Shopping.Shared.Authorization.Constants.Roles.STANDARDUSERS);


            string adminUserName = "admin@microsoft.com";
            var adminUser = new ApplicationUser { UserName = adminUserName, Email = adminUserName };
            await userManager.CreateAsync(adminUser, AuthorizationSettings.DEFAULT_PASSWORD);
            adminUser = await userManager.FindByNameAsync(adminUserName);
            await userManager.AddToRoleAsync(adminUser, Shopping.Shared.Authorization.Constants.Roles.ADMINISTRATORS);

            //string superAdminUserName = "superadmin@microsoft.com";
            //var superadminUser = new ApplicationUser { UserName = superAdminUserName, Email = superAdminUserName };
            //await userManager.CreateAsync(superadminUser, AuthorizationSettings.DEFAULT_PASSWORD);
            //superadminUser = await userManager.FindByNameAsync(superAdminUserName);
            //await userManager.AddToRoleAsync(superadminUser, Shopping.Shared.Authorization.Constants.Roles.SUPERADMINS);
        }

    }
}
