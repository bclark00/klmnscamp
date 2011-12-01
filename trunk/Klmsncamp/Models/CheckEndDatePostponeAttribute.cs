using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Klmsncamp.Models
{
    public class CheckEndDatePostponeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var my_model = validationContext.ObjectInstance as RequestIssue;

            if (value != null)
            {
                if (my_model.StartDate > (DateTime)value)
                {
                    string eErrorMessage = "Başlangıç Tarihi Bitiş Tarihinden daha sonra olamaz.";
                    return new ValidationResult(eErrorMessage);
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}