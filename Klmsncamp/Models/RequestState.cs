using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Klmsncamp.Models
{
    public class RequestState
    {
        public int RequestStateID { get; set; }

        [MaxLength(50, ErrorMessage = "50 Karakterden fazla olamaz")]
        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }


    }
}