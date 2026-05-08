using NUnit.Framework;
using MSFD_SafeVault.Services;

namespace MSFD_SafeVault.Tests
{
    public class SqlInjectionTests
    {
        // Tests for SQL injection patterns - single quotes
        [Test]
        public void Sanitize_RemovesSingleQuotes()
        {
            string malicious = "Robert'); DROP TABLE Users;--";
            string clean = InputSanitizer.Sanitize(malicious);

            Assert.That(!clean.Contains("'"));
        }

        // Tests for SQL injection patterns - double quotes
        [Test]
        public void Sanitize_RemovesDoubleQuotes()
        {
            string malicious = "\" OR \"1\"=\"1";
            string clean = InputSanitizer.Sanitize(malicious);

            Assert.That(!clean.Contains("\""));
        }

        // Tests for SQL injection patterns - SQL comment dashes
        [Test]
        public void Sanitize_RemovesSqlCommentDashes()
        {
            string malicious = "test--comment";
            string clean = InputSanitizer.Sanitize(malicious);

            Assert.That(!clean.Contains("--"));
        }

        // Tests for SQL injection patterns - semicolons
        [Test]
        public void Sanitize_RemovesSemicolons()
        {
            string malicious = "test; DROP TABLE Users;";
            string clean = InputSanitizer.Sanitize(malicious);

            Assert.That(!clean.Contains(";"));
        }

        [Test]
        public void Sanitize_RemovesMultipleSqlInjectionPatterns()
        {
            string malicious = "';--\"";
            string clean = InputSanitizer.Sanitize(malicious);

            Assert.That(!clean.Contains("'"));
            Assert.That(!clean.Contains("--"));
            Assert.That(!clean.Contains("\""));
            Assert.That(!clean.Contains(";"));
        }
    }
}
