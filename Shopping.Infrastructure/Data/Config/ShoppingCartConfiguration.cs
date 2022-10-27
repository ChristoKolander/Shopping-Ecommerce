using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping.Core.Entities;

namespace Shopping.Infrastructure.Data.Config
{
    //public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
    //{
        //public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        //{
        //    var navigation = builder.Metadata.FindNavigation(nameof(ShoppingCart.Items));
        //    navigation?.SetPropertyAccessMode(PropertyAccessMode.PreferProperty);


        //    builder.Property(b => b.UserStringId)
        //        .IsRequired()
        //        .HasMaxLength(256);
        //}
    //}
}