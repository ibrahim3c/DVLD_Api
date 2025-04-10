using DVLD.Core.Constants;
using DVLD.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.Api.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class DriversController:ControllerBase
    {
        private readonly IDriverServices driverServices;

        public DriversController(IDriverServices driverServices )
        {
            this.driverServices = driverServices;
        }

        [Authorize(Roles = Roles.AdminRole)]
        [HttpGet]
        public async Task<IActionResult>GetAllDriversAsync()
        {
            var result=await driverServices.GetAllDriversAsync();
            if(result.IsSuccess) 
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriverById(int id)
        {
            var result = await driverServices.GetDriversByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("GetDriverByApplicantId/{applicantId}")]
        public async Task<IActionResult> GetDriverByApplicantId(int applicantId)
        {
            var result = await driverServices.GetDriverByApplicantIdAsync(applicantId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = Roles.AdminRole)]
        [HttpGet("GetDriverByApplicantNationalNo")]
        public async Task<IActionResult> GetDriverByApplicantId([FromQuery]string NationalNo)
        {
            var result = await driverServices.GetDriverByApplicantNationalNoAsync(NationalNo);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("IsApplicantDriver/{applicantId}")]
        public async Task<IActionResult> IsApplicantDriver(int applicantId)
        {
            var result = await driverServices.IsApplicantDriver(applicantId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = Roles.AdminRole)]
        [HttpPost]
        public async Task<IActionResult> AddDriver(int applicantId)
        {
            var result = await driverServices.AddDriverAsync(applicantId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


    }
}
