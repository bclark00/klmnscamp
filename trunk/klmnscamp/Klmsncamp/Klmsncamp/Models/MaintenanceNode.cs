using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Klmsncamp.Models
{
    public class MaintenanceNode
    {
        public int MaintenanceNodeID { get; set; }

        [Required(ErrorMessage = "zorunlu alan")]
        [MaxLength(50, ErrorMessage = "50 karakterden uzun olamaz")]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
    }
}