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
    public class RequestStateController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /RequestState/

        public ViewResult Index()
        {
            return View(db.RequestStates.ToList());
        }

        //
        // GET: /RequestState/Details/5

        public ViewResult Details(int id)
        {
            RequestState requeststate = db.RequestStates.Find(id);
            return View(requeststate);
        }

        //
        // GET: /RequestState/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /RequestState/Create

        [HttpPost]
        public ActionResult Create(RequestState requeststate)
        {
            if (ModelState.IsValid)
            {
                db.RequestStates.Add(requeststate);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(requeststate);
        }
        
        //
        // GET: /RequestState/Edit/5
 
        public ActionResult Edit(int id)
        {
            RequestState requeststate = db.RequestStates.Find(id);
            return View(requeststate);
        }

        //
        // POST: /RequestState/Edit/5

        [HttpPost]
        public ActionResult Edit(RequestState requeststate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requeststate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(requeststate);
        }

        //
        // GET: /RequestState/Delete/5
 
        public ActionResult Delete(int id)
        {
            RequestState requeststate = db.RequestStates.Find(id);
            return View(requeststate);
        }

        //
        // POST: /RequestState/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            RequestState requeststate = db.RequestStates.Find(id);
            db.RequestStates.Remove(requeststate);
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