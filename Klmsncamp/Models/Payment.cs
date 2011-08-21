using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Klmsncamp.Models
{
    public class Payment
    {
        public int PaymentID { get; set; }

        [Display(Name = "Bütçe/Ref No")]
        [MaxLength(50, ErrorMessage = "50 karakterden fazla olamaz")]
        public string BudgetNum { get; set; }

        [Display(Name = "Bütçe/Ref No")]
        [MaxLength(50, ErrorMessage = "50 karakterden fazla olamaz")]
        public string PurchaseNum { get; set; }

        public int? CorporateAccountID { get; set; }

        [Display(Name = "Markası")]
        public virtual CorporateAccount CorporateAccount { get; set; }

        [Display(Name = "Fatura Tarihi")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Zorunlu Alan")]
        public DateTime InvoiceDate { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Seri No")]
        [StringLength(20, ErrorMessage = "{0} en az  {2} karakter uzunluğunda olmalı.", MinimumLength = 2)]
        public string InvoiceNum { get; set; }

        [DisplayFormat(DataFormatString = "{0:C4}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        [Display(Name = "Fatura Tutarı")]
        public decimal InvoiceTotal { get; set; }

        [Display(Name = "Ödeme Tarihi")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? PaymentDate { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Açıklama")]
        [StringLength(200, ErrorMessage = "{0} en az  {2} karakter uzunluğunda olmalı.", MinimumLength = 2)]
        public string Description { get; set; }

        [Display(Name = "İş / İstek Talebi")]
        public int? RequestIssueID { get; set; }

        [Display(Name = "İş / İstek Talebi")]
        public virtual RequestIssue RequestIssue { get; set; }

        public virtual ICollection<PaymentFile> PaymentFiles { get; set; }
    }
}