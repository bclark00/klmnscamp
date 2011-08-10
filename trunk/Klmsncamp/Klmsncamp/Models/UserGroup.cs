using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Klmsncamp.Models
{
    public class UserGroup
    {
        public int UserGroupID { get; set; }
        
        [MaxLength(50,ErrorMessage="50 Karakterden Fazla Olamaz")]
        public string Name { get; set; }

        public virtual ICollection<Workshop> Workshops { get; set; }
        public virtual ICollection<CustomPermission> CustomPermissions { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}