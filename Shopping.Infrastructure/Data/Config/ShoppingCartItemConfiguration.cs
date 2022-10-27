using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping.Core.Entities;

namespace Shopping.Infrastructure.Data.Config
{
    public class ShoppingCartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
        {
            builder.ToTable("ShoppingCartItem");

            builder.Property(bi => bi.Price)
                .IsRequired(true)
                .HasColumnType("decimal(18,2)");

            builder.Property(bi => bi.Qty);

            builder.Property(bi => bi.CartStringId)
               .IsRequired(true);

            builder.Property(bi => bi.ProductId)
                .IsRequired(true);
        }
    }
}


