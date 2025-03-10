using DVLD.Core.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost]
        public async Task<IActionResult> ApplyForNewLocalDrivingLincense(int applicantId,int classLicensId)
        {
            var result=await applicationService.ApplyForNewLocalDrivingLincense(applicantId, classLicensId);
            if(result.IsSuccess) 
                //change this to created after we did getApplicantionByIdAsync
                return Ok(result);
            return BadRequest(result);

        }

    }
}
