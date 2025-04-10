﻿using DVLD.Core.DTOs;
using DVLD.Core.Helpers;
using DVLD.Core.Models;

namespace DVLD.Core.Services.Interfaces
{
    public interface IApplicantService
    {
        Task<Result<Applicant>> GetDetailsByIdAsync(int id);
        Task<Result<Applicant>> GetDetailsByNationalNoAsync(string nationalNo);
        Task<Result<List<Applicant>>> GetDetailsApplicantsAsync(int pageNumber, int pageSize);

        Task<Result<GetApplicantDTO>> GetByIdAsync(int id);
        Task<Result<GetApplicantDTO>> GetByNationalNoAsync(string nationalNo);
        Task<Result<List<GetApplicantDTO>>> GetApplicantsAsync(int pageNumber, int pageSize);

        Task<Result<string>> GetApplicantUserIdbyIdAsync(int id);
        Task<Result<string>> GetApplicantUserIdbyNationalNoAsync(string nationalNo);
        Task<Result<string>> GetFullNameAsync(int id);
        Task<Result> DeleteApplicantByIdAsync(int id);
        Task<Result> DeleteApplicantByNationalNoAsync(string nationalNo);
        Task<Result> IsNationalNoTakenAsync(string nationalNo);
        Task<Result> UpdateApplicantAsync(int id, ApplicantDTO updatedApplicant);
        Task<Result<int>> AddApplicant(ApplicantDTO addedApplicant);

    }
}
