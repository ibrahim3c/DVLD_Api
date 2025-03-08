using System.ComponentModel.DataAnnotations;

namespace DVLD.Core.DTOs
{
    public class TypeDTO
    {
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        public decimal TypeFee { get; set; }
    }
}
