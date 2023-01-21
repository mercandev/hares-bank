using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HB.Infrastructure.Validation
{
    public class CustomHbValidation : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value.ToString() == ValidationControlWorlds().FirstOrDefault())
            {
                throw new Exception($"{validationContext.DisplayName} cannot be empty or null!");
            }

            if (!string.IsNullOrWhiteSpace(Convert.ToString(value)))
            {
                return ValidationResult.Success;
            }

            throw new Exception($"{validationContext.DisplayName} cannot be empty or null!");
        }

        private IEnumerable<string> ValidationControlWorlds()
        {
            yield return "string";
            yield return "0";
            yield return "";
        }
    }

   
}

