using CustomLinqSqlProvider.Models;
using CustomLinqSqlProvider.Models.Entities;
using CustomLinqSqlProvider.Services;

public class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Running test with SQL Server...");
        RunSqlServerTest();

        Console.WriteLine("\nRunning test with In-Memory Collection...");
        RunInMemoryTest();

        Console.ReadLine();
    }

    public static void RunSqlServerTest()
    {
        var connectionString = "Server=DESKTOP-B0NLOF1;Database=Products;Integrated Security=True;TrustServerCertificate=True";
        var sqlService = new SqlQueryService(connectionString);
        var productSet = new SqlQueryable<Products>(sqlService);

        var query = productSet.Where(p => p.Name == "Laptop");

        foreach (var product in query)
        {
            Console.WriteLine($"Product ID: {product.Id}, Name: {product.Name}, Description: {product.Description}, UnitPrice={product.UnitPrice}");
        }
    }

    // In-Memory Collection Testing Method
    public static void RunInMemoryTest()
    {
        var products = ProductTestData.GetSampleProducts();

        var productSet = products.AsQueryable();

        var query = productSet.Where(p => p.Name == "Laptop");

        var translator = new ExpressionToSqlTranslator("Products");
        string sqlQuery = translator.Translate(query.Expression);
        Console.WriteLine($"Genereted Querry:{sqlQuery}");

        foreach (var product in query)
        {
            Console.WriteLine($" Name: {product.Name}, Description: {product.Description}, UnitPrice={product.UnitPrice}");
        }
    }

    public class ProductTestData
    {
        public static List<Products> GetSampleProducts()
        {
            return new List<Products>
             {
               new Products { Name = "Laptop", UnitPrice = 999.99m, Description = "High-end laptop with 16GB RAM" },
               new Products { Name = "Smartphone", UnitPrice = 499.99m, Description = "Flagship smartphone with great camera" },
               new Products { Name = "Tablet", UnitPrice = 299.99m, Description = "10-inch tablet with stylus" },
               new Products { Name = "Headphones", UnitPrice = 89.99m, Description = "Noise-cancelling wireless headphones" },
               new Products { Name = "Monitor", UnitPrice = 199.99m, Description = "27-inch 4K monitor" },
               new Products { Name = "Keyboard", UnitPrice = 49.99m, Description = "Mechanical keyboard with RGB lights" },
               new Products { Name = "Mouse", UnitPrice = 29.99m, Description = "Wireless mouse with ergonomic design" },
               new Products { Name = "Smartwatch", UnitPrice = 149.99m, Description = "Fitness tracker smartwatch" },
               new Products { Name = "Charger", UnitPrice = 19.99m, Description = "Fast charger for smartphones" },
                new Products { Name = "External Hard Drive", UnitPrice = 79.99m, Description = "1TB external storage device" }
            };
        }
    }
}