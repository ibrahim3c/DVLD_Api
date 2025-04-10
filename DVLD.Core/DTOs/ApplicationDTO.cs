﻿using DVLD.Core.Helpers;

namespace DVLD.Core.DTOs
{
    public  class ApplicationDTO
    {
        public int AppId {  get; set; }
        public string ApplicationType { get; set; } 
        public DateTime AppDate { get; set; } = DateTime.UtcNow;
        public string AppStatus { get; set; } = AppStatuses.Pending; 
        public decimal AppFee { get; set; }
        public string ApplicantName { get; set; }
        public string NationalNumber { get; set; }
        public string? LicenseClass { get; set; }
    }
}
