using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Klmsncamp.Models
{
    public class SurveyTemplate
    {
        public int SurveyTemplateID { get; set; }

        [Display(Name = "Anket Taslağı Açıklama")]
        [MaxLength(150, ErrorMessage = "150 karakterden uzun olamaz")]
        [Required(ErrorMessage = " Zorunlu Alan ")]
        public string Description { get; set; }

        [Display(Name = "İş Talep Tipi")]
        [Required(ErrorMessage = " Zorunlu Alan ")]
        public int RequestTypeID { get; set; }

        [Display(Name = "İş Talep Tipi")]
        public virtual RequestType RequestType { get; set; }

        [Display(Name = "Anket Satırları")]
        public virtual ICollection<SurveyRecord> SurveyRecords { get; set; }

        [Display(Name = "Ön Tanımlı mı?")]
        public bool PreDefined { get; set; }
    }
}