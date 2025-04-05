namespace DVLD.Core.DTOs
{
    public class DetainedLicenseDTO
    {
        public decimal FineFees { get; set; }
        public DateTime DetainedDate { get; set; }
        public DateTime? ReleasedDate { get; set; }
        public int LicenseId { get; set; }
        public string Reason { get; set; } // Reason for the detention
        public string? Notes { get; set; } // Additional notes for the detention    }
    }
}