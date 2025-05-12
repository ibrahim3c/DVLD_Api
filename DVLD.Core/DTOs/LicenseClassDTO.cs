namespace DVLD.Core.DTOs
{
    public class LicenseClassDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MinAge { get; set; }
        public int ValidityPeriod { get; set; }
        public decimal Fee { get; set; }
    }
}
