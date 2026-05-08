using System.Text.RegularExpressions;

namespace MSFD_SafeVault.Services
{
    public static class InputSanitizer
    {
        public static string Sanitize(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Remove full script blocks first so embedded script content is not retained.
            input = Regex.Replace(input, @"<script\b[^<]*(?:(?!</script>)<[^<]*)*</script>", string.Empty, RegexOptions.IgnoreCase);

            // Remove remaining HTML tags.
            input = Regex.Replace(input, "<.*?>", string.Empty);

            // Remove SQL control characters and comment tokens.
            input = Regex.Replace(input, @"(--|['"";])", string.Empty);

            // Remove javascript: URI schemes to prevent XSS via href/src attributes.
            input = Regex.Replace(input, @"javascript\s*:", string.Empty, RegexOptions.IgnoreCase);

            // Trim whitespace from the beginning and end of the input
            return input.Trim();
        }
    }
}
