using DVLD.Core.DTOs;
using FluentValidation;
using System;

namespace DVLD.Core.Validators
{
    public class UserRegisterDTOValidator: AbstractValidator<UserRegisterDTO>
    {

        public UserRegisterDTOValidator()
        {
            RuleFor(x => x.NationalNo)
                .NotEmpty().WithMessage("National number is required.")
                .Matches(@"^\d{10,15}$").WithMessage("National number must be between 10 and 15 digits.");

            RuleFor(x => x.Fname)
                .NotEmpty().WithMessage("First name is required.")
                .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");

            RuleFor(x => x.Sname)
                .NotEmpty().WithMessage("Second name is required.")
                .Length(2, 50).WithMessage("Second name must be between 2 and 50 characters.");

            RuleFor(x => x.Tname)
                .NotEmpty().WithMessage("Third name is required.")
                .MaximumLength(50).WithMessage("Third name cannot exceed 50 characters.");

            RuleFor(x => x.Lname)
                .NotEmpty().WithMessage("Last name is required.")
                .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");

            RuleFor(x => x.CountryId)
                .InclusiveBetween(1, 82).WithMessage("Invalid Country ID. Country ID must be between 1 and 82.");

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Invalid gender value.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Birthdate is required.")
                .Must(BeAtLeast18).WithMessage("You must be at least 18 years old.")
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Birthdate cannot be in the future.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .Length(5, 255).WithMessage("Address must be between 5 and 255 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("The password and confirmation password do not match.");
        }

        private bool BeAtLeast18(DateTime birthDate)
        {
            return birthDate <= DateTime.Today.AddYears(-18);
        }

    }
}
