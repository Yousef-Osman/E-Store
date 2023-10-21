using E_Store.Models.Entities;
using E_Store.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace E_Store.Data;

public static class DatabaseSeeder
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.Roles.AnyAsync())
        {
            await roleManager.CreateAsync(new IdentityRole(nameof(UserRoles.SuperAdmin)));
            await roleManager.CreateAsync(new IdentityRole(nameof(UserRoles.Admin)));
            await roleManager.CreateAsync(new IdentityRole(nameof(UserRoles.User)));
        }
    }

    public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var superAdminUser = new ApplicationUser
        {
            Id = "d1801801-fc4e-4f62-afdd-55bfb3088daf", //optional - you can remove it
            FirstName = "Super",
            LastName = "Admin",
            UserName = "super_admin",
            Email = "superadmin@gmail.com",
            EmailConfirmed = true,
        };

        var user = await userManager.FindByEmailAsync(superAdminUser.Email);

        if (user == null)
        {
            var result = await userManager.CreateAsync(superAdminUser, "#Aaa123");

            if (result.Succeeded)
                await userManager.AddToRoleAsync(superAdminUser, UserRoles.SuperAdmin.ToString());
        }

        //var superAdminRole = await roleManager.FindByNameAsync(UserRoles.SuperAdmin.ToString());
        //await roleManager.AddAllPermissionsToRole(superAdminRole);
    }

    //public static async Task AddAllPermissionsToRole(this RoleManager<IdentityRole> roleManager, IdentityRole role)
    //{
    //    var currentPermissions = await roleManager.GetClaimsAsync(role);
    //    var allPermissions = Permissions.GenerateAllPermissions();

    //    foreach (var permission in allPermissions)
    //    {
    //        if (!currentPermissions.Any(a => a.Type == ClaimType.Permission.ToString() && a.Value == permission))
    //            await roleManager.AddClaimAsync(role, new Claim(ClaimType.Permission.ToString(), permission));
    //    }
    //}

    public static async Task SeedProductDataAsync(ApplicationDbContext context)
    {
        if (!context.Brands.Any())
        {
            var brands = new List<Brand> {
                new Brand { Name = "Lenovo", Description = "Lenovo", ImageUrl = "images/site/brands/lenovo.png" },
                new Brand { Name = "Dell", Description = "Dell", ImageUrl = "images/site/brands/dell.png" },
                new Brand { Name = "HP", Description = "HP", ImageUrl = "images/site/brands/hp.png" },
                new Brand { Name = "Toshiba", Description = "Toshiba", ImageUrl = "images/site/brands/toshiba.png" },
                new Brand { Name = "Asus", Description = "Asus", ImageUrl = "images/site/brands/asus.png" },
                new Brand { Name = "Apple", Description = "Apple", ImageUrl = "images/site/brands/apple.png" },
                new Brand { Name = "Fujitsu", Description = "Fujitsu", ImageUrl = "images/site/brands/fujitsu.png" },
                new Brand { Name = "Samsung", Description = "Samsung", ImageUrl = "images/site/brands/samsung.png" },
                new Brand { Name = "Acer", Description = "Acer", ImageUrl = "images/site/brands/acer.png" },
                new Brand { Name = "Canon", Description = "Canon", ImageUrl = "images/site/brands/canon.png" },
                new Brand { Name = "Sony", Description = "Sony", ImageUrl = "images/site/brands/sony.png" },
                new Brand { Name = "Xiaomi", Description = "Xiaomi", ImageUrl = "images/site/brands/xiaomi.png" },
            };

            context.Brands.AddRange(brands);
        }

        if (!context.Categories.Any())
        {
            var categories = new List<Category> {
                new Category { Name = "PC", Description = "PC", ImageUrl = "images/site/categories/pc.png" },
                new Category { Name = "Laptop", Description = "Laptop", ImageUrl = "images/site/categories/laptop.png"  },
                new Category { Name = "Televisions", Description = "Televisions", ImageUrl = "images/site/categories/tv.png"  },
                new Category { Name = "Cell Phones", Description = "Cell Phones", ImageUrl = "images/site/categories/phone.png"  },
                new Category { Name = "Digital Cameras", Description = "Digital Cameras", ImageUrl = "images/site/categories/camera.png"  },
                new Category { Name = "Tablets", Description = "Tablets", ImageUrl = "images/site/categories/tablet.png"  },
                new Category { Name = "Headphones", Description = "Headphones", ImageUrl = "images/site/categories/headphone.png"  },
                new Category { Name = "Playstration", Description = "Playstration", ImageUrl = "images/site/categories/playstration.png"  },
                new Category { Name = "Smart Watches", Description = "Smart Watches", ImageUrl = "images/site/categories/watch.png"  },
                new Category { Name = "Accessories", Description = "Accessories", ImageUrl = "images/site/categories/Accessories.png"  },
            };

            context.Categories.AddRange(categories);
        }

        if (!context.Products.Any())
        {
            var productsData = File.ReadAllText("./Data/SeedData/Products.json");
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);
            context.Products.AddRange(products);
        }

        if (!context.ProductCategories.Any())
        {
            var productCategoriesData = File.ReadAllText("./Data/SeedData/ProductCategories.json");
            var productCategories = JsonSerializer.Deserialize<List<ProductCategory>>(productCategoriesData);
            context.ProductCategories.AddRange(productCategories);
        }

        if (context.ChangeTracker.HasChanges()) 
            await context.SaveChangesAsync();
    }
}
