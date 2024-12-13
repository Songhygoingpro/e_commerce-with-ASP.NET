using System.Data.SqlClient;

namespace E_commerce.Models
{
    public class ProductDataAccess
    {
        private readonly string _connectionString;

        public ProductDataAccess(IConfiguration configuration)
        {
            // Retrieve connection string from configuration
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Adds a new product to the database.
        /// </summary>
        public void AddProduct(Product product)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO Products (Title, Price, Quantity, Image) VALUES (@Title, @Price, @Quantity, @Image)";
                var command = new SqlCommand(query, connection);

                // Add parameters
                command.Parameters.AddWithValue("@Title", product.Title);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                command.Parameters.AddWithValue("@Image", product.Image);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Products";
                var command = new SqlCommand(query, connection);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var product = new Product
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            Title = reader["Title"].ToString(),
                            Price = Convert.ToDecimal(reader["Price"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Image = reader["Image"].ToString()
                        };

                        products.Add(product);
                    }
                }
            }

            return products;
        }

        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        public void UpdateProduct(Product product)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE Products SET Title = @Title, Price = @Price, Quantity = @Quantity, Image = @Image WHERE ProductId = @ProductId";
                var command = new SqlCommand(query, connection);

                // Add parameters
                command.Parameters.AddWithValue("@ProductId", product.ProductId);
                command.Parameters.AddWithValue("@Title", product.Title);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                command.Parameters.AddWithValue("@Image", product.Image);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Deletes a product from the database.
        /// </summary>
        public void DeleteProduct(int productId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Products WHERE ProductId = @ProductId";
                var command = new SqlCommand(query, connection);

                // Add parameters
                command.Parameters.AddWithValue("@ProductId", productId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
