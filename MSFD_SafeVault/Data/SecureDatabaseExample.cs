using System.Data;
using Microsoft.Data.SqlClient;

namespace MSFD_SafeVault.Data
{
    public class SecureDatabaseExample
    {
        private readonly string _connectionString;

        public SecureDatabaseExample(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Example method that retrieves a user record by username using parameterized queries to prevent SQL injection
        public UserRecord GetUserByUsername(string username)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(
                "SELECT Username, Email FROM Users WHERE Username = @username",
                connection
            );

            // Parameterized query prevents SQL injection
            command.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar, 50)
            {
                Value = username
            });

            connection.Open();
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new UserRecord
                {
                    Username = reader.GetString(0),
                    Email = reader.GetString(1)
                };
            }

            return null;
        }
    }
}
