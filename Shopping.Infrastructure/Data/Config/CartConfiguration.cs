using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping.Core.Entities;

namespace Shopping.Infrastructure.Data.Config
{
    //public class CartConfiguration : IEntityTypeConfiguration<Cart>
    //{
        //public void Configure(EntityTypeBuilder<Cart> builder)
        //{
        //    var navigation = builder.Metadata.FindNavigation(nameof(Cart.CartItems));
        //    navigation?.SetPropertyAccessMode(PropertyAccessMode.PreferProperty);


        //    builder.Property(b => b.UserClaimStringId)
        //        .IsRequired()
        //        .HasMaxLength(256);
        //}
    //}
}