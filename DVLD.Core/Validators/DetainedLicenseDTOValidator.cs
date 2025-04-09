using DVLD.Core.DTOs;
using FluentValidation;

namespace DVLD.Core.Validators
{
    public class DetainedLicenseDTOValidator : AbstractValidator<DetainedLicenseDTO>
    {
        public DetainedLicenseDTOValidator()
        {
            RuleFor(x => x.FineFees)
                .GreaterThanOrEqualTo(0).WithMessage("Fine fees must be 0 or greater.");

            RuleFor(x => x.DetainedDate)
                .NotEmpty().WithMessage("Detained date is required.")
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Detained date cannot be in the future.");

            RuleFor(x => x.ReleasedDate)
                .GreaterThanOrEqualTo(x => x.DetainedDate)
                .When(x => x.ReleasedDate.HasValue)
                .WithMessage("Released date cannot be before detained date.");

            RuleFor(x => x.LicenseId)
                .GreaterThan(0).WithMessage("License ID must be greater than 0.");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Reason is required.")
                .MaximumLength(255).WithMessage("Reason must be at most 255 characters.");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes must be at most 500 characters.");
        }
    }
}
