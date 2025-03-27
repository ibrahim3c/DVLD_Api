namespace DVLD.Core.Models
{
    public class Driver
    {
        public int DriverId {  get; set; }
        public int applicantId {  get; set; }
        
        public ICollection<License> Licenses { get; set; }=new List<License>();
        public Applicant? Applicant { get; set; }
    }
}
