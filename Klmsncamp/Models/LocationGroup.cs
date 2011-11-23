using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Klmsncamp.Models
{
    public class LocationGroup
    {
        public int LocationGroupID { get; set; }

        [MaxLength(150, ErrorMessage = "150 Karakterden fazla olamaz")]
        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
    }
}