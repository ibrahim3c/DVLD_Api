using DVLD.Core.Helpers;

namespace DVLD.Core.DTOs
{
    public class GetApplicantDTO
    {
        public int applicantId { get; set; } = default!;
        public int DriverId { get; set; } = default!;
        public string NationalNo { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public int CountryId { get; set; } = default!;
        public Gender Gender { get; set; } = default!;
        public DateTime BirthDate { get; set; } = default!;
        public string Address { get; set; } = default!;
    }
}
