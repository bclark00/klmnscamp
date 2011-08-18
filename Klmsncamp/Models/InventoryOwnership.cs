using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Klmsncamp.Models
{
    public class InventoryOwnership
    {
        public int InventoryOwnershipID { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Açıklama")]
        [MaxLength(50, ErrorMessage = "50 karakterden uzun olamaz")]
        public string Description { get; set; }
    }
}