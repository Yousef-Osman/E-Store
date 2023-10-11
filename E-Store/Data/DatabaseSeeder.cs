using E_Store.Models.Entities;
using E_Store.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
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
                new Brand { Name = "Dell", Description = "Dell" },
                new Brand { Name = "HP", Description = "HP" },
                new Brand { Name = "Lenovo", Description = "Lenovo" },
                new Brand { Name = "Toshiba", Description = "Toshiba" },
                new Brand { Name = "Asus", Description = "Asus" },
                new Brand { Name = "Apple", Description = "Apple" },
                new Brand { Name = "Samsung", Description = "Samsung" },
            };

            context.Brands.AddRange(brands);
        }

        if (!context.Categories.Any())
        {
            var categories = new List<Category> {
                new Category { Name = "PC", Description = "PC" },
                new Category { Name = "Laptop", Description = "Laptop" },
                new Category { Name = "Accessories", Description = "Accessories" },
            };

            context.Categories.AddRange(categories);
        }

        if (!context.Products.Any())
        {
            var productsData = File.ReadAllText("./Data/SeedData/Products.json");
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);
            context.Products.AddRange(products);
        }

        if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
    }
}
