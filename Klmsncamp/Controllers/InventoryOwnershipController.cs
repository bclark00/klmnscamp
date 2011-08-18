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
    public class InventoryOwnershipController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /Default1/

        public ViewResult Index()
        {
            return View(db.InventoryOwnerships.ToList());
        }

        //
        // GET: /Default1/Details/5

        public ViewResult Details(int id)
        {
            InventoryOwnership ınventoryownership = db.InventoryOwnerships.Find(id);
            return View(ınventoryownership);
        }

        //
        // GET: /Default1/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Default1/Create

        [HttpPost]
        public ActionResult Create(InventoryOwnership ınventoryownership)
        {
            if (ModelState.IsValid)
            {
                db.InventoryOwnerships.Add(ınventoryownership);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(ınventoryownership);
        }
        
        //
        // GET: /Default1/Edit/5
 
        public ActionResult Edit(int id)
        {
            InventoryOwnership ınventoryownership = db.InventoryOwnerships.Find(id);
            return View(ınventoryownership);
        }

        //
        // POST: /Default1/Edit/5

        [HttpPost]
        public ActionResult Edit(InventoryOwnership ınventoryownership)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ınventoryownership).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ınventoryownership);
        }

        //
        // GET: /Default1/Delete/5
 
        public ActionResult Delete(int id)
        {
            InventoryOwnership ınventoryownership = db.InventoryOwnerships.Find(id);
            return View(ınventoryownership);
        }

        //
        // POST: /Default1/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            InventoryOwnership ınventoryownership = db.InventoryOwnerships.Find(id);
            db.InventoryOwnerships.Remove(ınventoryownership);
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