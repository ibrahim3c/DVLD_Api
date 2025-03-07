﻿using DVLD.Core.DTOs;
using DVLD.Core.Models;
using DVLD.Core.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Core.Services.Implementations
{
    public class RoleService : IRolesService
    {
        private readonly RoleManager<AppRole> roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        // get
        public async Task<ResultDTO<IEnumerable<GetRoleDTO>>> GetAllRolesAsync()
        {
            var roles = await roleManager.Roles.Select(r => new GetRoleDTO
            {
                RoleId = r.Id,
                RoleName = r.Name
            }).ToListAsync();

            if (!roles.Any())
                return ResultDTO<IEnumerable<GetRoleDTO>>.Failure(["No Roles Found"]);

            return ResultDTO<IEnumerable<GetRoleDTO>>.SuccessFully(Data: roles, messages: ["Roles Found"]);
        }
        public async Task<ResultDTO<GetRoleDTO>> GetRoleByIdAsync(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
                return ResultDTO<GetRoleDTO>.Failure(["No Role Found"]);

            var roleDTO =new GetRoleDTO { RoleId = roleId  , RoleName= role.Name};
            
            return ResultDTO<GetRoleDTO>.SuccessFully(Data: roleDTO, messages: ["Role Found"]);
        }

        // add
        public async Task<ResultDTO<GetRoleDTO>> AddRoleAsync(GetRoleDTO roleDTO)
        {
            if (await roleManager.FindByNameAsync(roleDTO.RoleName) != null)
            {
                return ResultDTO<GetRoleDTO>.Failure(["Role Name  already exists "]);
            }

            var role = new AppRole
            {
                Name = roleDTO.RoleName
            };

            IdentityResult result = await roleManager.CreateAsync(role);

            if (result.Succeeded)
                return ResultDTO<GetRoleDTO>.SuccessFully(["Role Added Successfully"], (await GetRoleByIdAsync(role.Id)).Data);

            return ResultDTO<GetRoleDTO>.Failure(["Faild To Add this Role"]);
        }

        // update
        public async Task<ResultDTO<GetRoleDTO>> UpdateRoleAsync(GetRoleDTO roleDTO)
        {
            var role = await roleManager.FindByIdAsync(roleDTO.RoleId);
            if (role == null)
                return ResultDTO<GetRoleDTO>.Failure(["No Role Found"]);

            if (role.Name != roleDTO.RoleName)
            {
                role.Name = roleDTO.RoleName;
                IdentityResult result = await roleManager.UpdateAsync(role);

                if (!result.Succeeded)
                    return ResultDTO<GetRoleDTO>.Failure(["Faild to update this Role"]);

            }

            return ResultDTO<GetRoleDTO>.SuccessFully(["Role Updated Successfully"], (await GetRoleByIdAsync(role.Id)).Data);



        }

        // delete role
        public async Task<ResultDTO<GetRoleDTO>> DeleteRoleAsync(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
                return ResultDTO<GetRoleDTO>.Failure(["No Role Found"]);

            IdentityResult result = await roleManager.DeleteAsync(role);

            if (!result.Succeeded)
                return ResultDTO<GetRoleDTO>.Failure(["Faild to delete this Role"]);

            return ResultDTO<GetRoleDTO>.SuccessFully(["Role Deleted Successfully"], (await GetRoleByIdAsync(role.Id)).Data);



        }
    }
}
