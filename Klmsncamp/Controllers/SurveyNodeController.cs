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
    public class SurveyNodeController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /SurveyNode/
        [Authorize]
        public ViewResult Index()
        {
            return View(db.SurveyNodes.ToList());
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SurveyNode/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(SurveyNode surveynode)
        {
            if (ModelState.IsValid)
            {
                db.SurveyNodes.Add(surveynode);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(surveynode);
        }

        //
        // GET: /SurveyNode/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            SurveyNode surveynode = db.SurveyNodes.Find(id);
            return View(surveynode);
        }

        //
        // POST: /SurveyNode/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(SurveyNode surveynode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(surveynode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(surveynode);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}