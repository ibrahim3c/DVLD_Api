using DVLD.Core.Helpers;
using System.ComponentModel.DataAnnotations;

namespace DVLD.Core.DTOs;

public class UserRegisterDTO
{
    [Required]
    [RegularExpression(@"^\d{10,15}$", ErrorMessage = "National number must be between 10 and 15 digits.")]
    public string NationalNo { get; set; } = default!;

    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
    public string Fname { get; set; } = default!;

    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Second name must be between 2 and 50 characters.")]
    public string Sname { get; set; } = default!;

    [StringLength(50, ErrorMessage = "Third name cannot exceed 50 characters.")]
    public string Tname { get; set; } = default!;

    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
    public string Lname { get; set; } = default!;

    [Required(ErrorMessage = "Country is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid Country ID.")]
    public int CountryId { get; set; } = default!;

    [Required(ErrorMessage = "Gender is required.")]
    public Gender Gender { get; set; } = default!;

    [Required(ErrorMessage = "Birthdate is required.")]
    [DataType(DataType.Date)]
    [BirthDateValidation(ErrorMessage = "You must be at least 18 years old.")]
    public DateTime BirthDate { get; set; } = default!;

    [Required]
    [StringLength(255, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 255 characters.")]
    public string Address { get; set; } = default!;

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = default!;

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = default!;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = default!;
}
