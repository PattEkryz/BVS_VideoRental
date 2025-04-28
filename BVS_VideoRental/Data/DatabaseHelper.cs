using Microsoft.Data.SqlClient;
using System.Data;

namespace BVS_VideoRental.Data
{
    public static class DatabaseHelper
    {
        private static readonly string connectionString = "Server=DESKTOP-KQ3GN66\\SQLEXPRESS;Database=BVS;Trusted_Connection=True;";

        // Customer Methods
        public static DataTable GetCustomers()
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("SELECT Id, Name, Address, Phone FROM Customers", connection);
            using var adapter = new SqlDataAdapter(command);
            var table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public static void AddCustomer(string name, string address, string phone)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(
                "INSERT INTO Customers (Name, Address, Phone) VALUES (@Name, @Address, @Phone)",
                connection);

            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Address", address);
            command.Parameters.AddWithValue("@Phone", phone);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public static void UpdateCustomer(int id, string name, string address, string phone)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(
                "UPDATE Customers SET Name = @Name, Address = @Address, Phone = @Phone WHERE Id = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Address", address);
            command.Parameters.AddWithValue("@Phone", phone);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public static void DeleteCustomer(int id)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("DELETE FROM Customers WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            command.ExecuteNonQuery();
        }

        // User Authentication Methods
        public static User? AuthenticateUser(string username, string password)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(
                "SELECT Id, Username, Role FROM Users WHERE Username = @Username AND Password = @Password",
                connection);

            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);

            connection.Open();
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Role = reader.GetString(2)
                };
            }

            return null;
        }

        public static void AddUser(string username, string password, string role)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(
                "INSERT INTO Users (Username, Password, Role) VALUES (@Username, @Password, @Role)",
                connection);

            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);
            command.Parameters.AddWithValue("@Role", role);

            connection.Open();
            command.ExecuteNonQuery();
        }

        // Add any other database methods you need here
    }
}