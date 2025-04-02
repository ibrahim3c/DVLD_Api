namespace DVLD.Core.DTOs
{
    public class GetInternationalLicenseDTO
    {
        public int LicenseId { get; set; }
        public int DriverId {  get; set; }
        public string IssueDate { get; set; }
        public string ExpirationDate { get; set; }
        public string DriverName { get; set; }
        public string DriverDateOfBirth { get; set; }
        public string? Notes { get; set; }
        public decimal PaidFees { get; set; }
    }
}
