using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping.Core.Entities;

namespace Shopping.Infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            builder.Property(ci => ci.Id)
                .UseHiLo("catalog_hilo")
                .IsRequired();

            builder.Property(ci => ci.Name)
                .IsRequired(true)
                .HasMaxLength(50);

            builder.Property(ci => ci.Price)
                .IsRequired(true)
                .HasColumnType("decimal(18,2)");
        

            builder.Property(ci => ci.Description)
                .IsRequired(true)
                .HasMaxLength(150);

            builder.Property(ci => ci.ImageURL)
                .IsRequired(false);


        }


    }
}

