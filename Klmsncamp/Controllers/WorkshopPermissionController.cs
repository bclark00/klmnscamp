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
    public class WorkshopPermissionController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /WorkshopPermission/

        public ViewResult Index()
        {
            var workshoppermissions = db.WorkshopPermissions.Include(w => w.Workshop);
            return View(workshoppermissions.ToList());
        }

        //
        // GET: /WorkshopPermission/Details/5

        public ViewResult Details(int id)
        {
            WorkshopPermission workshoppermission = db.WorkshopPermissions.Find(id);
            return View(workshoppermission);
        }

        //
        // GET: /WorkshopPermission/Create

        public ActionResult Create()
        {
            ViewBag.WorkshopID = new SelectList(db.Workshops, "WorkshopID", "Description");
            return View();
        } 

        //
        // POST: /WorkshopPermission/Create

        [HttpPost]
        [Authorize]
        public ActionResult Create(WorkshopPermission workshoppermission,int userID,string RedirectRoute)
        {
            if (ModelState.IsValid)
            {
                

                User user_ = db.Users.Find(userID);
                bool useralreadyhaswrp = false;
                foreach (var wrp in user_.WorkshopPermissions.ToList())
                {
                    if (wrp.WorkshopID == workshoppermission.WorkshopID)
                    {
                        wrp.Select = workshoppermission.Select;
                        wrp.Insert = workshoppermission.Insert;
                        wrp.Update = workshoppermission.Update;
                        wrp.Delete = workshoppermission.Delete;
                        wrp.Approve = workshoppermission.Approve;
                        useralreadyhaswrp = true;
                        db.SaveChanges();
                    }

                }
                

                if (!useralreadyhaswrp)
                {
                    try
                    {
                        db.WorkshopPermissions.Add(workshoppermission);
                        db.SaveChanges();
                    }
                    catch (Exception exx)
                    {
                        User xuser_ = db.Users.Find(userID);
                        return RedirectToAction("Edit", "Account", new { username_ = xuser_.UserName, err = exx.Message });
                    }
                    user_.WorkshopPermissions.Add(workshoppermission);
                    db.SaveChanges();
                }
                

                if (RedirectRoute == "Profil")
                {
                    return RedirectToAction("Edit", "Account", new { username_ = user_.UserName ,err=string.Empty});
                }
            }

            User xxuser_ = db.Users.Find(userID);
            return RedirectToAction("Edit", "Account", new { username_ = xxuser_.UserName, err = "Ana Birim Seçmediniz.." });
        }
        
        //
        // GET: /WorkshopPermission/Edit/5
 
        public ActionResult Edit(int id)
        {
            WorkshopPermission workshoppermission = db.WorkshopPermissions.Find(id);
            ViewBag.WorkshopID = new SelectList(db.Workshops, "WorkshopID", "Description", workshoppermission.WorkshopID);
            return View(workshoppermission);
        }

        //
        // POST: /WorkshopPermission/Edit/5

        [HttpPost]
        public ActionResult Edit(WorkshopPermission workshoppermission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workshoppermission).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WorkshopID = new SelectList(db.Workshops, "WorkshopID", "Description", workshoppermission.WorkshopID);
            return View(workshoppermission);
        }

        //
        // GET: /WorkshopPermission/Delete/5
        [Authorize]
        public ActionResult Delete(int id,int userID,string RedirectRoute)
        {
            WorkshopPermission workshoppermission = db.WorkshopPermissions.Find(id);
            db.WorkshopPermissions.Remove(workshoppermission);
            db.SaveChanges();
            if (RedirectRoute == "Profil")
            {
                User user_ = db.Users.Find(userID);
                return RedirectToAction("Edit", "Account", new { username_ = user_.UserName, err = string.Empty });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}