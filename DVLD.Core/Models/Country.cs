using System.ComponentModel.DataAnnotations;

namespace DVLD.Core.Models
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CountryName { get; set; } = default!;

        // Navigation Property
        public ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();
    }
}
