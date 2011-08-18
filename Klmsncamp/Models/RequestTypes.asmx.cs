using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Telerik.Web.Mvc.UI;

namespace Klmsncamp.Models
{
    /// <summary>
    /// Summary description for RequestTypes
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class RequestTypes : System.Web.Services.WebService
    {

        [WebMethod]
        public IEnumerable<TreeViewItem> GetEmployees(TreeViewItem node)
        {

            var db = new KlmsnContext();
            int? parentId = !string.IsNullOrEmpty(node.Value) ? (int?)Convert.ToInt32(node.Value) : null;

            IEnumerable<TreeViewItem> nodes = from item in db.RequestTypes
                                where item.ParentRequestTypeId == parentId || (parentId == null)
                                select new TreeViewItem
                                {
                                    Text = item.Description,
                                    Value = item.RequestTypeID.ToString(),
                                    Enabled = true
                                    //LoadOnDemand = item.Employees.Count > 0
                                };

            return nodes;
        }
    }
}
