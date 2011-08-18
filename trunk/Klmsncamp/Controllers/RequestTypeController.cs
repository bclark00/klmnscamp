using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Klmsncamp.Models;
using Klmsncamp.ViewModels;
using Telerik.Web.Mvc.UI;

namespace Klmsncamp.Controllers
{ 
    public class RequestTypeController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /RequestType/

        public ViewResult Index()
        {
            var rts = db.RequestTypes.ToList();
            _requestView _rView = new _requestView();
            foreach (var rt in rts)
            {

                _rView._requestViews.Add(new _requestView
                {
                    Text = rt.Description,
                    Value = rt.RequestTypeID.ToString(),
                    ParentText = rt.GetParentDescription(rt.ParentRequestTypeId),
                    ParentValue = rt.ParentRequestTypeId.ToString()
                }
                          );
            }

                    
            return View(_rView._requestViews); 
        }

        public ActionResult GetJsonTree()
        {
            _requestTypes _requests = new _requestTypes();
            
            var rt_list = db.RequestTypes.Where(e => e.ParentRequestTypeId == null).ToList();
            
            
            foreach (var item in rt_list)
            {
                _requests.Items.Add(olustur(item.RequestTypeID));
            }

            return Json(_requests,JsonRequestBehavior.AllowGet);
        }

        public _requestTypes olustur(int rt_id)
        {
            var item = db.RequestTypes.Find(rt_id);
            _requestTypes _requests = new _requestTypes();

             _requests.Text = item.Description;
             _requests.Value = item.RequestTypeID.ToString();
             _requests.Expanded = true;

             if (db.RequestTypes.Where(e => e.ParentRequestTypeId == rt_id).Count() > 0)
             {
                 var sbz = db.RequestTypes.Where(e => e.ParentRequestTypeId == rt_id).ToList();
                 foreach (var it in sbz)
                 {
                     _requests.Items.Add(olustur(it.RequestTypeID));
                 }

             }

             return _requests;
                
            
        }



        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult _Index(TreeViewItem node)
        //{
        //    int? parentId = !string.IsNullOrEmpty(node.Value) ? (int?)Convert.ToInt32(node.Value) : null;
        //    if (parentId == null)
        //        return null;
        //    var rts = db.RequestTypes.Where(e => e.ParentRequestTypeId == parentId);
        //    var nodes = rts.Select(x =>
        //                new TreeViewItem
        //                {
        //                    Text = x.Description,
        //                    Value = x.RequestTypeID.ToString(),
        //                    //LoadOnDemand = false,
        //                    Expanded = true
        //                });

        //    return new JsonResult { Data = nodes };

        //} 
        
        //
        // GET: /RequestType/Details/5

        public ViewResult Details(int id)
        {
            RequestType requesttype = db.RequestTypes.Find(id);
            return View(requesttype);
        }

        //
        // GET: /RequestType/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /RequestType/Create

        [HttpPost]
        public ActionResult Create(RequestType requesttype)
        {
            if (ModelState.IsValid)
            {
                db.RequestTypes.Add(requesttype);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(requesttype);
        }
        
        //
        // GET: /RequestType/Edit/5
 
        public ActionResult Edit(int id)
        {
            //RequestType requesttype = db.RequestTypes.Find(id);
            RequestType rt = db.RequestTypes.Where(i => i.RequestTypeID== id).Single();
            PopulateRTData(rt,id);
            return View(rt);
        }

        private void PopulateRTData(RequestType instructor,int id)
        {
            var allrequesttypes = db.RequestTypes.Where(i=>i.RequestTypeID!=id);
            
            var viewModel = new List<RequestTypeEditViewModel>();

            foreach (var allrt in allrequesttypes)
            {
                viewModel.Add(new RequestTypeEditViewModel
                {
                    RequestTypeID = allrt.RequestTypeID,
                    Description = allrt.Description,
                    RequestTypeParentID =  instructor.ParentRequestTypeId.GetValueOrDefault()
                
                });
            }
            ViewBag.ALLRT = viewModel;
        }
        //
        // POST: /RequestType/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection formCollection, string selectedParent)
        {
            var rtToUpdate = db.RequestTypes.Where(i => i.RequestTypeID == id).Single();

            if (ModelState.IsValid)
            {
                rtToUpdate.Description = formCollection["Description"];
                try
                {
                    rtToUpdate.ParentRequestTypeId = int.Parse(formCollection["selectedParent"]);
                }
                catch
                {
                    rtToUpdate.ParentRequestTypeId = null;
                }

                db.Entry(rtToUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            

            return View(rtToUpdate);
        }

        //
        // GET: /RequestType/Delete/5
 
        public ActionResult Delete(int id)
        {
            RequestType requesttype = db.RequestTypes.Find(id);
            return View(requesttype);
        }

        //
        // POST: /RequestType/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            RequestType requesttype = db.RequestTypes.Find(id);
            db.RequestTypes.Remove(requesttype);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}