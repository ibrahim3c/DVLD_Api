namespace DVLD.Core.Models
{
    public class RenewLicenseApplication:Application
    {
        public int ExpiredLicenseId {  get; set; }
        public License? ExpiredLicense { get; set; }
    }
}
