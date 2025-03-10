using DVLD.Core.Helpers;

namespace DVLD.Core.Models
{
    public sealed class Application
    {
            public int AppID { get; set; }  // Primary Key
            public int AppTypeID { get; set; }  // Foreign Key - Application Type
            public DateTime AppDate { get; set; } = DateTime.UtcNow;
            public string AppStatus { get; set; } = AppStatuses.Pending; // Default status
            public decimal AppFee { get; set; }
            public int ApplicantId { get; set; }  // Foreign Key - Applicant
            public int? LicenseClassId { get; set; } // i make nullable cuz i want the class just if the apptype is newLicence

            // Navigation Properties
            public Applicant Applicant { get; set; } = default!;
            public ApplicationType AppType { get; set; } = default!;
            public LicenseClass? LicenseClass { get; set; } = default!;
        }
    }
