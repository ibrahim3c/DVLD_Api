namespace DVLD.Core.DTOs
{
    public class GetDetainedLicenseDTO
    {
        public int DetainedLicenseId { get; set; }
        public int LicenseId { get; set; }
        public string DriverName { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public bool IsReleased { get; set; } = false;
        public string? Notes { get; set; }
        public int ReleaseApplicationId { get; set; }
        public string Reason { get; set; } 
        public decimal FineFees { get; set; }
    }
}
