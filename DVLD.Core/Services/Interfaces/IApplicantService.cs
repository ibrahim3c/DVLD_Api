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
        Task<Result<string>> GetApplicantUserIdbyIdAsync(int id);
        Task<Result<string>> GetApplicantUserIdbyNationalNoAsync(string nationalNo);
        Task<Result> DeleteApplicantByIdAsync(int id);
        Task<Result> DeleteApplicantByNationalNoAsync(string nationalNo);
        Task<Result> IsNationalNoTakenAsync(string nationalNo);
        Task<Result> UpdateApplicantAsync(int id, ApplicantDTO updatedApplicant);
        Task<Result<int>> AddApplicant(ApplicantDTO addedApplicant);

    }
}
