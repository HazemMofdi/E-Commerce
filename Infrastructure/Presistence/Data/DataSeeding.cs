using Domain.Contracts;
using Domain.Entities.ProductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presistence.Data
{
    public class DataSeeding(AppDbContext dbContext) : IDataSeeding
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
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
