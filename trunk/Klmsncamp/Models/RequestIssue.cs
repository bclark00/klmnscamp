using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Klmsncamp.Models
{
    public class RequestIssue
    {
        public int RequestIssueID { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "İş/İstek Türü")]
        public int RequestTypeID { get; set; }

        public virtual RequestType RequestType { get; set; }

        [Display(Name = "İşi İsteyen Personel")]
        public int? PersonnelID { get; set; }

        [Display(Name = "İşi İsteyen Personel")]
        public virtual Personnel Personnel { get; set; }

        [MaxLength(500, ErrorMessage = "500 karakterden uzun olamaz")]
        [Display(Name = "Açıklama")]
        [Required(ErrorMessage = "Zorunlu Alan")]
        public string DetailedDescription { get; set; }

        [Display(Name = "Lokasyon")]
        [Required(ErrorMessage = "Zorunlu Alan")]
        public int LocationID { get; set; }

        [Display(Name = "İş / Arıza Bildirim Yeri")]
        public virtual Location Location { get; set; }

        [Display(Name = "Cihaz")]
        public int? InventoryID { get; set; }

        [Display(Name = "Envanter / Cihaz")]
        public virtual Inventory Inventory { get; set; }

        [Display(Name = "Atölye")]
        [Required(ErrorMessage = "Zorunlu Alan")]
        public int WorkshopID { get; set; }

        [Display(Name = "İş İstenen Atölye")]
        public virtual Workshop Workshop { get; set; }

        [Display(Name = "İş/İstek Durum")]
        public int? RequestStateID { get; set; }

        [Display(Name = "İş/İstek Durum")]
        public virtual RequestState RequestState { get; set; }

        [MaxLength(500, ErrorMessage = "500 karakterden uzun olamaz")]
        [Display(Name = "Süreç Bilgilendirme Notu")]
        public string Note { get; set; }

        [Display(Name = "Planlanan Başlangıç Tarihi")]
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Zorunlu Alan")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Planlanan Bitiş Tarihi")]
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "İstek Yapan")]
        [ForeignKey("UserReq")]
        public int? UserReqID { get; set; }

        public virtual User UserReq { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Onaylandı?")]
        public bool IsApproved { get; set; }

        [Display(Name = "Email ile bildirim yapılsın mı?")]
        public bool SendEmail { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Tüm gün?")]
        public bool IsAllDay { get; set; }

        public DateTime TimeStamp { get; set; }

        [ForeignKey("User")]
        public int? UserID { get; set; }

        [Display(Name = "Kullanıcı")]
        public virtual User User { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Durum")]
        public int ValidationStateID { get; set; }

        public virtual ValidationState ValidationState { get; set; }
    }
}