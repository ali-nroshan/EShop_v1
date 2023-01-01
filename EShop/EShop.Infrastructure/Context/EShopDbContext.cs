using EShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EShop.Infrastructure.Context
{
    public class EShopDbContext : DbContext
    {
        public EShopDbContext(DbContextOptions<EShopDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.Entity<CategoryToProduct>()
                .HasKey(c => new { c.CategoryId, c.ProductId });

            _ = modelBuilder.Entity<ProductToWishList>()
                .HasKey(w => new { w.ProductId, w.UserId });

            _ = modelBuilder.Entity<EShopUser>().HasKey(e => e.Id);

            _ = modelBuilder.Entity<Product>(pi =>
            {
                pi.Property(t => t.ProductPrice).HasColumnType("Money");
                pi.HasKey(k => k.ProductId);
            });

            _ = modelBuilder.Entity<OrderDetail>(od =>
            {
                od.Property(p => p.Price).HasColumnType("Money");
                od.HasKey(p => p.Id);
            });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<EShopUser> EShopUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<CategoryToProduct> CategoryToProducts { get; set; }
        public DbSet<ProductToWishList> ProductToWishLists { get; set; }
    }
}
