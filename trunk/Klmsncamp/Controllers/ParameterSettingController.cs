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
    public class ParameterSettingController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /ParameterSetting/

        public ViewResult Index()
        {
            return View(db.ParameterSettings.ToList());
        }

        //
        // GET: /ParameterSetting/Details/5

        public ViewResult Details(int id)
        {
            ParameterSetting parametersetting = db.ParameterSettings.Find(id);
            return View(parametersetting);
        }

        //
        // GET: /ParameterSetting/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /ParameterSetting/Create

        [HttpPost]
        public ActionResult Create(ParameterSetting parametersetting)
        {
            if (ModelState.IsValid)
            {
                db.ParameterSettings.Add(parametersetting);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(parametersetting);
        }
        
        //
        // GET: /ParameterSetting/Edit/5
 
        public ActionResult Edit(int id)
        {
            ParameterSetting parametersetting = db.ParameterSettings.Find(id);
            return View(parametersetting);
        }

        //
        // POST: /ParameterSetting/Edit/5

        [HttpPost]
        public ActionResult Edit(ParameterSetting parametersetting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parametersetting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parametersetting);
        }

        //
        // GET: /ParameterSetting/Delete/5
 
        public ActionResult Delete(int id)
        {
            ParameterSetting parametersetting = db.ParameterSettings.Find(id);
            return View(parametersetting);
        }

        //
        // POST: /ParameterSetting/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            ParameterSetting parametersetting = db.ParameterSettings.Find(id);
            db.ParameterSettings.Remove(parametersetting);
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