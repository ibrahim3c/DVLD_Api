namespace DVLD.Core.Models
{
    public class License
    {
        public int LicenseId {  get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int LicenseClassId {  get; set; }
        public int DriverId {  get; set; }
        public string? IssueReason {  get; set; }
        public bool IsValid => ExpirationDate>=DateTime.Today && !IsDetained && !IsDamaged && !IsLost;
        public int AppId {  get; set; }
        public string? Notes {  get; set; }
        public decimal PaidFees {  get; set; }
        public bool IsDetained { get; set; } = false;
        public bool IsDamaged { get; set; } = false;
        public bool IsLost { get; set; } = false;

        public Driver? Driver { get; set; }
        public Application? Application { get; set; }
        public LicenseClass? LicenseClass { get; set; }
        public ICollection<DetainedLicense>? DetainedLicenses { get; set; }=new List<DetainedLicense>();
    }
}
