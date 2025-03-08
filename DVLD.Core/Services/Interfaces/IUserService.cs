using Core.DTOS;
using DVLD.Core.DTOs;
using DVLD.Core.Helpers;

namespace DVLD.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResultDTO<IEnumerable<UserDTO>>> GetAllUsersAsync();
        Task<ResultDTO<UserDTO>> GetUserByIdAsync(string userID);
        Task<ResultDTO<UserDTO>> GetUserByEmailAsync(string email);
        Task<ResultDTO<IEnumerable<GetRoleDTO>>> GetRolesOfUserAsync(string userId);
        Task<ResultDTO<IEnumerable<string>>> GetRolesNameOfUserAsync(string userId);
        Task<ResultDTO<string>> LockUnLock(string id);
        Task<Result<string>> AddUserAsync(CreatedUserDTO createduserDTO);
        Task<ResultDTO<UserDTO>> UpdateUserAsync(UpdateUserDTO userDTO);
        Task<Result> DeleteUserAsync(string userID);
        Task<Result> DeleteUserByEmailAsync(string email);
        Task<ResultDTO<ManageRolesDTO>> GetRolesForManagingAsync(string userId);
        Task<ResultDTO<ManageRolesDTO>> ManageUserRolesAsync(ManageRolesDTO manageRolesDTO);

    }
}
