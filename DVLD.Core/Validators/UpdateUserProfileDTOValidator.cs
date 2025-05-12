
using DVLD.Core.DTOs;
using FluentValidation;

    namespace DVLD.Core.Validators
    {
        public sealed class UserProfileDTOValidator : AbstractValidator<UserProfileDTO>
        {
            public UserProfileDTOValidator()
            {
                RuleFor(x => x.NationalNo)
                    .NotNull()
                    .NotEmpty().WithMessage("National number is required.")
                    .Matches(@"^\d{10,15}$").WithMessage("National number must be between 10 and 15 digits.");

                RuleFor(x => x.Fname)
                    .NotNull()
                    .NotEmpty().WithMessage("First name is required.")
                    .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");

                RuleFor(x => x.Sname)
                    .NotNull()
                    .NotEmpty().WithMessage("Second name is required.")
                    .Length(2, 50).WithMessage("Second name must be between 2 and 50 characters.");

                RuleFor(x => x.Tname)
                    .NotNull()
                    .NotEmpty().WithMessage("Third name is required.")
                    .MaximumLength(50).WithMessage("Third name cannot exceed 50 characters.");

                RuleFor(x => x.Lname)
                    .NotNull()
                    .NotEmpty().WithMessage("Last name is required.")
                    .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");

                RuleFor(x => x.CountryId)
                    .NotNull()
                    .InclusiveBetween(1, 82).WithMessage("Invalid Country ID. Country ID must be between 1 and 82.");

                RuleFor(x => x.Gender)
                    .NotNull()
                    .IsInEnum().WithMessage("Invalid gender value.");

                RuleFor(x => x.BirthDate)
                    .NotNull()
                    .NotEmpty().WithMessage("Birthdate is required.")
                    .Must(BeAtLeast18).WithMessage("You must be at least 18 years old.")
                    .LessThanOrEqualTo(DateTime.Today).WithMessage("Birthdate cannot be in the future.");

                RuleFor(x => x.Address)
                    .NotNull()
                    .NotEmpty().WithMessage("Address is required.")
                    .Length(5, 255).WithMessage("Address must be between 5 and 255 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number is required.")
                .Matches(@"^(010|011|012|015)\d{8}$")
                .WithMessage("Invalid Egyptian phone number format. It should start with 010, 011, 012, or 015 and be 11 digits long.");
        }

        private bool BeAtLeast18(DateTime birthDate)
            {
                return birthDate <= DateTime.Today.AddYears(-18);
            }
        }
    }

