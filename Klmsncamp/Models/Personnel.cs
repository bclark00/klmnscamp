using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Klmsncamp.Models
{
    public class Personnel
    {
        public int PersonnelID { get; set; }

        [MaxLength(100, ErrorMessage = "100 karakterden uzun olamaz")]
        public string FirstName { get; set; }

        [MaxLength(100, ErrorMessage = "100 karakterden uzun olamaz")]
        public string LastName { get; set; }

        public string getFullName()
        {
            try
            {
                return char.ToUpper(this.FirstName[0]) + (this.FirstName.ToLower()).Substring(1) + " " + char.ToUpper(this.LastName[0]) + (this.LastName.ToLower()).Substring(1);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public virtual string FullName { get { return char.ToUpper(this.FirstName[0]) + (this.FirstName.ToLower()).Substring(1) + " " + char.ToUpper(this.LastName[0]) + (this.LastName.ToLower()).Substring(1); } }

        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Durum")]
        public int ValidationStateID { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Durum")]
        public virtual ValidationState ValidationState { get; set; }
    }
}