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
    public class SurveyTemplateController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /SurveyTemplate/
        [Authorize]
        public ViewResult Index()
        {
            var surveytemplates = db.SurveyTemplates.Include(s => s.RequestType).Where(i => i.PreDefined == true);
            return View(surveytemplates.ToList());
        }

        //
        // GET: /SurveyTemplate/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.AllSurveyNodes = db.SurveyNodes.ToList();

            var query = db.RequestTypes.Where(u => db.SurveyTemplates.Where(i => i.PreDefined == true).Select(s => s.RequestTypeID).Contains(u.RequestTypeID));
            var emptyReqTypes = db.RequestTypes.Except(query);

            ViewBag.RequestTypeID = new SelectList(emptyReqTypes, "RequestTypeID", "Description");
            return View();
        }

        //
        // POST: /SurveyTemplate/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(SurveyTemplate surveytemplate, FormCollection formcollection)
        {
            if (ModelState.IsValid)
            {
                db.SurveyTemplates.Add(surveytemplate);
                surveytemplate.PreDefined = true;
                db.SaveChanges();
                surveytemplate.SurveyRecords = new List<SurveyRecord>();
                int xindex = 0;
                foreach (SurveyNode snode_ in db.SurveyNodes.ToList())
                {
                    try
                    {
                        if (bool.Parse(formcollection[snode_.SurveyNodeID.ToString() + "_Check"].Split(',')[0]))
                        {
                            SurveyRecord newsurvrec = new SurveyRecord { SurveyNodeID = snode_.SurveyNodeID, OrderNum = xindex, SurveyRecordTypeID = int.Parse(formcollection[snode_.SurveyNodeID.ToString() + "_survrectype"]) };
                            surveytemplate.SurveyRecords.Add(newsurvrec);
                            db.SaveChanges();
                            xindex++;
                        }
                    }
                    catch (Exception exx)
                    {
                        ViewBag.CustomErr = exx.Message;
                    }
                }

                return RedirectToAction("Index");
            }

            ViewBag.AllSurveyNodes = db.SurveyNodes.ToList();
            var query = db.RequestTypes.Where(u => db.SurveyTemplates.Where(i => i.PreDefined == true).Select(s => s.RequestTypeID).Contains(u.RequestTypeID));
            var emptyReqTypes = db.RequestTypes.Except(query);

            ViewBag.RequestTypeID = new SelectList(emptyReqTypes, "RequestTypeID", "Description", surveytemplate.RequestTypeID);
            return View(surveytemplate);
        }

        //
        // GET: /SurveyTemplate/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            SurveyTemplate surveytemplate = db.SurveyTemplates.Find(id);
            ViewBag.AllSurveyNodes = db.SurveyNodes.ToList();

            ViewBag.TheseSurveyRecords = surveytemplate.SurveyRecords.ToList();
            ViewBag.RequestTypeID = new SelectList(db.RequestTypes, "RequestTypeID", "Description", surveytemplate.RequestTypeID);
            return View(surveytemplate);
        }

        //
        // POST: /SurveyTemplate/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(SurveyTemplate surveytemplate, FormCollection formcollection)
        {
            if (ModelState.IsValid)
            {
                if (db.SurveyTemplates.AsNoTracking().Where(i => i.RequestTypeID == surveytemplate.RequestTypeID && i.PreDefined == true && i.SurveyTemplateID != surveytemplate.SurveyTemplateID).ToList().Count > 0)
                {
                    ViewBag.CustomErr = "İlgili İş tipine ait bir taslak zaten var. Lütfen Başka Bir Tip seçiniz";
                    return RedirectToAction("Edit", new { id = surveytemplate.SurveyTemplateID });
                }

                db.Entry(surveytemplate).State = EntityState.Modified;
                db.SaveChanges();
                SurveyTemplate mysurvtemplate_ = db.SurveyTemplates.Include(p => p.SurveyRecords).Where(i => i.SurveyTemplateID == surveytemplate.SurveyTemplateID).SingleOrDefault();
                int xindex = 0;
                foreach (SurveyNode snode_ in db.SurveyNodes.ToList())
                {
                    try
                    {
                        if (bool.Parse(formcollection[snode_.SurveyNodeID.ToString() + "_Remove"].Split(',')[0]))
                        {
                        }
                        else
                        {
                            SurveyRecord mysurvrec = db.SurveyTemplates.AsNoTracking().Where(i => i.SurveyTemplateID == surveytemplate.SurveyTemplateID).SingleOrDefault().SurveyRecords.Where(u => u.SurveyNodeID == snode_.SurveyNodeID).SingleOrDefault();
                            var mysrec = db.SurveyRecords.Find(mysurvrec.SurveyRecordID);
                            surveytemplate.SurveyRecords.Remove(mysrec);
                            db.Entry(surveytemplate).State = EntityState.Modified;

                            //db.SurveyRecords.Remove(mysurvrec);
                            db.SaveChanges();
                            KlmsnContext db_ = new KlmsnContext();
                            var mysrec_forremove = db_.SurveyRecords.Find(mysurvrec.SurveyRecordID);
                            db_.SurveyRecords.Remove(mysrec_forremove);
                            db_.SaveChanges();
                            db_.Dispose();
                            try
                            {
                                if (bool.Parse(formcollection[snode_.SurveyNodeID.ToString() + "_Check"].Split(',')[0]))
                                {
                                    SurveyRecord newsurvrec = new SurveyRecord { SurveyNodeID = snode_.SurveyNodeID, OrderNum = xindex, SurveyRecordTypeID = int.Parse(formcollection[snode_.SurveyNodeID.ToString() + "_survrectype"]) };
                                    mysurvtemplate_.SurveyRecords.Add(newsurvrec);
                                    db.SaveChanges();
                                    xindex++;
                                }
                            }
                            catch (Exception ex)
                            {
                                ViewBag.CustomErr = ex.Message;
                            }
                        }
                    }
                    catch 
                    {
                        try
                        {
                            if (bool.Parse(formcollection[snode_.SurveyNodeID.ToString() + "_Check"].Split(',')[0]))
                            {
                                SurveyRecord newsurvrec = new SurveyRecord { SurveyNodeID = snode_.SurveyNodeID, OrderNum = xindex, SurveyRecordTypeID = int.Parse(formcollection[snode_.SurveyNodeID.ToString() + "_survrectype"]) };
                                mysurvtemplate_.SurveyRecords.Add(newsurvrec);
                                db.SaveChanges();
                                xindex++;
                            }
                        }
                        catch (Exception ex)
                        {
                            ViewBag.CustomErr = ex.Message;
                        }
                    }
                    xindex++;
                }

                return RedirectToAction("Index");
            }

            ViewBag.AllSurveyNodes = db.SurveyNodes.ToList();
            ViewBag.TheseSurveyRecords = db.SurveyTemplates.Find(surveytemplate.SurveyTemplateID).SurveyRecords.ToList();
            ViewBag.RequestTypeID = new SelectList(db.RequestTypes, "RequestTypeID", "Description", surveytemplate.RequestTypeID);
            return View(surveytemplate);
        }

        //
        // GET: /SurveyTemplate/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            SurveyTemplate surveytemplate = db.SurveyTemplates.Find(id);
            return View(surveytemplate);
        }

        //
        // POST: /SurveyTemplate/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            SurveyTemplate surveytemplate = db.SurveyTemplates.Find(id);
            db.SurveyTemplates.Remove(surveytemplate);
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