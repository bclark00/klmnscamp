using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Klmsncamp.DAL;
using Klmsncamp.Models;

namespace Klmsncamp.ViewModels
{
    public class _requestTypes
    {
        public string Text;
        public string Value;
        public bool Expanded;
        public List<_requestTypes> Items = new List<_requestTypes>();


    }

    public class _requestView
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public string ParentValue { get; set; }
        public string ParentText { get; set; }
        public List<_requestView> _requestViews = new List<_requestView>();

        public _requestView()
        {
            this._requestViews = new List<_requestView>();
        }

       
    }

    public class RequestTypeEditViewModel
    {
        
        public int RequestTypeID { get; set; }
        public string  Description { get; set; }
        public int RequestTypeParentID { get; set; }
     
    }

   /* public class RequestTypeViewModel
    {
        public class RequestTypeLeaf
        {
            
           
            public string Description { get; set; }
            public Guid RequestTypeId { get; set; }
            public bool IsLast { get; set; }
            public List<RequestTypeLeaf> SubRequestTypes = new List<RequestTypeLeaf>();

            public RequestTypeLeaf()
            {
                this.SubRequestTypes = new List<RequestTypeLeaf>();
            }
        }

        public class RequestTypeBranch 
        {
            public List<RequestTypeLeaf> RequestTypes = new List<RequestTypeLeaf>();

            public RequestTypeBranch()
            {
                this.RequestTypes = new List<RequestTypeLeaf>();
            }
        }

        public List<RequestTypeBranch> requestTypeBranchs = new List<RequestTypeBranch>();

        public RequestTypeViewModel()
        {
            this.requestTypeBranchs = new List<RequestTypeBranch>();
        }
    }*/
  
}