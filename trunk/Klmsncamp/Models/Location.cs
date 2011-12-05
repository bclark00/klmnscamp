using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Klmsncamp.Models
{
    public class Location
    {
        public int LocationID { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Açıklama")]
        [MaxLength(50, ErrorMessage = "50 karakterden uzun olamaz")]
        public string Description { get; set; }

        public virtual string CapitalizedDescription { get { return char.ToUpper(this.Description[0]) + this.Description.ToLower().Substring(1); } }

        [Display(Name = "Ana Departman")]
        public int? LocationGroupID { get; set; }

        [Display(Name = "Ana Departman")]
        public virtual LocationGroup LocationGroup { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Durum")]
        public int ValidationStateID { get; set; }

        public virtual ValidationState ValidationState { get; set; }
    }
}