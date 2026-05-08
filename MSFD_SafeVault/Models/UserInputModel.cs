using System.ComponentModel.DataAnnotations;

namespace MSFD_SafeVault.Models
{
    public class UserInputModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
