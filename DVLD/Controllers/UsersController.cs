using Core.DTOS;
using DVLD.Core.Constants;
using DVLD.Core.DTOs;
using DVLD.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.Api.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class UsersController:ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IApplicantService applicantService;
        public UsersController(IUserService userService, IApplicantService applicantService)
        {
            _userService = userService;
            this.applicantService = applicantService;
        }

        [HttpGet("GetAllUsers")]
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var result = await _userService.GetAllUsersAsync();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("GetUser/{userID}")]
        public async Task<IActionResult> GetUserByIdAsync(string userID)
        {
            var result = await _userService.GetUserByIdAsync(userID);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("GetUserByEmail/{email}")]
        public async Task<IActionResult> GetUserByEmailAsync(string email)
        {
            var result = await _userService.GetUserByEmailAsync(email);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = Roles.AdminRole)]
        [HttpGet("GetRolesOfUser/{userId}")]
        public async Task<IActionResult> GetRolesOfUserAsync(string userId)
        {
            var result = await _userService.GetRolesOfUserAsync(userId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        //[Authorize(Roles = Roles.AdminRole)]
        [HttpGet("GetRolesNameOfUser/{userId}")]
        public async Task<IActionResult> GetRolesNameOfUserAsync(string userId)
        {
            var result = await _userService.GetRolesNameOfUserAsync(userId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpPost("LockUnLock/{id}")]
        public async Task<IActionResult> LockUnLock(string id)
        {
            var result = await _userService.LockUnLock(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = Roles.AdminRole)]
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUserAsync([FromBody] CreatedUserDTO createdUserDTO)
        {
            var result = await _userService.AddUserAsync(createdUserDTO);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize]
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDTO userDTO)
        {
            var result = await _userService.UpdateUserAsync(userDTO);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = Roles.AdminRole)]
        [HttpDelete("DeleteUser/{userID}")]
        public async Task<IActionResult> DeleteUserAsync(string userID)
        {
            var result = await _userService.DeleteUserAsync(userID);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = Roles.AdminRole)]
        [HttpDelete("DeleteUserByEmail/{email}")]
        public async Task<IActionResult> DeleteUserByEmailAsync(string email)
        {
            var result = await _userService.DeleteUserByEmailAsync(email);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = Roles.AdminRole)]
        [HttpGet("GetRolesForManaging/{userId}")]
        public async Task<IActionResult> GetRolesForManagingAsync(string userId)
        {
            var result = await _userService.GetRolesForManagingAsync(userId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = Roles.AdminRole)]
        [HttpPost("ManageUserRoles")]
        public async Task<IActionResult> ManageUserRolesAsync([FromBody] ManageRolesDTO manageRolesDTO)
        {
            var result = await _userService.ManageUserRolesAsync(manageRolesDTO);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        //[HttpGet("GetApplicantId/{userId}")]
        //public async Task<IActionResult> GetApplicantIdAsync(string userId)
        //{
        //    var result = await applicantService.GetApplicantIdByUserId(userId);
        //    if (result.IsSuccess)
        //        return Ok(result);
        //    return BadRequest(result);
        //}
    }
}
