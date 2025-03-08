using DVLD.Core.Helpers;
using System.ComponentModel.DataAnnotations;

namespace DVLD.Core.DTOs
{    public class CreatedUserDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = default!;

        public bool IsActive { get; set; } = true; // Default to active

        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one number, and one special character.")]
        public string Password { get; set; } = default!;

        [Required]
        [ValidRole(ErrorMessage = "Invalid role. Must be Admin or User.")]
        public string Role { get; set; } = default!;
    }

}
