using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Klmsncamp.Models
{
    public class CorporateAccount
    {
        public int CorporateAccountID { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Ünvan")]
        [MaxLength(150, ErrorMessage = "150 karakterden uzun olamaz")]
        public string Title { get; set; }

        [Display(Name = "Adres")]
        [MaxLength(150, ErrorMessage = "150 karakterden uzun olamaz")]
        public string Address { get; set; }

        [Display(Name = "Tel-1")]
        [MaxLength(20, ErrorMessage = "20 karakterden uzun olamaz")]
        public string Phone1 { get; set; }

        [Display(Name = "Tel-2")]
        [MaxLength(20, ErrorMessage = "20 karakterden uzun olamaz")]
        public string Phone2 { get; set; }

        [Display(Name = "Yetkili Kişi")]
        [MaxLength(150, ErrorMessage = "150 karakterden uzun olamaz")]
        public string ContactPerson { get; set; }

        [Display(Name = "Email")]
        [MaxLength(150, ErrorMessage = "150 karakterden uzun olamaz")]
        public string CorpEmail { get; set; }

        [Display(Name = "Tipi")]
        public int? CorporateTypeID { get; set; }

        public virtual CorporateType CorporateType { get; set; }

        [Display(Name = "Kullanıcı mı?")]
        public int? UserID { get; set; }

        public virtual User User { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Durum")]
        public int ValidationStateID { get; set; }

        public virtual ValidationState ValidationState { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<RequestIssue> RequestIssues { get; set; }
    }
}