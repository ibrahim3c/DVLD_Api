using System.ComponentModel.DataAnnotations;
namespace DVLD.Core.Helpers
{
    public class BirthDateValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime birthDate)
            {
                if (birthDate > DateTime.Now.AddYears(-18))
                {
                    return new ValidationResult("You must be at least 18 years old.");
                }
            }
            return ValidationResult.Success!;
        }
    }
}
