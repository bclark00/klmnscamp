using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Klmsncamp.Models
{
    public class Material
    {
        public int MaterialID { get; set; }

        [MaxLength(150, ErrorMessage = "En çok 150 karakter olabilir")]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        [Display(Name= "Kodu")]
        [MaxLength(20, ErrorMessage = "En çok 20 karakter olabilir")]
        public string MaterialCodeNum { get; set; }

        [Display(Name = "LokasyonID")]
        public int? LocationID { get; set; }

        [Display(Name = "Lokasyon")]
        public virtual Location Location { get; set; }

        [ForeignKey("CorporateAccount")]
        public int? CorporateAccountID { get; set; }

        [Display(Name = "Markası")]
        public virtual CorporateAccount CorporateAccount { get; set; }

        [Display(Name = "Model")]
        [MaxLength(100, ErrorMessage = "100 Karakterden fazla olamaz")]
        public string ComponentModel { get; set; }

        [ForeignKey("ParentMaterial")]
        [Display(Name="Ana Malzeme")]
        public int? ParentMaterialID { get; set; }

        public virtual Material ParentMaterial { get; set; }

        public virtual ICollection<Material> MaterialChilds { get; set; }

        [Display(Name = "Kategori")]
        [Required(ErrorMessage = "Zorunlu Alan")]
        public int MaterialCategoryID { get; set; }

        [Display(Name= "Kategori")]
        public virtual MaterialCategory MaterialCategory { get; set; }

        [Display(Name = "Malzeme Grubu")]
        public int? MaterialGroupID { get; set; }

        [Display(Name = "Malzeme Grubu")]
        public virtual MaterialGroup MaterialGroup { get; set; }

        [Display(Name = "Malzeme Tipi")]
        public int? MaterialTypeID { get; set; }

        [Display(Name = "Malzeme Tipi")]
        public virtual MaterialType MaterialType { get; set; }


        [Display(Name= "Bulunduğu Kutu")]
        [MaxLength(20, ErrorMessage = "En çok 20 karakter olabilir")]
        public string RackLocation { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Durum")]
        public int ValidationStateID { get; set; }
        
        public virtual ValidationState ValidationState { get; set; }

		[MaxLength(120,ErrorMessage="Note Alanı 120 karakteri geçmemelidir.")]
        public string Note { get; set; }

        public virtual ICollection<MaterialFile> MaterialFiles { get; set; }
    }
}