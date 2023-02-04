using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HB.Infrastructure.Validation
{
    public class CustomHbValidation : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int)
            {
                if ((int)value == default)
                {
                    throw new Exception($"{validationContext.DisplayName} cannot be default!");
                }
            }

            if (value is decimal)
            {
                if ((decimal)value <= 0M)
                {
                    throw new Exception($"{validationContext.DisplayName} cannot be under the zero!");
                }
            }

            if ((string)value.ToString() == ValidationControlWorldsString().FirstOrDefault())
            {
                throw new Exception($"{validationContext.DisplayName} cannot be empty or null!");
            }

            if (!string.IsNullOrWhiteSpace(Convert.ToString(value)))
            {
                return ValidationResult.Success;
            }

            throw new Exception($"{validationContext.DisplayName} cannot be empty or null!");
        }

        private IEnumerable<string> ValidationControlWorldsString()
        {
            yield return "string";
            yield return "0";
            yield return "";
            yield return null;
        }

    }

   
}

