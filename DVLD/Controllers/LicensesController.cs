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


    }
}
