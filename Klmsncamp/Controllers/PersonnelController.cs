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
    public class PersonnelController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /Personnel/

        public ViewResult Index()
        {
            var personnels = db.Personnels.Include(p => p.Location).Include(p => p.ValidationState);
            return View(personnels.ToList());
        }

        //
        // GET: /Personnel/Details/5

        public ViewResult Details(int id)
        {
            Personnel personnel = db.Personnels.Find(id);
            return View(personnel);
        }

        //
        // GET: /Personnel/Create

        public ActionResult Create()
        {
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description");
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description");
            return View();
        } 

        //
        // POST: /Personnel/Create

        [HttpPost]
        public ActionResult Create(Personnel personnel)
        {
            if (ModelState.IsValid)
            {
                db.Personnels.Add(personnel);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description", personnel.LocationID);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", personnel.ValidationStateID);
            return View(personnel);
        }
        
        //
        // GET: /Personnel/Edit/5
 
        public ActionResult Edit(int id)
        {
            Personnel personnel = db.Personnels.Find(id);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description", personnel.LocationID);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", personnel.ValidationStateID);
            return View(personnel);
        }

        //
        // POST: /Personnel/Edit/5

        [HttpPost]
        public ActionResult Edit(Personnel personnel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personnel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description", personnel.LocationID);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", personnel.ValidationStateID);
            return View(personnel);
        }

        //
        // GET: /Personnel/Delete/5
 
        public ActionResult Delete(int id)
        {
            Personnel personnel = db.Personnels.Find(id);
            return View(personnel);
        }

        //
        // POST: /Personnel/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Personnel personnel = db.Personnels.Find(id);
            db.Personnels.Remove(personnel);
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