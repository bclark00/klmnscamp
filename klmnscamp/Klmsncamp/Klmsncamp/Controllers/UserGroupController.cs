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
    public class UserGroupController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /UserGroup/

        public ViewResult Index()
        {
            return View(db.UserGroups.ToList());
        }

        //
        // GET: /UserGroup/Details/5

        public ViewResult Details(int id)
        {
            UserGroup usergroup = db.UserGroups.Find(id);
            return View(usergroup);
        }

        //
        // GET: /UserGroup/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /UserGroup/Create

        [HttpPost]
        public ActionResult Create(UserGroup usergroup)
        {
            if (ModelState.IsValid)
            {
                db.UserGroups.Add(usergroup);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(usergroup);
        }
        
        //
        // GET: /UserGroup/Edit/5
 
        public ActionResult Edit(int id)
        {
            UserGroup usergroup = db.UserGroups.Find(id);
            return View(usergroup);
        }

        //
        // POST: /UserGroup/Edit/5

        [HttpPost]
        public ActionResult Edit(UserGroup usergroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usergroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usergroup);
        }

        //
        // GET: /UserGroup/Delete/5
 
        public ActionResult Delete(int id)
        {
            UserGroup usergroup = db.UserGroups.Find(id);
            return View(usergroup);
        }

        //
        // POST: /UserGroup/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            UserGroup usergroup = db.UserGroups.Find(id);
            db.UserGroups.Remove(usergroup);
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