using E_Store.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace E_Store.Data;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrdersDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<Product>().HasQueryFilter(a => a.IsDeleted == false);
        builder.Entity<Brand>().HasQueryFilter(a => a.IsDeleted == false);
        builder.Entity<Category>().HasQueryFilter(a => a.IsDeleted == false);
        builder.Entity<Order>().HasQueryFilter(a => a.IsDeleted == false);
        builder.Entity<OrderDetail>().HasQueryFilter(a => a.IsDeleted == false);
        builder.Entity<ProductCategory>().HasKey(a => new { a.ProductId, a.CategoryId });
        
        base.OnModelCreating(builder);
    }
}
