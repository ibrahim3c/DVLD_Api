using DVLD.Core.Helpers;

namespace DVLD.Core.DTOs
{
    public sealed class UpdateApplicationDTO
    {
        public int AppType { get; set; } 
        public DateTime AppDate { get; set; } = DateTime.UtcNow;
        public string AppStatus { get; set; } = AppStatuses.Pending; 
       
    }
}
