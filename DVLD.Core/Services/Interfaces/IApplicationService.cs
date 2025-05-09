﻿using DVLD.Core.DTOs;
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
        Task<Result<IEnumerable<ApplicationDTO>>>GetAllApplicationWithStatusAsync(string status);
        Task<Result<IEnumerable<ApplicationDTO>>> GetAllApplicantApplicationsByNationalNoAsync(string nationalNo);
        Task<Result<IEnumerable<ApplicationDTO>>> GetAllApplicantApplicationsByIdAsync(int applicantId);

        Task<Result<ApplicationDTO>> GetApplicationByIdAsync(int id, int appType);
        Task<Result<IEnumerable<ApplicationDTO>>> GetAllApplicationAsync(int appType);
        Task<Result<IEnumerable<ApplicationDTO>>> GetAllApplicationWithStatusAsync(string status, int appType);
        Task<Result<IEnumerable<ApplicationDTO>>> GetAllApplicantApplicationsByNationalNoAsync(string nationalNo, int appType);
        Task<Result<IEnumerable<ApplicationDTO>>> GetAllApplicantApplicationsByIdAsync(int applicantId, int appType);

        Task<Result> DeleteApplicationAsync(int id);
        Task<Result> UpdateApplicationAsync(int id, UpdateApplicationDTO updateApplicationDTO);
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

        //NewInternationalLicenseApplication
        Task<Result<int>> ApplyForNewInternationalLicenseApplicationAsync(int applicantId);

        //Renew License
        Task<Result<int>> ApplyForRenewLicenseApplicationAsync(int licenseId);
        Task<Result<GetRenewLicenseApplicationDTO>> GetRewLicenseAppLicenseByIdAsync(int id,int appTypeId);
        Task<Result<IEnumerable<GetRenewLicenseApplicationDTO>>> GetAllRewLicenseAppLicensesAsync(int appTypeId);
        Task<Result<IEnumerable<GetRenewLicenseApplicationDTO>>> GetAllRewLicenseAppsWithsByNationalNoAsync(string nationalNo, int appTypeId);
        Task<Result<IEnumerable<GetRenewLicenseApplicationDTO>>> GetAllRewLicenseAppsByApplicantIdAsync(int applicantId, int appTypeId);

        // damaged and lost license
        Task<Result<int>> ApplyForReplacementDamagedLicenseApplicationAsync(int licenseId);
        Task<Result<int>> ApplyForReplacementLostLicenseApplicationAsync(int licenseId);

        // release LicenseApp
        Task<Result<int>> ApplyForReleaseLicenseApplicationAsync(int licenseId);

    }
}
