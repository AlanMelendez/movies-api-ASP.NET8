using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Validations
{
    public class FirstCapitalLetterAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()) )
            { 
                // If there is no letter to validate, return success.
                return ValidationResult.Success;
            }

            var firstLetter = value.ToString()![0].ToString(); // Ex: "jose" get char: "j"

            if(firstLetter  != firstLetter.ToUpper())
            {
                // If is different char j to J , validation is invalid.
                return new ValidationResult("The first letter must be capital.");
            }


            return ValidationResult.Success;
        }
    }
}
