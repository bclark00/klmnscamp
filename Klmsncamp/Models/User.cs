using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;

namespace Klmsncamp.Models
{
    public class User 
    {
       

        public int UserId { get; set; }

        [MaxLength(50,ErrorMessage="50 karakterden uzun olamaz")]
        public string UserName { get; set; }

        [MaxLength(100,ErrorMessage="100 karakterden uzun olamaz")]
        public string FirstName { get; set; }

        [MaxLength(100, ErrorMessage = "100 karakterden uzun olamaz")]
        public string LastName { get; set; }

        [MaxLength(100, ErrorMessage = "100 karakterden uzun olamaz")]
        public string Email { get; set; }

        [MaxLength(128, ErrorMessage = "128 karakterden uzun olamaz")]
        public string Password { get; set; }

        [MaxLength(128, ErrorMessage = "128 karakterden uzun olamaz")]
        public string PasswordSalt { get; set; }

        [MaxLength(350, ErrorMessage = "350 karakterden uzun olamaz")]
        public string Comments { get; set; }

        public DateTime CreatedDate {get;set;}
        public Nullable<DateTime> LastModifiedDate {get;set;}
        public Nullable<DateTime> LastLoginDate { get; set; }

        [MaxLength(50)]
        public string LastLoginIP { get; set; }

        public bool IsActivated { get; set; }
        public bool IsLockedOut { get; set; }

        public Nullable<DateTime> LastLockedOutDate { get; set; }

        [MaxLength(250, ErrorMessage = "250 karakterden uzun olamaz")]
        public string LastLockedOutReason {get;set;}

        [MaxLength(128, ErrorMessage = "128 karakterden uzun olamaz")]
        public string NewPasswordKey {get;set;}

        public Nullable<DateTime> NewPasswordRequested { get; set; }

        [MaxLength(100, ErrorMessage = "100 karakterden uzun olamaz")]
        public string NewEmail { get; set; }

        [MaxLength(128)]
        public string NewEmailKey { get; set; }

        public Nullable<DateTime> NewEmailRequested { get; set; }

        public virtual ICollection<Workshop> Workshops { get; set; }
        public virtual ICollection<CustomPermission> CustomPermissions { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; }
        public virtual ICollection<Role> Roles { get; set; }


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

    }
}