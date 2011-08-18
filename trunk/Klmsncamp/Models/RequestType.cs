using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Web.Mvc.UI;
using Klmsncamp.DAL;
using System.ComponentModel.DataAnnotations;

namespace Klmsncamp.Models
{
    public class RequestType
    {
        public int RequestTypeID { get; set; }

        [MaxLength(50, ErrorMessage = "50 Karakterden fazla olamaz")]
        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Açıklama")]
        public string Description {get;set;}

        public int? ParentRequestTypeId { get; set; }

        public string GetParentDescription(int? parent_id)
        {
            int? parentID = parent_id;
            if (parentID == null)
            {
                return "--";
            }
            else
            {
                KlmsnContext db = new KlmsnContext();
                var item = db.RequestTypes.Find(parentID);
                return item.Description;
            }
        }

    }
}