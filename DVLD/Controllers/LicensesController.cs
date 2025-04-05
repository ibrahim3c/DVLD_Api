using DVLD.Core.DTOs;
using DVLD.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.Api.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class LicensesController:ControllerBase
    {
        private readonly ILicenseService licenseService;

        public LicensesController(ILicenseService licenseService)
        {
            this.licenseService = licenseService;
        }
        [HttpPost("IssueLicenseFirstTime")]
        public async Task<IActionResult> IssueLicenseFirstTime(AddLicenseDTO licenseDTO)
        {
            var result=await licenseService.IssueLicenseFirstTimeAsync(licenseDTO);
            if(result.IsSuccess) 
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetLicensesByApplicantId/{applicantId}")]
        public async Task<IActionResult> GetLicensesByApplicantId(int applicantId)
        {
            var result = await licenseService.GetLicensesByApplicantIdAsync(applicantId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetLicensesByDriverId/{driverId}")]
        public async Task<IActionResult> GetLicensesByDriverId(int driverId)
        {
            var result = await licenseService.GetLicensesByDriverIdAsync(driverId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetLicenseByLicenseId/{licenseId}")]
        public async Task<IActionResult> GetLicenseByLicenseId(int licenseId)
        {
            var result = await licenseService.GetLicenseByLicenseIdAsync(licenseId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetLicensesByNationalNo")]
        public async Task<IActionResult> GetLicensesByNationalNo([FromQuery]string nationalNo)
        {
            var result = await licenseService.GetLicensesByNationalNoAsync(nationalNo);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        // international License
        [HttpPost("IssueInternationalLicesnse")]
        public async Task<IActionResult> IssueInternationalLicesnse(AddInternationalLicenseDTO licenseDTO)
        {
            var result = await licenseService.IssueInternationalLicenseAsync(licenseDTO);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("GetInternationalLicensesByApplicantId/{applicantId}")]
        public async Task<IActionResult> GetInternationalLicensesByApplicantId(int applicantId)
        {
            var result = await licenseService.GetInternationalLicensesByApplicantIdAsync(applicantId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetInternationalLicensesByDriverId/{driverId}")]
        public async Task<IActionResult> GetInternationalLicensesByDriverId(int driverId)
        {
            var result = await licenseService.GetInternationalLicensesByDriverIdAsync(driverId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetInternationalLicenseByLicenseId/{licenseId}")]
        public async Task<IActionResult> GetInternationalLicenseByLicenseId(int licenseId)
        {
            var result = await licenseService.GetInternationalLicenseByLicenseIdAsync(licenseId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetInternationalLicensesByNationalNo")]
        public async Task<IActionResult> GetInternationalLicensesByNationalNo([FromQuery] string nationalNo)
        {
            var result = await licenseService.GetInternationalLicensesByNationalNoAsync(nationalNo);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        //renew license
        [HttpPost("RenewLicense")]
        public async Task<IActionResult> RenewLicenseAsync(RenewLicenseApplicationDTO licenseDTO)
        {
            var result = await licenseService.RenewLicenseAsync(licenseDTO);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        // replace For Damaged or Lost License
        [HttpPost("ReplaceForDamagedLicense")]
        public async Task<IActionResult> ReplaceForDamagedLicenseAsync(RenewLicenseApplicationDTO licenseDTO)
        {
            var result = await licenseService.ReplaceForDamagedLicenseAsync(licenseDTO);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("ReplaceForLostLicense")]
        public async Task<IActionResult> ReplaceForLostLicenseAsync(RenewLicenseApplicationDTO licenseDTO)
        {
            var result = await licenseService.ReplaceForLostLicenseAsync(licenseDTO);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


    }
}
