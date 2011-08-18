using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Klmsncamp.ViewModels
{
    public class RequestIssueCalendarViewModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string url { get; set; }
        public bool allDay { get; set; }

    }
    
}