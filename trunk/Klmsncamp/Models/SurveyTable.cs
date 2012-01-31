using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Klmsncamp.Models
{
    public class SurveyTable
    {
        public int SurveyTableID { get; set; }

        [MaxLength(150, ErrorMessage = "150 karakterden uzun olamaz")]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        [Display(Name = "Anket Taslağı")]
        public int SurveyTemplateID { get; set; }

        [Display(Name = "Anket Taslağı")]
        public virtual SurveyTemplate SurveyTemplate { get; set; }

        public int RequestIssueID { get; set; }

        public virtual RequestIssue RequestIssue { get; set; }

        [MaxLength(50)]
        public string HashKey { get; set; }

        public DateTime TimeStamp { get; set; }

        public DateTime? mTimeStamp { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Onaylandı?")]
        public bool IsApproved { get; set; }
    }
}