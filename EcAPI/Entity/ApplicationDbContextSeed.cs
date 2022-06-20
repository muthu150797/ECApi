using EcAPI.Entity.OrderAggregrate;
using System.Reflection;
using System.Text.Json;

namespace EcAPI.Entity
{
	public class ApplicationDbContextSeed
	{
        public static async Task SeedAsync(AppIdentityDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                string path2 = Directory.GetCurrentDirectory();

                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (!context.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText(path + @"/DataSeed/SeedData/brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    foreach (var item in brands)
                    {
                        context.ProductBrands.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
                if (!context.ProductType.Any())
                {
                    var typesData = File.ReadAllText(path + @"/DataSeed/SeedData/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    foreach (var item in types)
                    {
                        context.ProductType.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
                if (!context.Products.Any())
                {
                    var productsData = File.ReadAllText(path + @"/DataSeed/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    foreach (var item in products)
                    {
                        context.Products.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
                // seed DeliveryMethods
                if (!context.DeliveryMethods.Any())
                {
                    var dmData = File.ReadAllText(path + @"/DataSeed/SeedData/delivery.json");
                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);
                    foreach (var item in deliveryMethods)
                    {
                        context.DeliveryMethods.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ApplicationDbContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}
