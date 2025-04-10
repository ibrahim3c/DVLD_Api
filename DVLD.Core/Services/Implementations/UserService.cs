using Core.DTOS;
using DVLD.Core.DTOs;
using DVLD.Core.Helpers;
using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Core.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Core.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IUOW unitOfWork;
        private readonly IFileService fileService;
        private readonly RoleManager<AppRole> roleManager;
        private readonly IRolesService rolesService;

        public UserService(UserManager<AppUser> userManager,
            IUOW unitOfWork
            , IFileService fileService,
            RoleManager<AppRole> roleManager,
            IRolesService rolesService)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.fileService = fileService;
            this.roleManager = roleManager;
            this.rolesService = rolesService;
        }

        // get
        public async Task<ResultDTO<IEnumerable<UserDTO>>> GetAllUsersAsync()
        {
            var users = await userManager.Users.Select(u=> new UserDTO
            {
                UserName=u.UserName,
                Email=u.Email,
                PhoneNumber=u.PhoneNumber,
                IsActive=u.IsActive
            }).ToListAsync();
            if (!users.Any())
                return ResultDTO<IEnumerable<UserDTO>>.Failure(["No Users Found"]);

            return ResultDTO<IEnumerable<UserDTO>>.SuccessFully(Data: users, messages: ["Users Found"]);
        }
        public async Task<ResultDTO<UserDTO>> GetUserByIdAsync(string userID)
        {
            var user = await userManager.FindByIdAsync(userID);
            if (user == null)
                return ResultDTO<UserDTO>.Failure(["No User Found"]);
            var userDTO = new UserDTO
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive
            };

            return ResultDTO<UserDTO>.SuccessFully(Data: userDTO, messages: ["Users Found"]);
        }

        public async Task<ResultDTO<IEnumerable<GetRoleDTO>>> GetRolesOfUserAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return ResultDTO<IEnumerable<GetRoleDTO>>.Failure(["No User Found"]);

            var rolesNames = await userManager.GetRolesAsync(user);
            if (!rolesNames.Any())
                return ResultDTO<IEnumerable<GetRoleDTO>>.Failure(["No Roles Found"]);

            // if u want to get roles as object
            var roles = new List<GetRoleDTO>();
            foreach (var roleName in rolesNames)
            {
                var role = await roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    roles.Add(new GetRoleDTO
                    {
                        RoleId = role.Id,
                        RoleName = roleName,
                    });
                }
            }

            return ResultDTO<IEnumerable<GetRoleDTO>>.SuccessFully(Data: roles, messages: ["Roles Found"]);
        }
        public async Task<ResultDTO<UserDTO>> GetUserByEmailAsync(string email)
        {

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return ResultDTO<UserDTO>.Failure(["No User Found"]);
            var userDTO = new UserDTO
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive
            };

            return ResultDTO<UserDTO>.SuccessFully(Data: userDTO, messages: ["Users Found"]);
        }

        // lock
        public async Task<ResultDTO<string>> LockUnLock(string id)
        {
            // Find the user by their Id
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return ResultDTO<string>.Failure(["No User Found"]);

            var message = "";
            // If user is not locked out, lock them for 1 year
            if (user.LockoutEnd == null || user.LockoutEnd < DateTime.UtcNow)
            {
                user.LockoutEnd = DateTime.UtcNow.AddYears(1);  // Lock the user
                message = "User Locked Ssuccessfully";
            }
            else
            {
                // Unlock the user by setting LockoutEnd to null
                user.LockoutEnd = null;
                message = "User UnLocked Ssuccessfully";

            }

            // Update the user in the database
            await userManager.UpdateAsync(user);

            return ResultDTO<string>.SuccessFully([message], null);
        }

        // add
        public async Task<Result<string>> AddUserAsync(CreatedUserDTO createduserDTO)
        {
            if (await userManager.FindByEmailAsync(createduserDTO.Email) is not null)
                return Result<string>.Failure(["Email is already Registered!"]);

            var user = new AppUser
            {
                UserName = createduserDTO.FirstName+" "+createduserDTO.LastName,  // Use email as username
                Email = createduserDTO.Email,
                PhoneNumber = createduserDTO.PhoneNumber,
                IsActive = createduserDTO.IsActive, // Ensure this property exists in AppUser
            };

            var result = await userManager.CreateAsync(user, createduserDTO.Password);
            if (!result.Succeeded)
                return Result<string>.Failure(result.Errors.Select(e => e.Description).ToList());

            return Result<string>.Success(user.Id);
        }
        // update 
        public async Task<ResultDTO<UserDTO>> UpdateUserAsync(UpdateUserDTO userDTO)
        {
            var user = await userManager.FindByIdAsync(userDTO.UserId);
            if (user == null)
                return ResultDTO<UserDTO>.Failure(["No User Found"]);

            if (user.UserName != userDTO.UserName)
                user.UserName = userDTO.UserName;


            if (user.PhoneNumber != userDTO.PhoneNumber)
                user.PhoneNumber = userDTO.PhoneNumber;

            await userManager.UpdateAsync(user);

            return ResultDTO<UserDTO>.SuccessFully(["User Updated Successfully"], (await GetUserByEmailAsync(user.Email)).Data);


        }

        //TODO:ChangeEmail and ChangePassword

        // delete User
        public async Task<Result> DeleteUserAsync(string userID)
        {
            var user = await userManager.FindByIdAsync(userID);

            if (user == null)
                return Result.Failure(new List<string> { "No User Found" });

            IdentityResult result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return Result.Failure(new List<string> { "Failed To Delete This User" });
            }

            return Result.Success();
        }

        public async Task<Result> DeleteUserByEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
                return Result.Failure(["No User Found"]);

            IdentityResult result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return Result.Failure(new List<string> { "Failed To Delete This User" });

            }
            return Result.Success();

        }


        // User Roles
        public async Task<ResultDTO<IEnumerable<string>>> GetRolesNameOfUserAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return ResultDTO<IEnumerable<string>>.Failure(["No User Found"]);

            var roles = await userManager.GetRolesAsync(user);
            if (!roles.Any())
                return ResultDTO<IEnumerable<string>>.Failure(["No Roles Found"]);

            return ResultDTO<IEnumerable<string>>.SuccessFully(Data: roles, messages: ["Roles Found"]);
        }
        public async Task<ResultDTO<ManageRolesDTO>> GetRolesForManagingAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return ResultDTO<ManageRolesDTO>.Failure(["No User Found"]);

            var roles = await rolesService.GetAllRolesAsync();

            if (!roles.Success)
                return ResultDTO<ManageRolesDTO>.Failure(roles.Messages);

            var manageRoles = roles.Data.Select(r => new RolesDTO
            {
                RoleName = r.RoleName,
                IsSelected = userManager.IsInRoleAsync(user, r.RoleName).Result
            }).ToList();

            var UserRoles = new ManageRolesDTO
            {
                Roles = manageRoles,
                UserId = userId
            };
            return ResultDTO<ManageRolesDTO>.SuccessFully(["Roles Found "], UserRoles);

        }
        public async Task<ResultDTO<ManageRolesDTO>> ManageUserRolesAsync(ManageRolesDTO manageRolesDTO)
        {
            var user = await userManager.FindByIdAsync(manageRolesDTO.UserId);

            if (user == null)
                return ResultDTO<ManageRolesDTO>.Failure(["No User Found"]);

            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in manageRolesDTO.Roles)
            {
                if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected)
                    await userManager.RemoveFromRoleAsync(user, role.RoleName);

                if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected)
                    await userManager.AddToRoleAsync(user, role.RoleName);
            }

            return ResultDTO<ManageRolesDTO>.SuccessFully(["Roles of User Managed Successfully"],
                (await GetRolesForManagingAsync(manageRolesDTO.UserId)).Data);
        }

    }
}
