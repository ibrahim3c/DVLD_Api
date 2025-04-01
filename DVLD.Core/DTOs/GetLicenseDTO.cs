namespace DVLD.Core.DTOs
{
    public class GetLicenseDTO
    {
        public int LicenseId { get; set; }
        public string IssueDate { get; set; }
        public string ExpirationDate { get; set; }
        public string LicenseClass { get; set; }
        public string DriverName { get; set; }
        public string DriverDateOfBirth { get; set; }
        public string? IssueReason { get; set; }
        public bool IsValid { get; set; } = false;
        public string? Notes { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsDetained { get; set; }
    }
}
