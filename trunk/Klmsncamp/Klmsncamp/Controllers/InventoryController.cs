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
    public class InventoryController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /Inventory/

        public ViewResult Index()
        {
            var ınventories = db.Inventories.Include(ı => ı.Location).Include(ı => ı.CorporateAccount).Include(ı => ı.ValidationState).Include(ı => ı.InvetoryOwnership).Include(ı => ı.CorporateAccountService);
            return View(ınventories.ToList());
        }

        //
        // GET: /Inventory/Details/5

        public ViewResult Details(int id)
        {
            Inventory ınventory = db.Inventories.Find(id);
            return View(ınventory);
        }

        //
        // GET: /Inventory/Create

        public ActionResult Create()
        {
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description");
            ViewBag.CorporateAccountID = new SelectList(db.CorporateAccounts.Where(e=>e.CorporateTypeID==1), "CorporateAccountID", "Title");
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description");
            ViewBag.InventoryOwnershipID = new SelectList(db.InventoryOwnerships, "InventoryOwnershipID", "Description");
            ViewBag.CorporateAccountServiceID = new SelectList(db.CorporateAccounts.Where(e=>e.CorporateTypeID==2), "CorporateAccountID", "Title");
            return View();
        } 

        //
        // POST: /Inventory/Create

        [HttpPost]
        public ActionResult Create(Inventory ınventory)
        {
            if (ModelState.IsValid)
            {
                db.Inventories.Add(ınventory);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description", ınventory.LocationID);
            ViewBag.CorporateAccountID = new SelectList(db.CorporateAccounts.Where(e => e.CorporateTypeID == 1), "CorporateAccountID", "Title", ınventory.CorporateAccountID);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", ınventory.ValidationStateID);
            ViewBag.InventoryOwnershipID = new SelectList(db.InventoryOwnerships, "InventoryOwnershipID", "Description", ınventory.InventoryOwnershipID);
            ViewBag.CorporateAccountServiceID = new SelectList(db.CorporateAccounts.Where(e => e.CorporateTypeID == 2), "CorporateAccountID", "Title", ınventory.CorporateAccountServiceID);
            return View(ınventory);
        }
        
        //
        // GET: /Inventory/Edit/5
 
        public ActionResult Edit(int id)
        {
            Inventory ınventory = db.Inventories.Find(id);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description", ınventory.LocationID);
            ViewBag.CorporateAccountID = new SelectList(db.CorporateAccounts.Where(e => e.CorporateTypeID == 1), "CorporateAccountID", "Title", ınventory.CorporateAccountID);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", ınventory.ValidationStateID);
            ViewBag.InventoryOwnershipID = new SelectList(db.InventoryOwnerships, "InventoryOwnershipID", "Description", ınventory.InventoryOwnershipID);
            ViewBag.CorporateAccountServiceID = new SelectList(db.CorporateAccounts.Where(e => e.CorporateTypeID == 2), "CorporateAccountID", "Title", ınventory.CorporateAccountServiceID);
            return View(ınventory);
        }

        //
        // POST: /Inventory/Edit/5

        [HttpPost]
        public ActionResult Edit(Inventory ınventory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ınventory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description", ınventory.LocationID);
            ViewBag.CorporateAccountID = new SelectList(db.CorporateAccounts.Where(e => e.CorporateTypeID == 1), "CorporateAccountID", "Title", ınventory.CorporateAccountID);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", ınventory.ValidationStateID);
            ViewBag.InventoryOwnershipID = new SelectList(db.InventoryOwnerships, "InventoryOwnershipID", "Description", ınventory.InventoryOwnershipID);
            ViewBag.CorporateAccountServiceID = new SelectList(db.CorporateAccounts.Where(e => e.CorporateTypeID == 2), "CorporateAccountID", "Title", ınventory.CorporateAccountServiceID);
            return View(ınventory);
        }

        //
        // GET: /Inventory/Delete/5
 
        public ActionResult Delete(int id)
        {
            Inventory ınventory = db.Inventories.Find(id);
            return View(ınventory);
        }

        //
        // POST: /Inventory/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Inventory ınventory = db.Inventories.Find(id);
            db.Inventories.Remove(ınventory);
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