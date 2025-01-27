using CustomLinqSqlProvider.Models;
using System.Linq.Expressions;

namespace CustomLinqSqlProvider.UnitTests
{
    public class ExpressionToSqlTranslatorTests
    {
        [Fact]
        public void Translate_WhereClause_GeneratesCorrectSql()
        {
            var productSet = ProductTestData.GetSampleProducts();

            Expression<Func<Products, bool>> filter = p => p.Name == "Laptop";

            var query = productSet.AsQueryable().Where(filter);

            var translator = new ExpressionToSqlTranslator("Products");
            string sqlQuery = translator.Translate(query.Expression);

            Assert.Equal("SELECT * FROM [Products] WHERE [Name] = 'Laptop'", sqlQuery);
        }

        [Fact]
        public void Translate_WhereClauseWithGreaterThan_GeneratesCorrectSql()
        {
            var productSet = ProductTestData.GetSampleProducts();

            Expression<Func<Products, bool>> filter = p => p.UnitPrice > 100;
            var query = productSet.AsQueryable().Where(filter);

            var translator = new ExpressionToSqlTranslator("Products");
            string sqlQuery = translator.Translate(query.Expression);

            Assert.Equal("SELECT * FROM [Products] WHERE [UnitPrice] > 100", sqlQuery);
        }

        [Fact]
        public void Translate_WhereClauseWithMultipleConditions_GeneratesCorrectSql()
        {
            var productSet = ProductTestData.GetSampleProducts();

            Expression<Func<Products, bool>> filter = p => p.Name == "Laptop" && p.UnitPrice > 100;
            var query = productSet.AsQueryable().Where(filter);

            var translator = new ExpressionToSqlTranslator("Products");
            string sqlQuery = translator.Translate(query.Expression);

            Assert.Equal("SELECT * FROM [Products] WHERE [Name] = 'Laptop' AND [UnitPrice] > 100", sqlQuery);
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