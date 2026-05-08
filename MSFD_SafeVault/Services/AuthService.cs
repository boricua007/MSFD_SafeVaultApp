using MSFD_SafeVault.Data;

namespace MSFD_SafeVault.Services
{
    // Validates user credentials against the in-memory store using BCrypt password hashing
    public class AuthService
    {
        // Authenticates a user by comparing the supplied plaintext password against
        // the stored BCrypt hash. BCrypt's timing-safe compare prevents timing attacks.

        public bool ValidateCredentials(string username, string password, out UserRecord? user)
        {
            // Guard: never call BCrypt.Verify with null/empty input
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                user = null;
                return false;
            }

            user = InMemoryDatabase.Users.FirstOrDefault(u =>
                string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));

            if (user == null) return false;

            // BCrypt.Verify performs a constant-time comparison to prevent timing attacks
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }
    }
}
