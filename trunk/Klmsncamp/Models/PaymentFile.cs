using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Klmsncamp.Models
{
    public class PaymentFile
    {
        public int PaymentFileID { get; set; }

        public int PaymentID { get; set; }

        public virtual Payment Payment { get; set; }

        public string PaymentFileDescription { get; set; }

        public int PaymentFileSize { get; set; }

        public string PaymentFileName { get; set; }

        public string PaymentFileContentType { get; set; }

        public byte[] PaymentFileContents { get; set; }
    }
}