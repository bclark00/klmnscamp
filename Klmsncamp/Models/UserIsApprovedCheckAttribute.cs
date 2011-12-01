using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Klmsncamp.Models
{
    public class UserIsApprovedCheckAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var my_model = validationContext.ObjectInstance as RequestIssue;

            if (my_model.IsApproved == true && (int?)value == null)
            {
                string eErrorMessage = "Onaylanmış kayıtlar için İş Sahibi bilgisi dolu olmalıdır";
                return new ValidationResult(eErrorMessage);
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}