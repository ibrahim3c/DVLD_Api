using DVLD.Core.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DVLD.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationsController:ControllerBase
    {
        private readonly ApplicationService applicationService;

        public ApplicationsController(ApplicationService applicationService)
        {
            this.applicationService = applicationService;
        }


        #region LocalDrivingLicense
        [HttpPost]
        public async Task<IActionResult> ApplyForNewLocalDrivingLincense(int applicantId, int classLicensId)
        {
            var result = await applicationService.ApplyForNewLocalDrivingLincense(applicantId, classLicensId);
            if (result.IsSuccess)
                //change this to created after we did getApplicantionByIdAsync
                return Ok(result);
            return BadRequest(result);
        }
        //Test
        [HttpPost]
        public async Task<IActionResult> ScheduleVisionTest(int appId, int applicantId)
        {
            var result = await applicationService.ScheduleVisionTestAsync(appId, applicantId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleWrittenTest(int appId, int applicantId)
        {
            var result = await applicationService.ScheduleWrittenTestAsync(appId, applicantId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost]
        public async Task<IActionResult> SchedulePractivalTest(int appId, int applicantId)
        {
            var result = await applicationService.SchedulePracticalTestAsync(appId, applicantId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        #endregion

        #region ManageApps

        [HttpPut]
        public async Task<IActionResult> AcceptApplication(int applicationId)
        {
            var result = await applicationService.ApproveTheApplicationAsync(applicationId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> RejectApplication(int applicationId)
        {
            var result = await applicationService.RejectTheApplicationAsync(applicationId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        #endregion


    }
}
