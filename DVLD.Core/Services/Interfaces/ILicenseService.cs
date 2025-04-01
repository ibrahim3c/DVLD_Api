using DVLD.Core.DTOs;
using DVLD.Core.Helpers;

namespace DVLD.Core.Services.Interfaces
{
    public interface ILicenseService
    {
        Task<Result<int>> IssueLicenseFirstTimeAsync(AddLicenseDTO addLicenseDTO);
        Task<Result<IEnumerable<GetLicenseDTO>>> GetLicensesByApplicantIdAsync(int applicantId);
        Task<Result<IEnumerable<GetLicenseDTO>>> GetLicensesByDriverIdAsync(int driverId);
        Task<Result<IEnumerable<GetLicenseDTO>>> GetLicenseByLicenseIdAsync(int licenseId);
        Task<Result<IEnumerable<GetLicenseDTO>>> GetLicensesByNationalNoAsync(string nationalNo);

    }
}
