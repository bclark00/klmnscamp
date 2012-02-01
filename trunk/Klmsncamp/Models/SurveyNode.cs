using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Klmsncamp.Models
{
    public class SurveyNode
    {
        public int SurveyNodeID { get; set; }

        [MaxLength(150, ErrorMessage = "150 Karakterden büyük olamaz")]
        [Required(ErrorMessage = " Zorunlu Alan ")]
        public string Description { get; set; }
    }
}