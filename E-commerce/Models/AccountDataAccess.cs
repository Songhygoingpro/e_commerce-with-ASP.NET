using System.Data.SqlClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Models
{
    public class AccountDataAccess
    {
        private readonly string _connectionString;

        public AccountDataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Register a new user
        public void RegisterUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", user.Password);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Authenticate user (login)
        public User Authenticate(string username, string password)
        {
            User user = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    user = new User
                    {
                        UserId = (int)reader["UserId"],
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        IsAdmin = (bool)reader["IsAdmin"]
                    };
                }
            }
            return user;
        }

        // Update user details (e.g., password change)
        public void UpdateUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE Users SET Username = @Username, Password = @Password, IsAdmin = @IsAdmin WHERE UserId = @UserId";
                var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", user.UserId);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@IsAdmin", user.IsAdmin);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Delete a user (if needed)
        public void DeleteUser(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Users WHERE UserId = @UserId";
                var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
