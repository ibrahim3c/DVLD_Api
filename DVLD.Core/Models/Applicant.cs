using DVLD.Core.Helpers;

namespace DVLD.Core.Models
{
    public class Applicant
    {
        public int ApplicantId { get; set; }

        public string NationalNo { get; set; } = default!;
        public string Fname { get; set; } = default!;

        public string Sname { get; set; } = default!;

        public string Tname { get; set; } = default!;

        public string Lname { get; set; } = default!;

        public int CountryId { get; set; } = default!;

        public Country Country { get; set; } = default!;

        public Gender Gender { get; set; } = default!;

        public DateTime BirthDate { get; set; } = default!;

        public string? ImagePath { get; set; } // Store path to image

        public string Address { get; set; } = default!;

        // Navigation Properties
        public AppUser User { get; set; } = default!;
        public string UserId {  get; set; } = default!;

        public ICollection<Application>? Applications { get; set; } = new List<Application>();


    }
}
