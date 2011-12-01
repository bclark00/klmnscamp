using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Klmsncamp.Models
{
    public class LogRequestIssue
    {
        public int LogRequestIssueID { get; set; }

        [Display(Name = "İş istek")]
        public int RequestIssueID { get; set; }

        public virtual RequestIssue RequestIssue { get; set; }

        [MaxLength(50, ErrorMessage = "50 karakterden fazla olamaz")]
        public string Action { get; set; }

        [Display(Name = "Değiştirilme Zamanı")]
        public DateTime ModifyTime { get; set; }

        [ForeignKey("User")]
        [Display(Name = "Kullanıcı")]
        public int? UserID { get; set; }

        [Display(Name = "Kullanıcı")]
        public virtual User User { get; set; }

        public virtual ICollection<LogRequestIssueDetail> LogRequestIssueDetails { get; set; }
    }
}