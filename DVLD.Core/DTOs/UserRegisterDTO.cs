using DVLD.Core.Helpers;

namespace DVLD.Core.DTOs;
public class UserRegisterDTO
{
    public string NationalNo { get; set; } = default!;
    public string Fname { get; set; } = default!;
    public string Sname { get; set; } = default!;
    public string Tname { get; set; } = default!;
    public string Lname { get; set; } = default!;
    public int CountryId { get; set; } = default!;

    public Gender Gender { get; set; } = default!;
    public DateTime BirthDate { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
}

