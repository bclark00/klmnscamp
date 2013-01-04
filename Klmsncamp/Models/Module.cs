using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Klmsncamp.Models
{
    public class Module
    {
        public int ModuleID { get; set; }

        [Display(Name = "Açıklama")]
        [Required(ErrorMessage = "Zorunlu Alan")]
        [MaxLength(150, ErrorMessage = "150 karakterden fazla olamaz")]
        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; }


    }
}