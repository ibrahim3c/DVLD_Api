using DVLD.Core.DTOs;
using DVLD.Core.Helpers;
using DVLD.Core.Models;

namespace DVLD.Core.Services.Interfaces
{
    public interface IDriverServices
    {
        Task <Result<IEnumerable<GetApplicantDTO>>> GetAllDriversAsync();
        Task<Result<GetApplicantDTO>> GetDriversByIdAsync(int id);
        Task<Result<GetApplicantDTO>> GetDriverByApplicantIdAsync(int applicantId);
        Task<Result<GetApplicantDTO>> GetDriverByApplicantNationalNoAsync(string nationalNo);
        Task<Result<bool>> IsApplicantDriver(int applicantId);
        Task<Result<int>> AddDriverAsync(int applicantId);

    }
}
