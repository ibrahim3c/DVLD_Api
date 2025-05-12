using DVLD.Core.DTOs;
using DVLD.Core.Helpers;

namespace DVLD.Core.Services.Interfaces
{
    public interface ILicenseService
    {
        Task<Result<int>> IssueLicenseFirstTimeAsync(AddLicenseDTO addLicenseDTO);
        Task<Result<IEnumerable<LicenseClassDTO>>> GetAllLicenseCLassesAsync();
        Task<Result<IEnumerable<GetLicenseDTO>>> GetLicensesByApplicantIdAsync(int applicantId);
        Task<Result<IEnumerable<GetLicenseDTO>>> GetLicensesByDriverIdAsync(int driverId);
        Task<Result<IEnumerable<GetLicenseDTO>>> GetLicenseByLicenseIdAsync(int licenseId);
        Task<Result<IEnumerable<GetLicenseDTO>>> GetLicensesByNationalNoAsync(string nationalNo);
        Task<Result<IEnumerable<GetLicenseDTO>>> GetAllLicensesAsync();

        Task<Result<int>> IssueInternationalLicenseAsync(AddInternationalLicenseDTO addLicenseDTO);
        Task<Result<GetInternationalLicenseDTO>> GetInternationalLicensesByApplicantIdAsync(int applicantId);
        Task<Result<GetInternationalLicenseDTO>> GetInternationalLicensesByDriverIdAsync(int driverId);
        Task<Result<GetInternationalLicenseDTO>>GetInternationalLicenseByLicenseIdAsync(int licenseId);
        Task<Result<GetInternationalLicenseDTO>> GetInternationalLicensesByNationalNoAsync(string nationalNo);

        Task<Result<int>> RenewLicenseAsync(RenewLicenseApplicationDTO renewLicenseApplicationDTO);
        Task<Result<int>> ReplaceForDamagedLicenseAsync(RenewLicenseApplicationDTO renewLicenseApplicationDTO);
        Task<Result<int>> ReplaceForLostLicenseAsync(RenewLicenseApplicationDTO renewLicenseApplicationDTO);
        Task<Result<int>> DetainLicenseAsync(DetainedLicenseDTO detainedLicenseDTO);
        Task<Result<IEnumerable<GetDetainedLicenseDTO>>> GetAllDetainedLicensesAsync();
        Task<Result<IEnumerable<GetDetainedLicenseDTO>>> GetAllDetainedLicensesByNationalNoAsync(string nationalNo);
        Task<Result<IEnumerable<GetDetainedLicenseDTO>>> GetAllDetainedLicensesByApplicantIdAsync(int applicantId);
        Task<Result<int>> ReleaseLicenseAsync(int applicantionId);





    }
}
