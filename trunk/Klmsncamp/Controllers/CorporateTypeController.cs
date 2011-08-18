using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Klmsncamp.Models;

namespace Klmsncamp.Controllers
{ 
    public class CorporateTypeController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /CorporateType/

        public ViewResult Index()
        {
            return View(db.CorporateTypes.ToList());
        }

        //
        // GET: /CorporateType/Details/5

        public ViewResult Details(int id)
        {
            CorporateType corporatetype = db.CorporateTypes.Find(id);
            return View(corporatetype);
        }

        //
        // GET: /CorporateType/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /CorporateType/Create

        [HttpPost]
        public ActionResult Create(CorporateType corporatetype)
        {
            if (ModelState.IsValid)
            {
                db.CorporateTypes.Add(corporatetype);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(corporatetype);
        }
        
        //
        // GET: /CorporateType/Edit/5
 
        public ActionResult Edit(int id)
        {
            CorporateType corporatetype = db.CorporateTypes.Find(id);
            return View(corporatetype);
        }

        //
        // POST: /CorporateType/Edit/5

        [HttpPost]
        public ActionResult Edit(CorporateType corporatetype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(corporatetype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(corporatetype);
        }

        //
        // GET: /CorporateType/Delete/5
 
        public ActionResult Delete(int id)
        {
            CorporateType corporatetype = db.CorporateTypes.Find(id);
            return View(corporatetype);
        }

        //
        // POST: /CorporateType/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            CorporateType corporatetype = db.CorporateTypes.Find(id);
            db.CorporateTypes.Remove(corporatetype);
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