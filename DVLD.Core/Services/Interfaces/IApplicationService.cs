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
        Task<Result> DeleteApplicationAsync(int id);
        Task<Result> UpdateApplicationAsync(int id, UpdateApplicationDTO updateApplicationDTO);
        Task<Result<IEnumerable<ApplicationDTO>>>GetAllApplicationWithStatusAsync(string status);
        Task<Result<IEnumerable<ApplicationDTO>>> GetAllApplicantApplicationsByNationalNoAsync(string nationalNo);
        Task<Result<IEnumerable<ApplicationDTO>>> GetAllApplicantApplicationsByIdAsync(int applicantId);
        // NewLocalDrivingLicense
        Task<Result<int>> ApplyForNewLocalDrivingLincense(int applicantId, int LicenseClassId);
        Task<Result> ApproveTheApplicationAsync(int appId);
        Task<Result> RejectTheApplicationAsync(int appId);
        Task<Result<IEnumerable<ApplicationDTO>>> GetAllLocalApplicationsLicense();

        // Tests
        Task<Result<int>> ScheduleVisionTestAsync(int appId,int applicantId);
        Task<Result<int>> ScheduleWrittenTestAsync(int appId, int applicantId);
        Task<Result<int>> SchedulePracticalTestAsync(int appId, int applicantId);
        Task<Result> EditTestAppointmentAsync(int testAppointmentId, EditTestAppointmentDTO editTestAppointmentDTO);
        Task<Result> CompleteTestAsync(CompleteTestDTO completeTestDTO);

       



    }
}
