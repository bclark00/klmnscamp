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
    public class MaterialCategoryController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /MaterialCategory/

        public ViewResult Index()
        {
            var materialcategories = db.MaterialCategories.Include(m => m.ParentMaterialCategory).Include(m=>m.MaterialCategoryChilds).Include(m => m.ValidationState).Where(m=>m.ParentMaterialCategoryID==0 || m.ParentMaterialCategoryID==null);
            return View(materialcategories.ToList());
        }

        //
        // GET: /MaterialCategory/Details/5

        public ViewResult Details(int id)
        {
            MaterialCategory materialcategory = db.MaterialCategories.Find(id);
            return View(materialcategory);
        }

        //
        // GET: /MaterialCategory/Create

        public ActionResult Create()
        {
            ViewBag.ParentMaterialCategoryID = new SelectList(db.MaterialCategories, "MaterialCategoryID", "Description");
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description");
            return View();
        } 

        //
        // POST: /MaterialCategory/Create

        [HttpPost]
        public ActionResult Create(MaterialCategory materialcategory)
        {
            if (ModelState.IsValid)
            {
                db.MaterialCategories.Add(materialcategory);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.ParentMaterialCategoryID = new SelectList(db.MaterialCategories, "MaterialCategoryID", "Description", materialcategory.ParentMaterialCategoryID);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", materialcategory.ValidationStateID);
            return View(materialcategory);
        }
        
        //
        // GET: /MaterialCategory/Edit/5
 
        public ActionResult Edit(int id)
        {
            MaterialCategory materialcategory = db.MaterialCategories.Find(id);
            ViewBag.ParentMaterialCategoryID = new SelectList(db.MaterialCategories, "MaterialCategoryID", "Description", materialcategory.ParentMaterialCategoryID);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", materialcategory.ValidationStateID);
            return View(materialcategory);
        }

        //
        // POST: /MaterialCategory/Edit/5

        [HttpPost]
        public ActionResult Edit(MaterialCategory materialcategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(materialcategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentMaterialCategoryID = new SelectList(db.MaterialCategories, "MaterialCategoryID", "Description", materialcategory.ParentMaterialCategoryID);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", materialcategory.ValidationStateID);
            return View(materialcategory);
        }

        //
        // GET: /MaterialCategory/Delete/5
 
        public ActionResult Delete(int id)
        {
            MaterialCategory materialcategory = db.MaterialCategories.Find(id);
            return View(materialcategory);
        }

        //
        // POST: /MaterialCategory/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            MaterialCategory materialcategory = db.MaterialCategories.Find(id);
            db.MaterialCategories.Remove(materialcategory);
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