using Domain.Contracts;
using Domain.Entities.IdentityModule;
using Domain.Entities.OrderModule;
using Domain.Entities.ProductModule;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presistence.Data
{
    public class DataSeeding(AppDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : IDataSeeding
    {
        public void SeedData()
        {
            try
            {
                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }
                if (!dbContext.ProductBrands.Any())
                {
                    // .. to handle any path before the \\Infrastructure
                    var productsBrandData = File.ReadAllText("..\\Infrastructure\\Presistence\\Data\\DataSeed\\brands.json");
                    // json => C# Object 
                    var productBrands = JsonSerializer.Deserialize<List<ProductBrand>>(productsBrandData);
                    if (productBrands is not null && productBrands.Any())
                    {
                        dbContext.ProductBrands.AddRange(productBrands);
                    }
                }
                if (!dbContext.ProductTypes.Any())
                {
                    var productTypesData = File.ReadAllText("..\\Infrastructure\\Presistence\\Data\\DataSeed\\types.json");
                    var productsTypes = JsonSerializer.Deserialize<List<ProductType>>(productTypesData);
                    if (productsTypes is not null && productsTypes.Any())
                    {
                        dbContext.ProductTypes.AddRange(productsTypes);
                    }
                }
                if (!dbContext.Products.Any())
                {
                    var productData = File.ReadAllText("..\\Infrastructure\\Presistence\\Data\\DataSeed\\products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productData);
                    if (products is not null && products.Any())
                    {
                        dbContext.Products.AddRange(products);
                    }
                }


                if (!dbContext.DeliveryMethods.Any())
                {
                    var deliveryMethodsData = File.ReadAllText("..\\Infrastructure\\Presistence\\Data\\DataSeed\\delivery.json");
                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);
                    if (deliveryMethods is not null && deliveryMethods.Any())
                    {
                        dbContext.DeliveryMethods.AddRange(deliveryMethods);
                    }
                }
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task SeedIdentityDataAsync()
        {
            if(!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }

            if(!userManager.Users.Any())
            {
                var adminUser = new ApplicationUser
                {
                    DisplayName = "Admin",
                    UserName = "Admin",
                    Email = "Admin@gmail.com",
                    PhoneNumber = "01234567890"
                };
                var superAdminUser = new ApplicationUser
                {
                    DisplayName = "SuperAdmin",
                    UserName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    PhoneNumber = "01134567890"
                };
                await userManager.CreateAsync(adminUser, "P@ssw0rd");
                await userManager.CreateAsync(superAdminUser, "P@ssw0rd");

                await userManager.AddToRoleAsync(adminUser, "Admin");
                await userManager.AddToRoleAsync(adminUser, "SuperAdmin");
            }
        }
    }
}
