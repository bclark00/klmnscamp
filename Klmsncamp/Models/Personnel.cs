﻿using System;
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
        [Display(Name = "Adı")]
        public string FirstName { get; set; }

        [MaxLength(100, ErrorMessage = "100 karakterden uzun olamaz")]
        [Display(Name = "Soyadı")]
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

        [Display(Name = "Adı-Soyadı")]
        public virtual string FullName { get { if (this.Location != null) { return char.ToUpper(this.FirstName[0]) + (this.FirstName.ToLower()).Substring(1) + " " + char.ToUpper(this.LastName[0]) + (this.LastName.ToLower()).Substring(1) + " (" + this.Location.CapitalizedDescription + ")"; } else { return char.ToUpper(this.FirstName[0]) + (this.FirstName.ToLower()).Substring(1) + " " + char.ToUpper(this.LastName[0]) + (this.LastName.ToLower()).Substring(1); } } }

        [Display(Name = "Departman")]
        public int? LocationID { get; set; }

        [Display(Name = "Departman")]
        public virtual Location Location { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Durum")]
        public int ValidationStateID { get; set; }

        [Display(Name = "Durum")]
        public virtual ValidationState ValidationState { get; set; }

        [MaxLength(100, ErrorMessage = "100 karakterden uzun olamaz")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Geçerli Bir Mail Adresi Değil")]
        public string Email { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public int? UserID { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public virtual User User{ get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<RequestIssue> RequestIssues { get; set; }
    }
}