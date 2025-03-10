namespace DVLD.Core.Models
{
    public sealed class ApplicationType
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal TypeFee {  get; set; }
        public ICollection<Application>? Applications {  get; set; }=new List<Application>();
    }
}
