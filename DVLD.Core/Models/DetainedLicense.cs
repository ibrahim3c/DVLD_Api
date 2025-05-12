namespace DVLD.Core.Models
{
    public class DetainedLicense
    {
        public int DetainedLicenseId {  get; set; }
        public decimal FineFees {  get; set; }
        public DateTime DetainedDate {  get; set; }
        public DateTime? ReleasedDate { get; set; }
        public int LicenseId {  get; set; }
        public bool IsReleased { get; set; } = false;
        public int? ReleaseApplicationId {  get; set; }
        public string Reason { get; set; } // Reason for the detention
        public string? Notes { get; set; } // Additional notes for the detention

        public License? License { get; set; }
        public Application? ReleaseApplication {  get; set; }

    }
}
