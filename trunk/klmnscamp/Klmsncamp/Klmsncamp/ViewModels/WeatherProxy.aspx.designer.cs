using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Collections.Generic;
using System.Net;
using System.IO;

namespace Klmsncamp.ViewModels
{   
    public class Weather
    {
        public string city;
        public int curr_temp;
        public string curr_text;
        public int curr_icon;

        public List<Forecast> forecast = new List<Forecast>();

        
    }

    public class Forecast
    {
        public string day_date;
        public string day_text;
        public int day_icon;
        public int day_htemp;
        public int day_ltemp;
    }

    public class WJson
    {
        public string Json;

    }
}
