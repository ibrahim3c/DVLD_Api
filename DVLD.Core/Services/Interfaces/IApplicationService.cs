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

        // NewLocalDrivingLicense
        Task<Result<int>> ApplyForNewLocalDrivingLincense(int applicantId, int LicenseClassId);
        Task<Result> ApproveTheApplicationAsync(int appId);
        Task<Result> RejectTheApplicationAsync(int appId);

    }
}
