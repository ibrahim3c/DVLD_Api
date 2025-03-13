namespace DVLD.Core.Models
{
    public sealed class LicenseClass
    {
        public int Id {  get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MinAge {  get; set; }
        public int ValidityPeriod {  get; set; }
        public decimal Fee {  get; set; }
        public ICollection<Application> Applications { get; set; }=new List<Application>();
    }
}
