using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Klmsncamp.Models
{
    public class SurveyRecord
    {
        public int SurveyRecordID { get; set; }

        public int SurveyNodeID { get; set; }

        public virtual SurveyNode SurveyNode { get; set; }

        public bool ApprovalStatus { get; set; }

        public int? Score { get; set; }

        public int SurveyRecordTypeID { get; set; }

        public virtual SurveyRecordType SurveyRecordType { get; set; }

        [MaxLength(150, ErrorMessage = "150 karakterden uzun olamaz")]
        public string Note { get; set; }

        [Display(Name = "Sıra No")]
        public int? OrderNum { get; set; }

        public ICollection<SurveyTemplate> SurveyTemplates { get; set; }
    }
}