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
        [Authorize]
        public ViewResult Index()
        {
            return View(db.ParameterSettings.ToList());
        }

        //
        // GET: /ParameterSetting/Details/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            ParameterSetting parametersetting = db.ParameterSettings.Find(id);
            return View(parametersetting);
        }

        //
        // POST: /ParameterSetting/Edit/5
        [Authorize]
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}