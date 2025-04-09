using DVLD.Core.DTOs;
using FluentValidation;

namespace DVLD.Core.Validators
{
    public class AddInternationalLicenseDTOValidator:AbstractValidator<AddInternationalLicenseDTO>
    {
        public AddInternationalLicenseDTOValidator()
        {
            RuleFor(x => x.AppId)
                .GreaterThan(0).WithMessage("Application ID must be greater than 0.");

            RuleFor(x => x.PaidFees)
                .GreaterThanOrEqualTo(0).WithMessage("Paid fees must be 0 or more.");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
        }
    }
}
