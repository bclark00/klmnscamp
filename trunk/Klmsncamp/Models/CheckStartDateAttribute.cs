using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Klmsncamp.Models
{
    public class CheckStartDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime xpresent = DateTime.Now;
            DateTime xvalue = (DateTime)value;
            if (xpresent.AddHours(-18) > xvalue)
            {
                string sErrorMessage = "Başlangıç Tarihi, şimdiden ( " + xpresent.ToLongDateString() + " " + xpresent.ToLongTimeString() + " ) en fazla 18 saat geri olabilir.";
                return new ValidationResult(sErrorMessage);
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}