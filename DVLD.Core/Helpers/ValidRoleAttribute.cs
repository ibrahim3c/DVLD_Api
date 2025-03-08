using DVLD.Core.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace DVLD.Core.Helpers
{
    public class ValidRoleAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string role)
            {
                // Get all role values from the Roles class
                var validRoles = typeof(Roles)
                    .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .Where(f => f.IsLiteral && !f.IsInitOnly)
                    .Select(f => f.GetValue(null)?.ToString())
                    .ToList();

                if (validRoles.Contains(role))
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(ErrorMessage ?? "Invalid role.");
        }
    }

}
