using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Klmsncamp.Models
{
    public class WorkshopPermission
    {
        public int WorkshopPermissionID { get; set; }

        [Display(Name = "Atölye")]
        [Required(ErrorMessage = "Zorunlu Alan")]
        public int WorkshopID { get; set; }

        [Display(Name = "İş İstenen Atölye")]
        public virtual Workshop Workshop { get; set; }

        [Display(Name = "Görüntüleme")]
        public bool Select { get; set; }

        [Display(Name = "Kayıt Ekleme")]
        public bool Insert { get; set; }

        [Display(Name = "Güncelleme")]
        public bool Update { get; set; }

        [Display(Name = "Silme")]
        public bool Delete {get;set;}

        [Display(Name = "Kapatma")]
        public bool Approve {get;set;}

        public virtual ICollection<User> Users { get; set; }

    }
}