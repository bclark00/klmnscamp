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
            var my_model = validationContext.ObjectInstance as RequestIssue;
            DateTime xpresent = DateTime.Now;
            if (my_model.TimeStamp > DateTime.Parse("01.01.0001"))
            {
                xpresent = my_model.TimeStamp;
            }
            DateTime xvalue = (DateTime)value;
            if (xpresent.AddHours(-36) > xvalue)
            {
                string sErrorMessage = "Başlangıç Tarihi, Kayıt tarihinden ( " + xpresent.ToLongDateString() + " " + xpresent.ToLongTimeString() + " ) en fazla 36 saat geri olabilir.";
                return new ValidationResult(sErrorMessage);
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}