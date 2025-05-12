using DVLD.Core.Helpers;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DVLD.Core.DTOs
{
    public class UserProfileDTO
    {
        public string NationalNo { get; set; } = default!;
        public string Fname { get; set; } = default!;
        public string Sname { get; set; } = default!;
        public string Tname { get; set; } = default!;
        public string Lname { get; set; } = default!;
        public int CountryId { get; set; } = default!;

        public Gender Gender { get; set; } = default!;
        public DateTime BirthDate { get; set; } = default!;
        public IFormFile? Image { get; set; } = default!;
        public string? Address { get; set; } = default!;

        [Phone]
        public string? PhoneNumber { get; set; }
    }
}
