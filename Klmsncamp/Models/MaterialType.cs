using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Klmsncamp.Models
{
    public class MaterialType
    {
        public int MaterialTypeID { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Açıklama")]
        [MaxLength(150, ErrorMessage = "150 karakterden uzun olamaz")]
        public string Description { get; set; }
    }
}