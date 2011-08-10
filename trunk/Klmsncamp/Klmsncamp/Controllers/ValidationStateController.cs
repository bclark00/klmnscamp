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
    public class ValidationStateController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /ValidationState/

        public ViewResult Index()
        {
            return View(db.ValidationStates.ToList());
        }

        //
        // GET: /ValidationState/Details/5

        public ViewResult Details(int id)
        {
            ValidationState validationstate = db.ValidationStates.Find(id);
            return View(validationstate);
        }

        //
        // GET: /ValidationState/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /ValidationState/Create

        [HttpPost]
        public ActionResult Create(ValidationState validationstate)
        {
            if (ModelState.IsValid)
            {
                db.ValidationStates.Add(validationstate);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(validationstate);
        }
        
        //
        // GET: /ValidationState/Edit/5
 
        public ActionResult Edit(int id)
        {
            ValidationState validationstate = db.ValidationStates.Find(id);
            return View(validationstate);
        }

        //
        // POST: /ValidationState/Edit/5

        [HttpPost]
        public ActionResult Edit(ValidationState validationstate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(validationstate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(validationstate);
        }

        //
        // GET: /ValidationState/Delete/5
 
        public ActionResult Delete(int id)
        {
            ValidationState validationstate = db.ValidationStates.Find(id);
            return View(validationstate);
        }

        //
        // POST: /ValidationState/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            ValidationState validationstate = db.ValidationStates.Find(id);
            db.ValidationStates.Remove(validationstate);
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