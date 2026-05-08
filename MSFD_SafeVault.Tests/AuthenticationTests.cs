using NUnit.Framework;
using MSFD_SafeVault.Services;
using MSFD_SafeVault.Data;

namespace MSFD_SafeVault.Tests
{
    public class AuthenticationTests
    {
        private AuthService _authService = null!;

        [SetUp]
        public void SetUp()
        {
            _authService = new AuthService();
        }

        // Verify that the seeded admin account authenticates successfully
        [Test]
        public void ValidateCredentials_AdminUser_ReturnsTrue()
        {
            bool result = _authService.ValidateCredentials("admin", "Admin@123", out _);
            Assert.That(result);
        }

        // Verify that the seeded regular user authenticates successfully
        [Test]
        public void ValidateCredentials_RegularUser_ReturnsTrue()
        {
            bool result = _authService.ValidateCredentials("user1", "User@123", out _);
            Assert.That(result);
        }

        // Wrong password must be rejected
        [Test]
        public void ValidateCredentials_WrongPassword_ReturnsFalse()
        {
            bool result = _authService.ValidateCredentials("admin", "wrongpassword", out _);
            Assert.That(!result);
        }

        // Non-existent user must be rejected
        [Test]
        public void ValidateCredentials_NonExistentUser_ReturnsFalse()
        {
            bool result = _authService.ValidateCredentials("nobody", "Password1!", out _);
            Assert.That(!result);
        }

        // Admin user record must carry the Admin role
        [Test]
        public void ValidateCredentials_AdminUser_HasAdminRole()
        {
            _authService.ValidateCredentials("admin", "Admin@123", out var user);
            Assert.That(user?.Role == "Admin");
        }

        // Regular user record must carry the User role (not Admin)
        [Test]
        public void ValidateCredentials_RegularUser_HasUserRole()
        {
            _authService.ValidateCredentials("user1", "User@123", out var user);
            Assert.That(user?.Role == "User");
        }

        // Empty credentials must be rejected without throwing
        [Test]
        public void ValidateCredentials_EmptyCredentials_ReturnsFalse()
        {
            bool result = _authService.ValidateCredentials(string.Empty, string.Empty, out _);
            Assert.That(!result);
        }

        // Username look-up must be case-insensitive
        [Test]
        public void ValidateCredentials_CaseInsensitiveUsername_ReturnsTrue()
        {
            bool result = _authService.ValidateCredentials("ADMIN", "Admin@123", out _);
            Assert.That(result);
        }
    }
}
