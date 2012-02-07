using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Klmsncamp.Models
{
    public class ParameterSetting
    {
        public int ParameterSettingID { get; set; }

        [MaxLength(100, ErrorMessage = "100 karakterden uzun olamaz")]
        [Display(Name = "Parametre Adı")]
        public string Description { get; set; }

        [Display(Name = "Parametre Değeri")]
        [MaxLength(100, ErrorMessage = "100 karakterden uzun olamaz")]
        public string ParameterValue { get; set; }
    }
}