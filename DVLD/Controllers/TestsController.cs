using DVLD.Core.Constants;
using DVLD.Core.DTOs;
using DVLD.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TestsController:ControllerBase
    {
        private readonly ITestService testService;

        public TestsController(ITestService testService)
        {
            this.testService = testService;
        }

        [HttpGet]
        [Authorize(Roles =Roles.AdminRole)]
        public async Task<IActionResult> GetAllTestAppoinments()
        {
            var result = await testService.GetAllTestAppoinments();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetAllTestAppoinmentByApplicantId/{applicantId}")]
        public async Task<IActionResult> GetAllApplicantTestAppoinments(int applicantId)
        {
            var result = await testService.GetAllTestAppoinmentByApplicantId(applicantId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> GetTestAppointmentById(int Id)
        {
            var result = await testService.GetTestAppointmentById(Id);
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
        public async Task<IActionResult> UpdateTestAppointment(int appointmentId, EditTestAppointmentDTO editTestAppointmentDTO)
        {
            var result = await testService.EditTestAppointmentAsync(appointmentId, editTestAppointmentDTO);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("TakeTest")]
        public async Task<IActionResult> TakeTestAsync(CompleteTestDTO completeTestDTO)
        {
            var result = await testService.CompleteTestAsync(completeTestDTO);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetTestResultByTestAppoinmentId/{TestAppoinmentId}")]
        public async Task<IActionResult> GetTestResultByTestAppoinmentId(int TestAppoinmentId)
        {
            var result = await testService.GetTestResultByTestAppoinmentId(TestAppoinmentId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

    }
}
