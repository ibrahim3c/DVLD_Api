using System.ComponentModel.DataAnnotations;

namespace DVLD.Core.Helpers
{
    internal class ObjectValidator
    {
        public static List<string> Validate<T>(T obj)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(obj);

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

            return isValid ? new List<string>() : validationResults.Select(v => v.ErrorMessage).ToList();
        }
    }

}

