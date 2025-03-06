using DVLD.Core.DTOs;

namespace DVLD.Core.Services.Interfaces
{
    public interface IRolesService
    {
        Task<ResultDTO<IEnumerable<GetRoleDTO>>> GetAllRolesAsync();
        Task<ResultDTO<GetRoleDTO>> GetRoleByIdAsync(string roleId);
        Task<ResultDTO<GetRoleDTO>> AddRoleAsync(GetRoleDTO roleDTO);
        Task<ResultDTO<GetRoleDTO>> UpdateRoleAsync(GetRoleDTO roleDTO);
        Task<ResultDTO<GetRoleDTO>> DeleteRoleAsync(string roleId);
    }

}
