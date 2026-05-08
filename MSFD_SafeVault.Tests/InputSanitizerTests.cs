using NUnit.Framework;
using MSFD_SafeVault.Services;

namespace MSFD_SafeVault.Tests
{
    public class InputSanitizerTests
    {
        // Tests for SQL injection patterns - single quotes
        [Test]
        public void Sanitize_RemovesScriptTags()
        {
            string malicious = "<script>alert('xss')</script>";
            string clean = InputSanitizer.Sanitize(malicious);

            Assert.That(!clean.Contains("<script>"));
            Assert.That(!clean.Contains("</script>"));
            Assert.That(!clean.Contains("alert"));
        }

        // Tests for HTML tag removal and whitespace trimming
        [Test]
        public void Sanitize_RemovesHtmlTags()
        {
            string malicious = "<b>Hello</b>";
            string clean = InputSanitizer.Sanitize(malicious);

            Assert.That(clean == "Hello");
        }

        // Tests for SQL injection patterns - double quotes
        [Test]
        public void Sanitize_TrimsWhitespace()
        {
            string input = "   test   ";
            string clean = InputSanitizer.Sanitize(input);

            Assert.That(clean == "test");
        }

        // Tests for SQL injection patterns - SQL comment dashes
        [Test]
        public void Sanitize_EmptyOrWhitespace_ReturnsEmptyString()
        {
            string input = "   ";
            string clean = InputSanitizer.Sanitize(input);

            Assert.That(clean == string.Empty);
        }
    }
}
