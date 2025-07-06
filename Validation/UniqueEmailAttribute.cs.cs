
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using IdentityText.Data;
using System.Linq;

namespace IdentityText.Validation
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = validationContext.GetService<ApplicationDbContext>();
            var email = value as string;

            if (email != null && dbContext.Users.Any(u => u.Email == email))
            {
                return new ValidationResult("هذا البريد الإلكتروني مستخدم بالفعل.");
            }
            return ValidationResult.Success;
        }
    }
}

