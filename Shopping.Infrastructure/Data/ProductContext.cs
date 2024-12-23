using Microsoft.EntityFrameworkCore;
using Shopping.Core.Entities;
using Shopping.Core.Entities.People;
using System;
using System.Reflection;
using System.Threading;


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

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // JSON column and Arrays in SQL           
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Customer>()
                .OwnsOne(customer => customer.Details, builder =>
                {
                    builder.OwnsMany(b => b.Addresses);
                    builder.OwnsMany(b => b.PhoneNumbers);
                    builder.ToJson();
                });

            // Shadow Property
            modelBuilder.Entity<Product>()
                        .Property<DateTime>("LastUpdated");


            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }

        // Because of using Shadow Property, we override ( But not to be used inside large batches/loops )
        // Need to clear from tracking inside these loops, perf. issue.
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    entry.Property("LastUpdated").CurrentValue = DateTime.UtcNow;
                }
            }
            return base.SaveChanges();
        }
    }
}