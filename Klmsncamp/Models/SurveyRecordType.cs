using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Klmsncamp.Models
{
    public class SurveyRecordType
    {
        public int SurveyRecordTypeID { get; set; }

        [MaxLength(50, ErrorMessage = "50 karakterden uzun olamaz")]
        public string Description { get; set; }
    }
}