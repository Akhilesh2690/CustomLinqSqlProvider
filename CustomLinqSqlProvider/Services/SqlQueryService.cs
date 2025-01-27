using CustomLinqSqlProvider.Models;
using Microsoft.Data.SqlClient;

namespace CustomLinqSqlProvider.Services
{
    public class SqlQueryService
    {
        private readonly string _connectionString;

        public SqlQueryService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IQueryable<Products> ExecuteSqlQuery(string query)
        {
            Console.WriteLine($"Executing SQL query: {query}");
            List<Products> products = new List<Products>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var product = new Products
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice"))
                            };

                            products.Add(product);
                        }
                    }
                }
            }

            return products.AsQueryable();
        }
    }
}