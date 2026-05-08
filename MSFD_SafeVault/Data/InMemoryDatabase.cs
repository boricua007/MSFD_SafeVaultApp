using System.Collections.Generic;

namespace MSFD_SafeVault.Data
{
    // Simple in-memory database for demonstration purposes
    public static class InMemoryDatabase
    {
        public static List<UserRecord> Users { get; } = new List<UserRecord>();

        static InMemoryDatabase()
        {
            // Seed users with BCrypt-hashed passwords (work factor 12)
            Users.Add(new UserRecord
            {
                Username = "admin",
                Email = "admin@safevault.com",
                Role = "Admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123", workFactor: 12)
            });

            Users.Add(new UserRecord
            {
                Username = "user1",
                Email = "user1@safevault.com",
                Role = "User",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123", workFactor: 12)
            });
        }
    }

    public class UserRecord
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
    }
}
