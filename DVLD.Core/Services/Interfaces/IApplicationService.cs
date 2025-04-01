using DVLD.Core.DTOs;
using DVLD.Core.Helpers;

namespace DVLD.Core.Services.Interfaces
{
    public interface IApplicationService
    {
        // applicationType
        Task<Result<IEnumerable<TypeDTO>>> GetAllAppTypesAsync();
        Task<Result<TypeDTO>> GetAppTypeByIdAsync(int id);
        Task<Result> UpdateAppTypeAsync(int id, TypeDTO appTypeDTO);
        Task<Result> DeleteAppTypeAsync(int id);
        Task<Result<int>> AddAppTypeAync(TypeDTO appTypeDTO);

        // application
        Task<Result<ApplicationDTO>> GetApplicationByIdAsync(int id);
        Task<Result<IEnumerable<ApplicationDTO>>> GetAllApplicationAsync();
        Task<Result> DeleteApplicationAsync(int id);
        Task<Result> UpdateApplicationAsync(int id, UpdateApplicationDTO updateApplicationDTO);
        Task<Result<IEnumerable<ApplicationDTO>>>GetAllApplicationWithStatusAsync(string status);
        Task<Result<IEnumerable<ApplicationDTO>>> GetAllApplicantApplicationsByNationalNoAsync(string nationalNo);
        Task<Result<IEnumerable<ApplicationDTO>>> GetAllApplicantApplicationsByIdAsync(int applicantId);
        Task<Result> ApproveTheApplicationAsync(int appId);
        Task<Result> RejectTheApplicationAsync(int appId);

        // NewLocalDrivingLicense
        Task<Result<int>> ApplyForNewLocalDrivingLincense(int applicantId, int LicenseClassId);
        Task<Result<LocalAppLicenseDTO>> GetLocalAppLicenseByIdAsync(int id);
        Task<Result<IEnumerable<LocalAppLicenseDTO>>> GetAllLocalAppLicensesAsync();
        Task<Result<IEnumerable<LocalAppLicenseDTO>>> GetAllLocalAppLicensesWithsByNationalNoAsync(string nationalNo);
        Task<Result<IEnumerable<LocalAppLicenseDTO>>> GetAllLocalAppLicensesByApplicantIdAsync(int applicantId);

        // retakeTestApp
        Task<Result<int>> ApplyForRetakeTestApp(int applicantId);







    }
}
