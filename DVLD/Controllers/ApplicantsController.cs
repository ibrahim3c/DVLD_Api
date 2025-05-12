using DVLD.Core.Constants;
using DVLD.Core.DTOs;
using DVLD.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicantsController:ControllerBase
    {
        private readonly IApplicantService _applicantService;

        public ApplicantsController(IApplicantService applicantService)
        {
            _applicantService = applicantService;
        }

        [HttpGet("GetAllApplicants")]
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> GetAllApplicants()
        {
            var result = await _applicantService.GetAllApplicantsAsync();
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        [Authorize(Roles =Roles.AdminRole)]
        public async Task<IActionResult> AddApplicant([FromForm] ApplicantDTO dto)
        {
            var result = await _applicantService.AddApplicant(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }


        [HttpDelete("{id:int}")]
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> DeleteById(int id)
        {
            var result = await _applicantService.DeleteApplicantByIdAsync(id);
            return result.IsSuccess ? Ok() : NotFound(result);
        }

        [HttpDelete("by-nationalno/{nationalNo}")]
        [Authorize(Roles = Roles.AdminRole)]

        public async Task<IActionResult> DeleteByNationalNo(string nationalNo)
        {
            var result = await _applicantService.DeleteApplicantByNationalNoAsync(nationalNo);
            return result.IsSuccess ? Ok() : NotFound(result);
        }

        [HttpGet("GetPaged")]
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _applicantService.GetApplicantsAsync(pageNumber, pageSize);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("GetApplicantByApplicantId/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _applicantService.GetByIdAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("GetApplicantByNationalNo/{nationalNo}")]
        public async Task<IActionResult> GetByNationalNo(string nationalNo)
        {
            var result = await _applicantService.GetByNationalNoAsync(nationalNo);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("GetApplicantByUserId/{userId}")]
        public async Task<IActionResult> GetApplicantByUserId(string userId)
        {
            var result = await _applicantService.GetByUserIdAsync(userId);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id:int}/fullname")]
        public async Task<IActionResult> GetFullName(int id)
        {
            var result = await _applicantService.GetFullNameAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("GetUserIdByApplicantId/{id:int}")]
        public async Task<IActionResult> GetUserIdByApplicantId(int id)
        {
            var result = await _applicantService.GetApplicantUserIdbyIdAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound();
        }

        [HttpGet("GetUserIdByNationalNo/{nationalNo}")]
        public async Task<IActionResult> GetUserIdByNationalNo(string nationalNo)
        {
            var result = await _applicantService.GetApplicantUserIdbyNationalNoAsync(nationalNo);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromForm] ApplicantDTO dto)
        {
            var result = await _applicantService.UpdateApplicantAsync(id, dto);
            return result.IsSuccess ? Ok() : BadRequest(result);
        }

        [HttpGet("IsNationalNoExist/{nationalNo}")]
        public async Task<IActionResult> IsNationalNoTaken(string nationalNo)
        {
            var result = await _applicantService.IsNationalNoTakenAsync(nationalNo);
            return result.IsSuccess ? Ok(false) : Ok(true); // Returns true if taken
        }

        [HttpGet("{id:int}/details")]
        public async Task<IActionResult> GetDetailsById(int id)
        {
            var result = await _applicantService.GetDetailsByIdAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("details/by-nationalno/{nationalNo}")]
        public async Task<IActionResult> GetDetailsByNationalNo(string nationalNo)
        {
            var result = await _applicantService.GetDetailsByNationalNoAsync(nationalNo);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("GetApplicantIdByUserId/{userId}")]
        public async Task<IActionResult> GetApplicantIdByUserId(string userId)
        {
            var result = await _applicantService.GetApplicantIdByUserId(userId);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("GetUserProfile/{applicantId}")]
        public async Task<IActionResult> GetUserProfile(int applicantId )
        {
            var result = await _applicantService.GetUserProfile(applicantId);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
        [HttpPut("UpdateUserProfile/{id:int}")]
        public async Task<IActionResult> UpdateUserProfile(int id, UserProfileDTO dto)
        {
            var result = await _applicantService.UpdateUserProfile(id, dto,Request);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
