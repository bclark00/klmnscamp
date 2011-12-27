using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Klmsncamp.Models
{
    public class Project
    {
        public int ProjectID { get; set; }

        [MaxLength(500, ErrorMessage = "500 karakterden uzun olamaz")]
        [Display(Name = "Açıklama")]
        [Required(ErrorMessage = "Zorunlu Alan")]
        public string Description { get; set; }

        [Display(Name = "Planlanan Başlangıç Tarihi")]
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Zorunlu Alan")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Planlanan Bitiş Tarihi")]
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime? EndDate { get; set; }

        [Display(Name = "İlgili Firmalar")]
        public ICollection<CorporateAccount> CorporateAccounts { get; set; }

        [Display(Name = "İlgili Departmanlar")]
        public ICollection<Location> Locations { get; set; }

        [Display(Name = "İlgili Personel")]
        public ICollection<Personnel> Personnels { get; set; }

        [Display(Name = "İş/İstek Durum")]
        public int? RequestStateID { get; set; }

        [Display(Name = "İş/İstek Durum")]
        public virtual RequestState RequestState { get; set; }

        [ForeignKey("User")]
        [Display(Name = "Proje Sahibi")]
        [Required(ErrorMessage = "Proje Sahibi/Yönetici Alanı Zorunludur")]
        public int UserID { get; set; }

        [Display(Name = "İş Sahibi")]
        public virtual User User { get; set; }

        [Display(Name = "Kullanıcı")]
        [ForeignKey("cUser")]
        public int cUserID { get; set; }

        public virtual User cUser { get; set; }

        public DateTime TimeStamp { get; set; }

        public virtual ICollection<RequestIssue> RequestIssues { get; set; }

        [Display(Name = "Açıklama (Uzun)")]
        public virtual string MultiboxDescription { get { int substrlen = this.Description.Length; if (substrlen > 50) { substrlen = 50; } return "#" + this.ProjectID.ToString() + ". " + this.Description.ToLower().Substring(0, substrlen) + ".. - " + this.User.FullName + " - " + this.StartDate.ToShortDateString(); } }
    }
}