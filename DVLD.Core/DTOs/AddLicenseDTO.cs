namespace DVLD.Core.DTOs
{
    public class AddLicenseDTO
    {
        public int LicenseClassId { get; set; }
        public string NationalNo { get; set; }
        public int AppId { get; set; }
        public string? Notes { get; set; }
        public decimal PaidFees { get; set; }
    }
}
