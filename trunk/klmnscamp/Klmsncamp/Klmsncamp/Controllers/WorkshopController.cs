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
    public class WorkshopController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /Workshop/

        public ViewResult Index()
        {
            var workshops = db.Workshops.Include(w => w.ValidationState);
            return View(workshops.ToList());
        }

        //
        // GET: /Workshop/Details/5

        public ViewResult Details(int id)
        {
            Workshop workshop = db.Workshops.Find(id);
            return View(workshop);
        }

        //
        // GET: /Workshop/Create

        public ActionResult Create()
        {
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description");
            return View();
        } 

        //
        // POST: /Workshop/Create

        [HttpPost]
        public ActionResult Create(Workshop workshop)
        {
            if (ModelState.IsValid)
            {
                db.Workshops.Add(workshop);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", workshop.ValidationStateID);
            return View(workshop);
        }
        
        //
        // GET: /Workshop/Edit/5
 
        public ActionResult Edit(int id)
        {
            Workshop workshop = db.Workshops.Find(id);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", workshop.ValidationStateID);
            return View(workshop);
        }

        //
        // POST: /Workshop/Edit/5

        [HttpPost]
        public ActionResult Edit(Workshop workshop)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workshop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", workshop.ValidationStateID);
            return View(workshop);
        }

        //
        // GET: /Workshop/Delete/5
 
        public ActionResult Delete(int id)
        {
            Workshop workshop = db.Workshops.Find(id);
            return View(workshop);
        }

        //
        // POST: /Workshop/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Workshop workshop = db.Workshops.Find(id);
            db.Workshops.Remove(workshop);
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