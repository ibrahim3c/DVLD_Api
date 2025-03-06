using DVLD.Core.DTOs;
using DVLD.Core.Helpers;
using DVLD.Core.Models;

namespace DVLD.Core.Services.Interfaces
{
    public interface IApplicantService
    {
        Task<Result<Applicant>> GetByIdAsync(int id);
        Task<Result<Applicant>> GetByNationalNoAsync(string nationalNo);
        Task<Result<List<Applicant>>> GetApplicantsAsync(int pageNumber, int pageSize);
        Task<Result<AppUser>> GetApplicantUserbyIdAsync(int id);
        Task<Result<AppUser>> GetApplicantUserbyNationalNoAsync(string nationalNo);
        Task<Result<bool>> DeleteApplicantByIdAsync(int id);
        Task<Result<bool>> DeleteApplicantByNationalNoAsync(string nationalNo);
        Task<Result<bool>> IsNationalNoTakenAsync(string nationalNo);
        Task<Result<bool>> UpdateApplicantAsync(int id, ApplicantDTO updatedApplicant);
        Task<Result<bool>> AddApplicant(ApplicantDTO addedApplicant);

    }
}
