using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Klmsncamp.Models
{
    public class LogRequestIssueDetail
    {
        public int LogRequestIssueDetailID { get; set; }

        public int LogRequestIssueID { get; set; }

        public virtual LogRequestIssue LogRequestIssue { get; set; }

        [MaxLength(1000, ErrorMessage = "1000 karakterden fazla olamaz")]
        public string PropertyName { get; set; }

        [MaxLength(1000, ErrorMessage = "1000 karakterden fazla olamaz")]
        public string PropertyOldValue { get; set; }

        [MaxLength(1000, ErrorMessage = "1000 karakterden fazla olamaz")]
        public string PropertyNewValue { get; set; }
    }
}