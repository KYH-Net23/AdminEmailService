
using System.ComponentModel.DataAnnotations;

namespace EmailProvider.Models.OrderConfirmationModels
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext context)
        {
            var todaysDate = DateOnly.FromDateTime(DateTime.Now);

            if(value is DateOnly dateValue)
            {
                if (dateValue >= todaysDate)
                {
                    return ValidationResult.Success!;
                }
                return new ValidationResult($"Please enter a date after {todaysDate}");
            }
            return new ValidationResult("Invalid date format. Please enter YYYY-MM-DD");
        }
    }
}