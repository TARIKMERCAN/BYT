using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementSystem.Validation
{
    public class NonDefaultDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            // Ensure the value is a DateTime and check for default value
            if (value is DateTime dateValue && dateValue == DateTime.MinValue)
            {
                return new ValidationResult(ErrorMessage ?? "Date cannot be the default value (01/01/0001).");
            }

            // If valid, return success
            return ValidationResult.Success!;
        }
    }
}
