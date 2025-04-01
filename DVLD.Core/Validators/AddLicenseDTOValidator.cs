using DVLD.Core.DTOs;
using FluentValidation;

namespace DVLD.Core.Validators
{
    public sealed class AddLicenseDTOValidator: AbstractValidator<AddLicenseDTO>
    {
        public AddLicenseDTOValidator()
        {
            RuleFor(x => x.LicenseClassId)
                .GreaterThan(0).WithMessage("License class ID must be greater than 0.");

            RuleFor(x => x.NationalNo)
                    .NotNull()
                   .NotEmpty().WithMessage("National number is required.")
                   .Matches(@"^\d{10,15}$").WithMessage("National number must be between 10 and 15 digits.");

            RuleFor(x => x.AppId)
                .GreaterThan(0).WithMessage("Application ID must be greater than 0.");

            RuleFor(x => x.PaidFees)
                .GreaterThanOrEqualTo(0).WithMessage("Paid fees must be 0 or more.");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
        }
    }
}
