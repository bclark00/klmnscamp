using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Klmsncamp.Models
{
    public class RequestState
    {
        public int RequestStateID { get; set; }

        [MaxLength(50, ErrorMessage = "50 Karakterden fazla olamaz")]
        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        public virtual string CapitalizedDescription { get { return char.ToUpper(this.Description[0]) + this.Description.ToLower().Substring(1); } }
    }
}