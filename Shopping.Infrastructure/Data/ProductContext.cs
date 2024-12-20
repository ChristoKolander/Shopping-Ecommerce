using Microsoft.EntityFrameworkCore;
using Shopping.Core.Entities;
using Shopping.Core.Entities.People;
using System.Reflection;


namespace Shopping.Infrastructure.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        { }


        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Customer> Customers { get; set; }

        // Note: EF CORE 8 Feature for JSON inside SQL
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Customer>()
                .OwnsOne(customer => customer.Details, builder =>
                {
                    builder.OwnsMany(b => b.Addresses);
                    builder.OwnsMany(b => b.PhoneNumbers);
                    builder.ToJson();
                });
            
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}