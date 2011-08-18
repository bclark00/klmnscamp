using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Klmsncamp.Models;
using Klmsncamp.ViewModels;

namespace Klmsncamp.Controllers
{ 
    public class CorporateAccountController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /CorporateAccount/

        public ViewResult Index()
        {
            var corporateaccounts = db.CorporateAccounts.Include(c => c.CorporateType).Include(c => c.User).Include(c => c.ValidationState);
            return View(corporateaccounts.ToList());
        }

        //
        // GET: /CorporateAccount/Details/5

        public ViewResult Details(int id)
        {
            CorporateAccount corporateaccount = db.CorporateAccounts.Find(id);
            return View(corporateaccount);
        }

        //
        // GET: /CorporateAccount/Create

        public ActionResult Create()
        {
            ViewBag.CorporateTypeID = new SelectList(db.CorporateTypes, "CorporateTypeID", "Description");
            ViewBag.UserID = new SelectList(db.Users, "UserId", "UserName");
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description");
            return View();
        } 

        //
        // POST: /CorporateAccount/Create

        [HttpPost]
        public ActionResult Create(CorporateAccount corporateaccount)
        {
            if (ModelState.IsValid)
            {
                db.CorporateAccounts.Add(corporateaccount);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.CorporateTypeID = new SelectList(db.CorporateTypes, "CorporateTypeID", "Description", corporateaccount.CorporateTypeID);
            ViewBag.UserID = new SelectList(db.Users, "UserId", "UserName", corporateaccount.UserID);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", corporateaccount.ValidationStateID);
            return View(corporateaccount);
        }
        
        //
        // GET: /CorporateAccount/Edit/5
 
        public ActionResult Edit(int id)
        {
            CorporateAccount corporateaccount = db.CorporateAccounts.Find(id);
            ViewBag.CorporateTypeID = new SelectList(db.CorporateTypes, "CorporateTypeID", "Description", corporateaccount.CorporateTypeID);
            ViewBag.UserID = new SelectList(db.Users, "UserId", "UserName", corporateaccount.UserID);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", corporateaccount.ValidationStateID);
            return View(corporateaccount);
        }

        //
        // POST: /CorporateAccount/Edit/5

        [HttpPost]
        public ActionResult Edit(CorporateAccount corporateaccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(corporateaccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CorporateTypeID = new SelectList(db.CorporateTypes, "CorporateTypeID", "Description", corporateaccount.CorporateTypeID);
            ViewBag.UserID = new SelectList(db.Users, "UserId", "UserName", corporateaccount.UserID);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", corporateaccount.ValidationStateID);
            return View(corporateaccount);
        }

        [HttpGet]
        public ActionResult FillContent()
        {

            try
            {
                CorporateAccount _corp = new CorporateAccount();
                string querystring = Request.Params[0];
                _corp = db.CorporateAccounts.Where(e => e.Title == querystring).Single();

                CorporateAccountViewModel _cview = new CorporateAccountViewModel
                {
                    Title = _corp.Title,
                    Address = _corp.Address,
                    Phone1 = _corp.Phone1,
                    Phone2 = _corp.Phone2,
                    ContactPerson = _corp.ContactPerson,
                    CorpEmail = _corp.CorpEmail,
                    CorporateTypeID = _corp.CorporateTypeID.ToString(),
                    UserID = _corp.UserID.ToString(),
                    ValidationStateID = _corp.ValidationStateID.ToString(),
                    is_ok = "ok"
                };




                return Json(_cview, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                CorporateAccountViewModel _cview = new CorporateAccountViewModel
                {
                    is_ok = "false"
                };
                return Json(_cview, JsonRequestBehavior.AllowGet);
            }

           
        }

        //
        // GET: /CorporateAccount/Delete/5
 
        public ActionResult Delete(int id)
        {
            CorporateAccount corporateaccount = db.CorporateAccounts.Find(id);
            return View(corporateaccount);
        }

        //
        // POST: /CorporateAccount/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            CorporateAccount corporateaccount = db.CorporateAccounts.Find(id);
            db.CorporateAccounts.Remove(corporateaccount);
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