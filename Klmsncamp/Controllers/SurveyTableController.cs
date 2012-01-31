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
    public class SurveyTableController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /SurveyTable/

        public ViewResult Index()
        {
            var surveytables = db.SurveyTables.Include(s => s.SurveyTemplate).Include(s => s.RequestIssue);
            return View(surveytables.ToList());
        }

        //
        // GET: /SurveyTable/Details/5

        public ViewResult Details(int id)
        {
            SurveyTable surveytable = db.SurveyTables.Find(id);
            return View(surveytable);
        }

        //
        // GET: /SurveyTable/Create

        public ActionResult Create()
        {
            ViewBag.SurveyTemplateID = new SelectList(db.SurveyTemplates, "SurveyTemplateID", "Description");
            ViewBag.RequestIssueID = new SelectList(db.RequestIssues, "RequestIssueID", "DetailedDescription");
            return View();
        }

        //
        // POST: /SurveyTable/Create

        [HttpPost]
        public ActionResult Create(SurveyTable surveytable)
        {
            if (ModelState.IsValid)
            {
                db.SurveyTables.Add(surveytable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SurveyTemplateID = new SelectList(db.SurveyTemplates, "SurveyTemplateID", "Description", surveytable.SurveyTemplateID);
            ViewBag.RequestIssueID = new SelectList(db.RequestIssues, "RequestIssueID", "DetailedDescription", surveytable.RequestIssueID);
            return View(surveytable);
        }

        //
        // GET: /SurveyTable/Edit/5

        public ActionResult Edit(int id)
        {
            SurveyTable surveytable = db.SurveyTables.Find(id);
            ViewBag.SurveyTemplateID = new SelectList(db.SurveyTemplates, "SurveyTemplateID", "Description", surveytable.SurveyTemplateID);
            ViewBag.RequestIssueID = new SelectList(db.RequestIssues, "RequestIssueID", "DetailedDescription", surveytable.RequestIssueID);

            ViewBag.TheseSurveyRecords = surveytable.SurveyTemplate.SurveyRecords.ToList();

            return View(surveytable);
        }

        //
        // POST: /SurveyTable/Edit/5

        [HttpPost]
        public ActionResult Edit(SurveyTable surveytable, FormCollection formcollection)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(surveytable).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SurveyTemplateID = new SelectList(db.SurveyTemplates, "SurveyTemplateID", "Description", surveytable.SurveyTemplateID);
            ViewBag.RequestIssueID = new SelectList(db.RequestIssues, "RequestIssueID", "DetailedDescription", surveytable.RequestIssueID);
            return View(surveytable);
        }

        //
        // GET: /SurveyTable/Delete/5

        public ActionResult Delete(int id)
        {
            SurveyTable surveytable = db.SurveyTables.Find(id);
            return View(surveytable);
        }

        //
        // POST: /SurveyTable/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            SurveyTable surveytable = db.SurveyTables.Find(id);
            db.SurveyTables.Remove(surveytable);
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