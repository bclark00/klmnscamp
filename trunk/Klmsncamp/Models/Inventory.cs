using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;

namespace Klmsncamp.Models
{
    public class Inventory
    {
        public int InventoryID { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Açıklama")]
        [MaxLength(100, ErrorMessage = "100 Karakterden fazla olamaz")]
        public string Description { get; set; }
        
        [Required(ErrorMessage="Zorunlu Alan")]
        [Display(Name="Seri No")]
        [MaxLength(100,ErrorMessage="100 Karakterden fazla olamaz")]
        public string SerialNumber { get; set; }

        [Display(Name = "Lokasyon")]
        public int? LocationID { get; set; }

        public virtual Location Location { get; set; }

        
        [ForeignKey("CorporateAccount")]
        public int? CorporateAccountID { get; set; }

        [Display(Name = "Markası")]
        public virtual CorporateAccount CorporateAccount { get; set; }

        [Display(Name = "Model")]
        [MaxLength(100, ErrorMessage = "100 Karakterden fazla olamaz")]
        public string BrandModel { get; set; }

        [Display(Name="Demirbaş No")]
        [MaxLength(100, ErrorMessage="100 Karakterden fazla olamaz")]
        public string InventoryStockNo { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Durum")]
        public int ValidationStateID { get; set; }

        public virtual ValidationState ValidationState { get; set; }

        [Display(Name="Ek Not")]
        [MaxLength(450, ErrorMessage="450 Karakterden fazla olamaz")]
        public string Notes { get; set; }

        [Display(Name="Aidiyet")]
        [Required(ErrorMessage="Zorunlu Alan")]
        public int InventoryOwnershipID { get; set; }

        public virtual InventoryOwnership InvetoryOwnership { get; set; }

        [Display(Name = "Barkod")]
        [MaxLength(100, ErrorMessage = "100 Karakterden fazla olamaz")]
        public string StockBarcode { get; set; }

        [Display(Name = "Yetkili Servisi")]
        [ForeignKey("CorporateAccountService")]
        public int? CorporateAccountServiceID { get; set; }

        public virtual CorporateAccount CorporateAccountService { get; set; }

        [Display(Name = "Garanti Bitiş Tarihi")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? WarrantyEnd { get; set; }

        [Display(Name = "Cihaz Alınış Tarihi")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? PurchaseDate { get; set; }
        
        
        
    }
}