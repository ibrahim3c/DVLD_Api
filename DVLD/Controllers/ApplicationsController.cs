using DVLD.Core.DTOs;
using DVLD.Core.Helpers;
using DVLD.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService applicationService;
        private readonly ITestService testService;

        public ApplicationsController(IApplicationService applicationService, ITestService testService)
        {
            this.applicationService = applicationService;
            this.testService = testService;
        }
        #region ManageApps
        [HttpGet]
        public async Task<IActionResult> GetAllApplicationsAsync()
        {
            var result = await applicationService.GetAllApplicationAsync();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("ApplicantApplicationByNationalNo")]
        public async Task<IActionResult> GetAllApplicantApplicationsByNationalNoAsync([FromQuery] string nationalNo)
        {
            var result = await applicationService.GetAllApplicantApplicationsByNationalNoAsync(nationalNo);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpGet("ApplicantApplicationById/{applicantId}")]
        public async Task<IActionResult> GetAllApplicantApplicationsByNationalNoAsync(int applicantId)
        {
            var result = await applicationService.GetAllApplicantApplicationsByIdAsync(applicantId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplicationByIdAsync(int id)
        {
            var result = await applicationService.GetApplicationByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetAllApplicationsOfStatusAsync([FromQuery] string status)
        {
            var result = await applicationService.GetAllApplicationWithStatusAsync(status);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicationByIdAsync(int id)
        {
            var result = await applicationService.DeleteApplicationAsync(id);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApplicationAsync(int id, UpdateApplicationDTO updateApplicationDTO)
        {
            var result = await applicationService.UpdateApplicationAsync(id, updateApplicationDTO);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpPut("Accept/{applicationId}")]
        public async Task<IActionResult> AcceptApplication(int applicationId)
        {
            var result = await applicationService.ApproveTheApplicationAsync(applicationId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("Reject/{applicationId}")]
        public async Task<IActionResult> RejectApplication(int applicationId)
        {
            var result = await applicationService.RejectTheApplicationAsync(applicationId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        #endregion

        #region LocalDrivingLicense
        [HttpPost("ApplyForNewLocalDrivingLicense")]
        public async Task<IActionResult> ApplyForNewLocalDrivingLincense(int applicantId, int classLicensId)
        {
            var result = await applicationService.ApplyForNewLocalDrivingLincense(applicantId, classLicensId);
            if (result.IsSuccess)
                //return Ok(result);
                return CreatedAtAction(nameof(GetApplicationByIdAsync), new { id = result.Value }, result);
            return BadRequest(result);
        }

        [HttpGet("GetAllLocalAppsLicense")]
        public async Task<IActionResult> GetAllLocalApplicationsLicense()
        {
            var result = await applicationService.GetAllLocalAppLicensesAsync();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpGet("GetLocalAppLicenseByIdAsync/{id}")]
        public async Task<IActionResult> GetLocalAppLicenseByIdAsync(int id)
        {
            var result = await applicationService.GetLocalAppLicenseByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }



        [HttpGet("GetAllLocalAppLicensesByNationalNoAsync")]
        public async Task<IActionResult> GetAllLocalAppLicensesWithsByNationalNoAsync([FromQuery] string nationalNo)
        {
            var result = await applicationService.GetAllLocalAppLicensesWithsByNationalNoAsync(nationalNo);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetAllLocalAppLicensesByApplicantIdAsync/{applicantId}" )]
        public async Task<IActionResult> GetAllLocalAppLicensesByApplicantIdAsync(int applicantId)
        {
            var result = await applicationService.GetAllLocalAppLicensesByApplicantIdAsync(applicantId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        //Test
        [HttpPost("ScheduleVisionTest")]
        public async Task<IActionResult> ScheduleVisionTest(int appId, int applicantId)
        {
            var result = await testService.ScheduleVisionTestAsync(appId, applicantId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("ScheduleWrittenTest")]
        public async Task<IActionResult> ScheduleWrittenTest(int appId, int applicantId)
        {
            var result = await testService.ScheduleWrittenTestAsync(appId, applicantId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("SchedulePracticalTest")]
        public async Task<IActionResult> SchedulePracticalTest(int appId, int applicantId)
        {
            var result = await testService.SchedulePracticalTestAsync(appId, applicantId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("UpdateTestAppointment")]
        public async Task<IActionResult> UpdateTestAppointment(int appointmentId,EditTestAppointmentDTO editTestAppointmentDTO)
        {
            var result = await testService.EditTestAppointmentAsync(appointmentId, editTestAppointmentDTO);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("TakeTest")]
        public async Task<IActionResult>TakeTestAsync(CompleteTestDTO completeTestDTO)
        {
            var result =await testService.CompleteTestAsync(completeTestDTO);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        #endregion

        #region InternationalLicenseApp
        [HttpPost("ApplyForNewInternationalLicenseApplication/{applicantId}")]
        public async Task<IActionResult> ApplyNewForInternationalLicenseApplication(int applicantId)
        {
            var result = await applicationService.ApplyForNewInternationalLicenseApplicationAsync(applicantId);
            if (result.IsSuccess)
                //return Ok(result);
                return CreatedAtAction(nameof(GetApplicationByIdAsync), new { id = result.Value }, result);
            return BadRequest(result);
        }
        [HttpGet("GetAllInternationalLicenseApps")]
        public async Task<IActionResult> GetAllInternationalLicenseApps()
        {
            var result = await applicationService.GetAllApplicationAsync((int)AppTypes.NewInternationalDrivingLicense);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpGet("GetInternationalLicenseAppByIdAsync/{id}")]
        public async Task<IActionResult> GetInternationalLicenseAppByIdAsync(int id)
        {
            var result = await applicationService.GetApplicationByIdAsync(id, (int)AppTypes.NewInternationalDrivingLicense);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }



        [HttpGet("GetAllInternationalLicenseAppsWithsByNationalNoAsync")]
        public async Task<IActionResult> GetAllInternationalLicenseAppsWithsByNationalNoAsync([FromQuery] string nationalNo)
        {
            var result = await applicationService.GetAllApplicantApplicationsByNationalNoAsync(nationalNo,(int)AppTypes.NewInternationalDrivingLicense);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetAllInternationalLicenseAppByApplicantIdAsync/{applicantId}")]
        public async Task<IActionResult> GetAllInternationalLicenseAppByApplicantIdAsync(int applicantId)
        {
            var result = await applicationService.GetAllApplicantApplicationsByIdAsync(applicantId,(int)AppTypes.NewInternationalDrivingLicense);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        #endregion

        #region RenewLicenseApp
        [HttpPost("ApplyForRenewLicenseApplicantion/{licenseId}")]
        public async Task<IActionResult> ApplyForRenewLicenseApplicantion(int licenseId)
        {
            var result = await applicationService.ApplyForRenewLicenseApplicationAsync(licenseId);
            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetApplicationByIdAsync), new { id = result.Value }, result);
            return BadRequest(result);
        }

        [HttpGet("GetAllRenewLicenseApps")]
        public async Task<IActionResult> GetAllRenewLicenseApps()
        {
            var result = await applicationService.GetAllApplicationAsync((int)AppTypes.RenewDrivingLicense);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpGet("GetRenewLicenseAppByIdAsync/{id}")]
        public async Task<IActionResult> GetRenewLicenseAppByIdAsync(int id)
        {
            var result = await applicationService.GetApplicationByIdAsync(id, (int)AppTypes.RenewDrivingLicense);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }



        [HttpGet("GetAllRenewLicenseAppsWithsByNationalNoAsync")]
        public async Task<IActionResult> GetAllRenewLicenseAppsWithsByNationalNoAsync([FromQuery] string nationalNo)
        {
            var result = await applicationService.GetAllApplicantApplicationsByNationalNoAsync(nationalNo, (int)AppTypes.RenewDrivingLicense);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetAllRenewLicenseAppByApplicantIdAsync/{applicantId}")]
        public async Task<IActionResult> GetAllRenewLicenseAppByApplicantIdAsync(int applicantId)
        {
            var result = await applicationService.GetAllApplicantApplicationsByIdAsync(applicantId, (int)AppTypes.RenewDrivingLicense);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }



        #endregion



    }
}
