using System.ComponentModel.DataAnnotations;
using PatientSystem.Services.Models;

namespace PatientAPI.Controllers
{
    public class ValidValuesAttribute : ValidationAttribute
    {
        string[] _args;

        public ValidValuesAttribute(params string[] args)
        {
            _args = args;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (_args.Contains((string)value))
                return ValidationResult.Success;
            return new ValidationResult("Invalid value.");
        }
    }

    public class PatientRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Range(0, 120)]
        public int Age { get; set; }
        [Required]
        [ValidValues("Male", "Female", "Other")]
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
    }
}
