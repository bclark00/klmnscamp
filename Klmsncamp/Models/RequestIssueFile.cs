using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Klmsncamp.Models
{
    public class RequestIssueFile
    {
        public int RequestIssueFileID { get; set; }

        public int RequestIssueID { get; set; }

        public virtual RequestIssue RequestIssue { get; set; }

        public string RequestIssueFileDescription { get; set; }

        public int RequestIssueFileSize { get; set; }

        public string RequestIssueFileName { get; set; }

        public string RequestIssueFileContentType { get; set; }

        public byte[] RequestIssueFileContents { get; set; }
    }
}