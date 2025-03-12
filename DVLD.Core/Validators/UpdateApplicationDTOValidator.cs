using DVLD.Core.DTOs;
using DVLD.Core.Helpers;
using FluentValidation;

namespace DVLD.Core.Validators
{
    public class UpdateApplicationDTOValidator:AbstractValidator<UpdateApplicationDTO>
    {
        public UpdateApplicationDTOValidator()
        {
            RuleFor(a=>a.AppType).NotEmpty()
                .WithMessage("The Application Type is required");
            RuleFor(a => a.AppStatus)
                .NotEmpty().WithMessage("The Application Status is required") 
                .Must(a => AppStatuses.IsValidStatus(a)).WithMessage("Invalid application status");

            RuleFor(a => a.AppDate).NotEmpty()
                .WithMessage("Application Date is required");

        }
    }
}
