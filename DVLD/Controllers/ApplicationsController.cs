using DVLD.Core.DTOs;
using DVLD.Core.Services.Implementations;
using DVLD.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DVLD.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationsController:ControllerBase
    {
        private readonly IApplicationService applicationService;

        public ApplicationsController(IApplicationService applicationService)
        {
            this.applicationService = applicationService;
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
        public async Task<IActionResult> GetAllApplicantApplicationsByNationalNoAsync([FromQuery]string nationalNo)
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
        public async Task<IActionResult> GetApplicationByIdAsync(int id )
        {
            var result = await applicationService.GetApplicationByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetAllApplicationsOfStatusAsync([FromQuery]string status)
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
        public async Task<IActionResult> UpdateApplicationAsync(int id ,UpdateApplicationDTO updateApplicationDTO)
        {
            var result = await applicationService.UpdateApplicationAsync(id,updateApplicationDTO);
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
            var result = await applicationService.GetAllLocalApplicationsLicense();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        //Test
        [HttpPost("ScheduleVisionTest")]
        public async Task<IActionResult> ScheduleVisionTest(int appId, int applicantId)
        {
            var result = await applicationService.ScheduleVisionTestAsync(appId, applicantId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("ScheduleWrittenTest")]
        public async Task<IActionResult> ScheduleWrittenTest(int appId, int applicantId)
        {
            var result = await applicationService.ScheduleWrittenTestAsync(appId, applicantId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("SchedulePracticalTest")]
        public async Task<IActionResult> SchedulePracticalTest(int appId, int applicantId)
        {
            var result = await applicationService.SchedulePracticalTestAsync(appId, applicantId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("UpdateTestAppointment")]
        public async Task<IActionResult> UpdateTestAppointment(int appointmentId,EditTestAppointmentDTO editTestAppointmentDTO)
        {
            var result = await applicationService.EditTestAppointmentAsync(appointmentId, editTestAppointmentDTO);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("TakeTest")]
        public async Task<IActionResult>TakeTestAsync(CompleteTestDTO completeTestDTO)
        {
            var result =await applicationService.CompleteTestAsync(completeTestDTO);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        #endregion



    }
}
