using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Shopping.Api.Data
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {

            //builder.HasData(
            //    new IdentityRole
            //    {
            //        Name = "Viewer",
            //        NormalizedName = "VIEWER"
            //    },
            //    new IdentityRole
            //    {
            //        Name = "standardUser",
            //        NormalizedName = "STANDARDUSER"
            //    },
            //     new IdentityRole
            //     {
            //         Name = "manager",
            //         NormalizedName = "MANAGER"
            //     },
            //    new IdentityRole
            //    {
            //        Name = "Administrator",
            //        NormalizedName = "ADMINISTRATOR"
            //    }
            //);

        }
    }
}
