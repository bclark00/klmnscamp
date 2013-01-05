using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Klmsncamp.Models
{
    public class MaterialFile
    {
        public int MaterialFileID { get; set; }

        public int MaterialID { get; set; }

        public virtual Material Material { get; set; }

        public string MaterialFileDescription { get; set; }

        public int MaterialFileSize { get; set; }

        public string MaterialFileName { get; set; }

        public string MaterialFileContentType { get; set; }

        public byte[] MaterialFileContents { get; set; }
    }
}