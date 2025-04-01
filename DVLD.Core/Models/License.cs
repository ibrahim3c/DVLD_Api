namespace DVLD.Core.Models
{
    public class License
    {
        public int LicenseId {  get; set; }
        public DateTime IssueDate { get; set; }
        public int LicenseClassId {  get; set; }
        public int DriverId {  get; set; }
        public string? IssueReason {  get; set; }
        public bool IsValid {  get; set; }=false;
        public int AppId {  get; set; }
        public string? Notes {  get; set; }
        public decimal PaidFees {  get; set; }

        public Driver? Driver { get; set; }
        public Application? Application { get; set; }
        public LicenseClass? LicenseClass { get; set; }
    }
}
