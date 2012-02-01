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

        [Authorize]
        public ActionResult Edit(int id, string customerr)
        {
            SurveyTable surveytable = db.SurveyTables.Find(id);
            ViewBag.SurveyTemplateID = new SelectList(db.SurveyTemplates, "SurveyTemplateID", "Description", surveytable.SurveyTemplateID);
            ViewBag.RequestIssueID = new SelectList(db.RequestIssues, "RequestIssueID", "DetailedDescription", surveytable.RequestIssueID);

            ViewBag.TheseSurveyRecords = surveytable.SurveyTemplate.SurveyRecords.ToList();
            if (!(string.IsNullOrEmpty(customerr)))
            {
                ViewBag.CustomErr = customerr;
            }

            if (surveytable.IsApproved)
            {
                ViewBag.CustomErr = "Bu Anket İş Talep sahibi tarafından doldurularak tamamlanmıştır. İlginize Teşekkürler..";
            }
            return View(surveytable);
        }

        //
        // POST: /SurveyTable/Edit/5

        [HttpPost]
        [Authorize]
        public ActionResult Edit(SurveyTable surveytable, FormCollection formcollection)
        {
            if (ModelState.IsValid)
            {
                //db.SaveChanges();
                if (surveytable.IsApproved)
                {
                    return RedirectToAction("Edit", new { id = surveytable.SurveyTableID });
                }

                if (DateTime.Now > surveytable.TimeStamp.AddDays(1))
                {
                    return RedirectToAction("Edit", new { id = surveytable.SurveyTableID, customerr = "Üzgünüz, bu anketin geçerlilik süresi dolmuştur." });
                }

                try
                {
                    string hashconfirm = formcollection["HashkeyConfirm"];
                    var mysvtable_ = db.SurveyTables.AsNoTracking().Where(i => i.SurveyTableID == surveytable.SurveyTableID).SingleOrDefault();

                    if (mysvtable_.HashKey.Equals(hashconfirm))
                    {
                        var mysvtemplate_ = db.SurveyTemplates.Where(i => i.SurveyTemplateID == surveytable.SurveyTemplateID).Include(p => p.SurveyRecords).SingleOrDefault();
                        foreach (SurveyRecord mysurvrecord in mysvtemplate_.SurveyRecords.ToList())
                        {
                            if (mysurvrecord.SurveyRecordTypeID == 1)
                            {
                                mysurvrecord.Score = int.Parse(formcollection[mysurvrecord.SurveyRecordID.ToString() + "_Score"]);
                            }
                            else if (mysurvrecord.SurveyRecordTypeID == 2)
                            {
                                mysurvrecord.ApprovalStatus = bool.Parse(formcollection[mysurvrecord.SurveyRecordID.ToString() + "_ApprovalStatus"].Split(',')[0]);
                            }

                            mysurvrecord.Note = formcollection[mysurvrecord.SurveyRecordID.ToString() + "_Note"];
                        }
                        db.SaveChanges();
                        db.Entry(surveytable).State = EntityState.Modified;
                        surveytable.HashKey = hashconfirm;
                        surveytable.IsApproved = true;
                        surveytable.mTimeStamp = DateTime.Now;
                        db.SaveChanges();
                    }
                    else
                    {
                        return RedirectToAction("Edit", new { id = surveytable.SurveyTableID, customerr = "Şifre Uyumsuz. Her anket için farklı şifre üretilmektedir, lütfen elinizdeki şifrenin doğruluğunu kontrol edin." });
                    }
                }
                catch (Exception exx)
                {
                    return RedirectToAction("Edit", new { id = surveytable.SurveyTableID, customerr = "Üzgünüz bir hata oldu.(Detaylar :" + exx.Message + ")" });
                }
                return RedirectToAction("Edit", new { id = surveytable.SurveyTableID });
            }
            ViewBag.SurveyTemplateID = new SelectList(db.SurveyTemplates, "SurveyTemplateID", "Description", surveytable.SurveyTemplateID);
            ViewBag.RequestIssueID = new SelectList(db.RequestIssues, "RequestIssueID", "DetailedDescription", surveytable.RequestIssueID);
            return View(surveytable);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}