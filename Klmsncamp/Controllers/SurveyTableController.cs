using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Klmsncamp.Models;
using Klmsncamp.ViewModels;
using PagedList;
using Telerik.Web.Mvc;

namespace Klmsncamp.Controllers
{
    public class SurveyTableController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        public ViewResult Index(int? page)
        {
            /* var surveys = from s in db.SurveyTables select s; //.Where(i=>i.ValidationStateID==1).Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState);

             surveys = surveys.Where(i => i.IsApproved == true);

             surveys = surveys.Include(r => r.SurveyTemplate.SurveyRecords);

             //enter paging:
             int pageIndex, pageSize;
             if (page == -1)
             {
                 pageIndex = 1;
                 pageSize = surveys.ToList().Count;
             }
             else
             {
                 pageIndex = (page ?? 1);
                 pageSize = 50;
             }

             var rlist = surveys.AsEnumerable().ToPagedList(pageIndex, pageSize);
             ViewBag.CurrentPage = (page ?? 1);
             ViewBag.PrevPageNumber = rlist.PageNumber - 1;
             ViewBag.NextPageNumber = rlist.PageNumber + 1;
             ViewBag.TotalPages = rlist.PageCount;
             ViewBag.HasPrevPage = rlist.HasPreviousPage;
             ViewBag.HasNextPage = rlist.HasNextPage;
             ViewBag.isFirstPage = rlist.IsFirstPage;
             ViewBag.isLastPage = rlist.IsLastPage;
             ViewBag.StartIndex = rlist.FirstItemOnPage;
             ViewBag.EndIndex = rlist.LastItemOnPage;
             ViewBag.TotalItems = rlist.TotalItemCount;
             Response.AddHeader("Refresh", "90");
             return View(rlist);*/
            return View();
        }

        public IEnumerable<Klmsncamp.Models.SurveyTable> GetSurveys()
        {
            //var surveys = from s in db.SurveyTables select s; //.Where(i=>i.ValidationStateID==1).Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState);

            //surveys = surveys.Where(i => i.IsApproved == true);

            //surveys = surveys.Include(r => r.SurveyTemplate.SurveyRecords);
            /*
            //enter paging:
            int pageIndex, pageSize;
            if (page == -1)
            {
                pageIndex = 1;
                pageSize = surveys.ToList().Count;
            }
            else
            {
                pageIndex = (page ?? 1);
                pageSize = 50;
            }
            */

            /*var rlist = surveys.AsEnumerable().ToPagedList(pageIndex, pageSize);
            ViewBag.CurrentPage = (page ?? 1);
            ViewBag.PrevPageNumber = rlist.PageNumber - 1;
            ViewBag.NextPageNumber = rlist.PageNumber + 1;
            ViewBag.TotalPages = rlist.PageCount;
            ViewBag.HasPrevPage = rlist.HasPreviousPage;
            ViewBag.HasNextPage = rlist.HasNextPage;
            ViewBag.isFirstPage = rlist.IsFirstPage;
            ViewBag.isLastPage = rlist.IsLastPage;
            ViewBag.StartIndex = rlist.FirstItemOnPage;
            ViewBag.EndIndex = rlist.LastItemOnPage;
            ViewBag.TotalItems = rlist.TotalItemCount;
            Response.AddHeader("Refresh", "90");*/
            var rlist = new List<SurveyTable>();
            foreach (var sv in db.SurveyTables)
            {
                rlist.Add(sv);
            }
            return rlist;
        }

        [GridAction]
        public ActionResult _AjaxBinding()
        {
            var surveytables = from e in db.SurveyTables.Where(i => i.IsApproved == true)
                               select new SurveyTableViewModel
                               {
                                   SurveyTableID = e.SurveyTableID,
                                   RequestIssueID = e.RequestIssueID,
                                   Description = e.Description,
                                   Timestamp = e.TimeStamp
                               };

            //sorun şu kendi basit viewmodelini yapıcan
            return View(new GridModel(surveytables));
        }

        [GridAction]
        public ActionResult _AjaxBindingDetails(int surveyID)
        {
            int mysurvtemplateID = db.SurveyTables.Find(surveyID).SurveyTemplateID;

            SurveyTemplate survtemplate = db.SurveyTemplates.Include(u => u.SurveyRecords).Where(i => i.SurveyTemplateID == mysurvtemplateID).SingleOrDefault();

            var surveyrecords = from e in survtemplate.SurveyRecords
                                select new SurveyTableDetailViewModel
                                  {
                                      SurveyNodeDescription = e.SurveyNode.Description,
                                      SurveyRecordTypeDescription = e.SurveyRecordType.Description,
                                      ApprovalStatus = e.ApprovalStatus,
                                      Score = int.Parse(e.Score.GetValueOrDefault(0).ToString()),
                                      Note = e.Note
                                  };

            return View(new GridModel(surveyrecords));
        }

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