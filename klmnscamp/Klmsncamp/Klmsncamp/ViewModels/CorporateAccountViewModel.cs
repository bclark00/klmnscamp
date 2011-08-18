using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Klmsncamp.DAL;
using Klmsncamp.Models;

namespace Klmsncamp.ViewModels
{
    public class CorporateAccountViewModel
    {
        public string Title { get; set; }
        public string Address { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public string ContactPerson { get; set; }

        public string CorpEmail { get; set; }

        public string CorporateTypeID { get; set; }
       
        public string UserID { get; set; }
        
        public string ValidationStateID { get; set; }

        public string is_ok { get; set; }
    }
}