﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Klmsncamp.DAL;
using Klmsncamp.Models;
using Klmsncamp.ViewModels;
using NPOI.HSSF.UserModel;
using PagedList;

namespace Klmsncamp.Controllers
{
    public class RequestIssueController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /RequestIssue/
        [Authorize]
        public ActionResult Find(int? Rqid)
        {
            try
            {
                RequestIssue rq = db.RequestIssues.Find(Rqid);
                return RedirectToAction("Edit", new { id = rq.RequestIssueID, show = "A", page = 1 });
            }
            catch
            {
                return RedirectToAction("Index", "Home", new { err = "404" });
            }
        }

        [Authorize]
        public ViewResult Index(string show, int? page, string resp)
        {
            MembershipUser currentuser_ = new UserRepository().GetUser(User.Identity.Name);
            int user_wherecondition = int.Parse((currentuser_.ProviderUserKey).ToString());

            var requests = from r in db.RequestIssues select r; //.Where(i=>i.ValidationStateID==1).Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState);

            if (!(User.IsInRole("moderators") || User.IsInRole("administrators")))
            {
                requests = requests.Where(i => i.UserReqID == user_wherecondition);//.Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState);
            }

            //sadece kendilerinin valide ettikleri
            requests = requests.Where(i => i.ValidationStateID == 1 || (i.ValidationStateID == 2 && i.UserReqID == user_wherecondition));

            //else
            //{
            //    requests = requests.Where(i => i.ValidationStateID == 1);//.Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState);
            //}

            if (show != null)
            {
                if (show == "W")
                {
                    requests = requests.Where(i => i.ValidationStateID == 2 && i.UserReqID == user_wherecondition);
                }
                else if (show == "P")
                {
                    requests = requests.Where(i => i.IsApproved == false);
                }
                else if (show == "C")
                {
                    requests = requests.Where(i => i.IsApproved == true);
                }

                ViewBag.CurrentShow = show;
            }
            else
            {
                ViewBag.CurrentShow = "A";
            }

            if (resp != null)
            {
                if (resp == "Y")
                {
                    requests = requests.Where(i => i.UserID == user_wherecondition);
                    ViewBag.CurrentResp = "Y";
                }
            }

            requests = requests.Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Personnel).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState).OrderByDescending(o => o.RequestIssueID);

            //enter paging:
            int pageIndex, pageSize;
            if (page == -1)
            {
                pageIndex = 1;
                pageSize = requests.ToList().Count;
            }
            else
            {
                pageIndex = (page ?? 1);
                pageSize = 50;
            }

            var rlist = requests.AsEnumerable().ToPagedList(pageIndex, pageSize);

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
            return View(rlist);
        }

        private IEnumerable<T> MakeMeEnumerable<T>(T Entity)
        {
            yield return Entity;
        }

        //
        // GET: /RequestIssue/Details/5

        public ViewResult Details(int id)
        {
            RequestIssue requestıssue = db.RequestIssues.Find(id);
            return View(requestıssue);
        }

        [Authorize]
        public ActionResult Validate(int id, string show, int page)
        {
            RequestIssue requestissue = db.RequestIssues.Find(id);

            if (requestissue.ValidationStateID != 1)
            {
                requestissue.ValidationStateID = 1;
                db.Entry(requestissue).State = EntityState.Modified;
                db.SaveChanges();

                if (requestissue.SendEmail == true)
                {
                    MembershipUser currentuser_ = new UserRepository().GetUser(User.Identity.Name);
                    User user_from = db.Users.AsNoTracking().Where(b => b.UserId == requestissue.UserID).SingleOrDefault();

                    string mailsonucstr = SendEmail(new MailAddress(user_from.Email), new MailAddress(currentuser_.Email), "[Klimasan HelpDesk] İş isteğiniz hakkında.", "İsteğiniz doğrulanarak kayıt altına alınmıştır.Tarih: " + DateTime.Now.ToString() + " - İş No: #" + (requestissue.RequestIssueID).ToString() + ". İyi çalışmalar dileriz.");
                    if (mailsonucstr != "OK")
                    {
                        ViewBag.Bilgilendirme = "Mail Gönderiminde Hata: " + mailsonucstr;
                    }
                    else
                    {
                        ViewBag.Bilgilendirme = "Mail Gönderimi başarılı..";
                    }
                }
                return RedirectToAction("Index", new { show = show, page = page });
            }
            else
            {
                ViewBag.Bilgilendirme = "Bu İş için doğrulama için yetkisi yok ya da zaten doğrulanmış.";
            }
            return RedirectToAction("Index", new { show = show, page = page });
        }

        //
        // GET: /RequestIssue/Create
        [Authorize]
        public ActionResult Create(int? projectid)
        {
            MembershipUser currentuser_ = new UserRepository().GetUser(User.Identity.Name);
            int user_wherecondition = int.Parse((currentuser_.ProviderUserKey).ToString());

            ViewBag.RequestTypeID = new SelectList(db.RequestTypes, "RequestTypeID", "Description");
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description");
            ViewBag.PersonnelID = new SelectList(db.Personnels, "PersonnelID", "FullName");
            ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "Description");
            ViewBag.WorkshopID = new SelectList(db.Workshops, "WorkshopID", "Description");
            ViewBag.RequestStateID = new SelectList(db.RequestStates, "RequestStateID", "Description", 1);
            ViewBag.UserReqID = new SelectList(db.Users.Where(x => x.UserId == user_wherecondition), "UserId", "FullName", currentuser_.ProviderUserKey);
            ViewBag.UserID = new SelectList(db.Users, "UserId", "FullName", currentuser_.ProviderUserKey);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description");

            if (projectid != null && projectid.GetType().Name == "Int32")
            {
                ViewBag.DetailProjectID = new MultiSelectList(db.Projects, "ProjectID", "MultiboxDescription", db.Projects.Where(i => i.ProjectID == projectid).Select(p => p.ProjectID).ToList());
                ViewBag.DetailLocationID = new MultiSelectList(db.Locations, "LocationID", "Description", db.Projects.Include(s => s.Locations).Where(i => i.ProjectID == projectid).SingleOrDefault().Locations.Select(p => p.LocationID).ToList());
                ViewBag.DetailCorporateAccountID = new MultiSelectList(db.CorporateAccounts, "CorporateAccountID", "Title", db.Projects.Include(s => s.CorporateAccounts).Where(i => i.ProjectID == projectid).SingleOrDefault().CorporateAccounts.Select(p => p.CorporateAccountID).ToList());
                ViewBag.DetailPersonnelID = new MultiSelectList(db.Personnels, "PersonnelID", "FullName", db.Projects.Include(s => s.Personnels).Where(i => i.ProjectID == projectid).SingleOrDefault().Personnels.Select(p => p.PersonnelID).ToList());
            }
            else
            {
                ViewBag.DetailProjectID = new MultiSelectList(db.Projects, "ProjectID", "MultiboxDescription");
                ViewBag.DetailLocationID = new MultiSelectList(db.Locations, "LocationID", "Description");
                ViewBag.DetailCorporateAccountID = new MultiSelectList(db.CorporateAccounts, "CorporateAccountID", "Title");
                ViewBag.DetailPersonnelID = new MultiSelectList(db.Personnels, "PersonnelID", "FullName");
            }

            ViewBag.timestamp = DateTime.Now;
            return View();
        }

        //
        // POST: /RequestIssue/Create

        [Authorize]
        [HttpPost]
        public ActionResult Create(RequestIssue requestıssue, FormCollection formcollection)
        {
            if (ModelState.IsValid)
            {
                requestıssue.TimeStamp = DateTime.Now;
                requestıssue.ValidationStateID = 2;
                db.RequestIssues.Add(requestıssue);
                db.SaveChanges();

                if (formcollection["DetailLocationID"] != null)
                {
                    foreach (var location_ in formcollection["DetailLocationID"].Split(',').ToList())
                    {
                        try
                        {
                            int location_index = int.Parse(location_.ToString());
                            var x_location = db.Locations.Find(location_index);
                            x_location.RequestIssues.Add(requestıssue);
                        }
                        catch
                        { }
                        db.SaveChanges();
                    }
                }

                if (formcollection["DetailCorporateAccountID"] != null)
                {
                    foreach (var corp_ in formcollection["DetailCorporateAccountID"].Split(',').ToList())
                    {
                        try
                        {
                            int corp_index = int.Parse(corp_.ToString());
                            var x_corp = db.CorporateAccounts.Find(corp_index);
                            x_corp.RequestIssues.Add(requestıssue);
                        }
                        catch
                        { }

                        db.SaveChanges();
                    }
                }

                if (formcollection["DetailPersonnelID"] != null)
                {
                    foreach (var pers_ in formcollection["DetailPersonnelID"].Split(',').ToList())
                    {
                        try
                        {
                            int pers_index = int.Parse(pers_.ToString());
                            var x_pers = db.Personnels.Find(pers_index);
                            x_pers.RequestIssues.Add(requestıssue);
                        }
                        catch
                        { }
                        db.SaveChanges();
                    }
                }

                if (formcollection["DetailProjectID"] != null)
                {
                    foreach (var proj_ in formcollection["DetailProjectID"].Split(',').ToList())
                    {
                        try
                        {
                            int project_index = int.Parse(proj_.ToString());
                            var x_proj = db.Projects.Find(project_index);
                            x_proj.RequestIssues.Add(requestıssue);
                        }
                        catch
                        { }
                        db.SaveChanges();
                    }
                }

                int i = 0;
                foreach (var file_ in Request.Files)
                {
                    RequestIssueFile file = RetrieveFileFromRequest(i);

                    if (file.RequestIssueFileName != null
                        && !db.RequestIssueFiles.Any(f => f.RequestIssueFileName.Equals(file.RequestIssueFileName))
                        && file.RequestIssueFileSize > 0)
                    {
                        file.RequestIssue = requestıssue;
                        db.RequestIssueFiles.Add(file);
                        db.SaveChanges();
                    }
                    i++;
                }

                return RedirectToAction("Details/" + requestıssue.RequestIssueID.ToString());
            }

            MembershipUser currentuser_ = new UserRepository().GetUser(User.Identity.Name);
            int user_wherecondition = int.Parse((currentuser_.ProviderUserKey).ToString());

            ViewBag.RequestTypeID = new SelectList(db.RequestTypes, "RequestTypeID", "Description", requestıssue.RequestTypeID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description", requestıssue.LocationID);
            ViewBag.PersonnelID = new SelectList(db.Personnels, "PersonnelID", "FullName", requestıssue.PersonnelID);
            ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "Description", requestıssue.InventoryID);
            ViewBag.WorkshopID = new SelectList(db.Workshops, "WorkshopID", "Description", requestıssue.WorkshopID);
            ViewBag.RequestStateID = new SelectList(db.RequestStates, "RequestStateID", "Description", requestıssue.RequestStateID);
            ViewBag.UserReqID = new SelectList(db.Users.Where(x => x.UserId == user_wherecondition), "UserId", "FullName", requestıssue.UserReqID);
            ViewBag.UserID = new SelectList(db.Users, "UserId", "FullName", requestıssue.UserID);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", requestıssue.ValidationStateID);

            if (formcollection["DetailProjectID"] != null)
            {
                ViewBag.DetailProjectID = new MultiSelectList(db.Projects, "ProjectID", "MultiboxDescription", formcollection["DetailProjectID"].Split(',').ToList());
            }
            else
            {
                ViewBag.DetailProjectID = new MultiSelectList(db.Projects, "ProjectID", "MultiboxDescription");
            }

            if (formcollection["DetailLocationID"] != null)
            {
                ViewBag.DetailLocationID = new MultiSelectList(db.Locations, "LocationID", "Description", formcollection["DetailLocationID"].Split(',').ToList());
            }
            else
            {
                ViewBag.DetailLocationID = new MultiSelectList(db.Locations, "LocationID", "Description");
            }

            if (formcollection["DetailCorporateAccountID"] != null)
            {
                ViewBag.DetailCorporateAccountID = new MultiSelectList(db.CorporateAccounts, "CorporateAccountID", "Title", formcollection["DetailCorporateAccountID"].Split(',').ToList());
            }
            else
            {
                ViewBag.DetailCorporateAccountID = new MultiSelectList(db.CorporateAccounts, "CorporateAccountID", "Title");
            }

            if (formcollection["DetailPersonnelID"] != null)
            {
                ViewBag.DetailPersonnelID = new MultiSelectList(db.Personnels, "PersonnelID", "FullName", formcollection["DetailPersonnelID"].Split(',').ToList()); ;
            }
            else
            {
                ViewBag.DetailPersonnelID = new MultiSelectList(db.Personnels, "PersonnelID", "FullName");
            }
            return View(requestıssue);
        }

        //
        // GET: /RequestIssue/Edit/5
        [Authorize]
        public ActionResult Edit(int id, string show, int page)
        {
            RequestIssue requestıssue = db.RequestIssues.Include(p => p.RequestIssueFiles).Include(p => p.Projects).Include(p => p.Locations).Include(p => p.CorporateAccounts).Include(p => p.Personnels).Where(i => i.RequestIssueID == id).SingleOrDefault();

            if (requestıssue.ValidationStateID == 1)
            {
                return RedirectToAction("Editp", new { id = id.ToString(), show = show, page = page });
            }

            ViewBag.RequestTypeID = new SelectList(db.RequestTypes, "RequestTypeID", "Description", requestıssue.RequestTypeID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description", requestıssue.LocationID);
            ViewBag.PersonnelID = new SelectList(db.Personnels, "PersonnelID", "FullName", requestıssue.PersonnelID);
            ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "Description", requestıssue.InventoryID);
            ViewBag.WorkshopID = new SelectList(db.Workshops, "WorkshopID", "Description", requestıssue.WorkshopID);
            ViewBag.RequestStateID = new SelectList(db.RequestStates, "RequestStateID", "Description", requestıssue.RequestStateID);
            ViewBag.UserReqID = new SelectList(db.Users.Where(i => i.UserId == requestıssue.UserReqID), "UserId", "FullName", requestıssue.UserReqID);
            ViewBag.UserID = new SelectList(db.Users, "UserId", "FullName", requestıssue.UserID);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", requestıssue.ValidationStateID);

            ViewBag.DetailProjectID = new MultiSelectList(db.Projects, "ProjectID", "MultiboxDescription", requestıssue.Projects.Select(p => p.ProjectID).ToList());
            ViewBag.DetailLocationID = new MultiSelectList(db.Locations, "LocationID", "Description", requestıssue.Locations.Select(p => p.LocationID).ToList());
            ViewBag.DetailCorporateAccountID = new MultiSelectList(db.CorporateAccounts, "CorporateAccountID", "Title", requestıssue.CorporateAccounts.Select(p => p.CorporateAccountID).ToList());
            ViewBag.DetailPersonnelID = new MultiSelectList(db.Personnels, "PersonnelID", "FullName", requestıssue.Personnels.Select(p => p.PersonnelID).ToList());

            ViewBag.show = show;
            ViewBag.page = page;

            return View(requestıssue);
        }

        //
        // POST: /RequestIssue/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(RequestIssue requestıssue, FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requestıssue).State = EntityState.Modified;
                db.SaveChanges();

                //locations,corps,pers bosalt
                RequestIssue requestissue_ = db.RequestIssues.Include(p => p.RequestIssueFiles).Include(p => p.Projects).Include(p => p.Locations).Include(p => p.Personnels).Include(p => p.CorporateAccounts).Where(i => i.RequestIssueID == requestıssue.RequestIssueID).SingleOrDefault();
                requestissue_.Projects.Clear();
                requestissue_.Locations.Clear();
                requestissue_.Personnels.Clear();
                requestissue_.CorporateAccounts.Clear();
                db.SaveChanges();

                if (formCollection["DetailLocationID"] != null)
                {
                    foreach (var location_ in formCollection["DetailLocationID"].Split(',').ToList())
                    {
                        try
                        {
                            int location_index = int.Parse(location_.ToString());
                            var x_location = db.Locations.Find(location_index);
                            x_location.RequestIssues.Add(requestıssue);
                        }
                        catch
                        { }
                        db.SaveChanges();
                    }
                }

                if (formCollection["DetailCorporateAccountID"] != null)
                {
                    foreach (var corp_ in formCollection["DetailCorporateAccountID"].Split(',').ToList())
                    {
                        try
                        {
                            int corp_index = int.Parse(corp_.ToString());
                            var x_corp = db.CorporateAccounts.Find(corp_index);
                            x_corp.RequestIssues.Add(requestıssue);
                        }
                        catch
                        { }

                        db.SaveChanges();
                    }
                }

                if (formCollection["DetailPersonnelID"] != null)
                {
                    foreach (var pers_ in formCollection["DetailPersonnelID"].Split(',').ToList())
                    {
                        try
                        {
                            int pers_index = int.Parse(pers_.ToString());
                            var x_pers = db.Personnels.Find(pers_index);
                            x_pers.RequestIssues.Add(requestıssue);
                        }
                        catch
                        { }
                        db.SaveChanges();
                    }
                }

                if (formCollection["DetailProjectID"] != null)
                {
                    foreach (var proj_ in formCollection["DetailProjectID"].Split(',').ToList())
                    {
                        try
                        {
                            int project_index = int.Parse(proj_.ToString());
                            var x_proj = db.Projects.Find(project_index);
                            x_proj.RequestIssues.Add(requestıssue);
                        }
                        catch
                        { }
                        db.SaveChanges();
                    }
                }

                int fileindex_ = 0;
                foreach (var file_ in Request.Files)
                {
                    RequestIssueFile file = RetrieveFileFromRequest(fileindex_);

                    if (file.RequestIssueFileName != null
                        && !db.RequestIssueFiles.Any(f => f.RequestIssueFileName.Equals(file.RequestIssueFileName))
                        && file.RequestIssueFileSize > 0)
                    {
                        file.RequestIssue = requestıssue;
                        db.RequestIssueFiles.Add(file);
                        db.SaveChanges();
                    }
                    fileindex_++;
                }
                return RedirectToAction("Validate", "RequestIssue", new { id = requestıssue.RequestIssueID, show = formCollection["show"], page = formCollection["page"] });
            }
            ViewBag.RequestTypeID = new SelectList(db.RequestTypes, "RequestTypeID", "Description", requestıssue.RequestTypeID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description", requestıssue.LocationID);
            ViewBag.PersonnelID = new SelectList(db.Personnels, "PersonnelID", "FullName", requestıssue.PersonnelID);
            ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "Description", requestıssue.InventoryID);
            ViewBag.WorkshopID = new SelectList(db.Workshops, "WorkshopID", "Description", requestıssue.WorkshopID);
            ViewBag.RequestStateID = new SelectList(db.RequestStates, "RequestStateID", "Description", requestıssue.RequestStateID);
            ViewBag.UserReqID = new SelectList(db.Users, "UserId", "FullName", requestıssue.UserReqID);
            ViewBag.UserID = new SelectList(db.Users, "UserId", "FullName", requestıssue.UserID);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", requestıssue.ValidationStateID);

            RequestIssue requestissue__ = db.RequestIssues.Include(p => p.RequestIssueFiles).Include(p => p.Projects).Include(p => p.Locations).Include(p => p.Personnels).Include(p => p.CorporateAccounts).Where(i => i.RequestIssueID == requestıssue.RequestIssueID).SingleOrDefault();

            requestıssue.RequestIssueFiles = requestissue__.RequestIssueFiles;
            requestıssue.Projects = requestissue__.Projects;
            requestıssue.Locations = requestissue__.Locations;
            requestıssue.CorporateAccounts = requestissue__.CorporateAccounts;
            requestıssue.Personnels = requestissue__.Personnels;

            ViewBag.DetailProjectID = new MultiSelectList(db.Projects, "ProjectID", "MultiboxDescription", requestissue__.Projects.Select(p => p.ProjectID).ToList());
            ViewBag.DetailLocationID = new MultiSelectList(db.Locations, "LocationID", "Description", requestissue__.Locations.Select(p => p.LocationID).ToList());
            ViewBag.DetailCorporateAccountID = new MultiSelectList(db.CorporateAccounts, "CorporateAccountID", "Title", requestissue__.CorporateAccounts.Select(p => p.CorporateAccountID).ToList());
            ViewBag.DetailPersonnelID = new MultiSelectList(db.Personnels, "PersonnelID", "FullName", requestissue__.Personnels.Select(p => p.PersonnelID).ToList());

            ViewBag.show = formCollection["show"];
            ViewBag.page = formCollection["page"];
            return View(requestıssue);
        }

        [Authorize]
        public ActionResult Editp(int id, string show, int page)
        {
            RequestIssue rq = db.RequestIssues.Include(p => p.RequestIssueFiles).Include(p => p.Projects).Include(p => p.Locations).Include(p => p.CorporateAccounts).Include(p => p.Personnels).Where(i => i.RequestIssueID == id).SingleOrDefault();
            if (rq.ValidationStateID == 2)
            {
                return RedirectToAction("Edit", new { id = id.ToString(), show = show, page = page });
            }
            else
            {
                ViewBag.RequestTypeID = new SelectList(db.RequestTypes, "RequestTypeID", "Description", rq.RequestTypeID);
                ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description", rq.LocationID);
                ViewBag.PersonnelID = new SelectList(db.Personnels, "PersonnelID", "FullName", rq.PersonnelID);
                ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "Description", rq.InventoryID);
                ViewBag.WorkshopID = new SelectList(db.Workshops, "WorkshopID", "Description", rq.WorkshopID);
                ViewBag.RequestStateID = new SelectList(db.RequestStates, "RequestStateID", "Description", rq.RequestStateID);
                ViewBag.UserReqID = new SelectList(db.Users, "UserId", "FullName", rq.UserReq.UserId);

                ViewBag.DetailProjectID = new MultiSelectList(db.Projects, "ProjectID", "MultiboxDescription", rq.Projects.Select(p => p.ProjectID).ToList());
                ViewBag.DetailLocationID = new MultiSelectList(db.Locations, "LocationID", "Description", rq.Locations.Select(p => p.LocationID).ToList());
                ViewBag.DetailCorporateAccountID = new MultiSelectList(db.CorporateAccounts, "CorporateAccountID", "Title", rq.CorporateAccounts.Select(p => p.CorporateAccountID).ToList());
                ViewBag.DetailPersonnelID = new MultiSelectList(db.Personnels, "PersonnelID", "FullName", rq.Personnels.Select(p => p.PersonnelID).ToList());

                ViewBag.show = show;
                ViewBag.page = page;

                if (User.IsInRole("administrators") || User.IsInRole("moderators"))
                {
                    ViewBag.UserID = new SelectList(db.Users, "UserId", "FullName", rq.UserID);
                }
                else
                {
                    MembershipUser currentuser_ = new UserRepository().GetUser(User.Identity.Name);
                    int user_wherecondition = int.Parse((currentuser_.ProviderUserKey).ToString());
                    ViewBag.UserID = new SelectList(db.Users.Where(i => i.UserId == user_wherecondition), "UserId", "FullName", rq.UserID);
                }

                ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", rq.ValidationStateID);

                //loglari göstermece
                IList<LogRequestIssue> MyLogs = new List<LogRequestIssue>();
                foreach (LogRequestIssue log_item in db.LogRequestIssues.Where(i => i.RequestIssueID == id).ToList())
                {
                    foreach (LogRequestIssueDetail logdetail_item in db.LogRequestIssueDetails.Where(s => s.LogRequestIssueID == log_item.LogRequestIssueID).ToList())
                    {
                        log_item.LogRequestIssueDetails.Add(logdetail_item);
                    }
                    if (log_item.LogRequestIssueDetails.Count() > 0)
                    {
                        MyLogs.Add(log_item);
                    }
                }
                ViewBag.TheLogs = MyLogs;

                return View(rq);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Editp(int id, RequestIssue rqToUpdate, FormCollection formCollection)
        {
            MembershipUser currentuser_ = new UserRepository().GetUser(User.Identity.Name);
            int user_wherecondition = int.Parse((currentuser_.ProviderUserKey).ToString());

            var rqDBrecord = db.RequestIssues.AsNoTracking().Where(i => i.RequestIssueID == rqToUpdate.RequestIssueID).SingleOrDefault();

            if (formCollection["isApproved"] == "false" && rqDBrecord.IsApproved == true && User.IsInRole("administrators"))
            {
                var rqToUpdatex = db.RequestIssues.Find(id);
                rqToUpdatex.IsApproved = false;
                db.Entry(rqToUpdatex).State = EntityState.Modified;
                db.SaveChanges();
                //log
                LogRequestIssue mylog = new LogRequestIssue { RequestIssueID = id, Action = "Güncelleme", ModifyTime = DateTime.Now, UserID = user_wherecondition };
                db.LogRequestIssues.Add(mylog);
                db.SaveChanges();
                LogRequestIssueDetail mylogdetail = new LogRequestIssueDetail { LogRequestIssueID = mylog.LogRequestIssueID, PropertyName = "isApproved", PropertyOldValue = "True", PropertyNewValue = "False" };
                db.LogRequestIssueDetails.Add(mylogdetail);
                db.SaveChanges();
                mylog.LogRequestIssueDetails.Add(mylogdetail);
                db.SaveChanges();
                //log biter
                return RedirectToAction("Editp", new { id = rqToUpdatex.RequestIssueID.ToString(), show = formCollection["show"], page = formCollection["page"] });
            }

            //bool _enddatepostpone = CheckEndDateIfCanBeUpdated(rqToUpdate);

            if (ModelState.IsValid)
            {
                if (rqToUpdate.IsApproved && rqToUpdate.RequestStateID != 6)
                {
                    rqToUpdate.RequestStateID = 5;
                }
                db.Entry(rqToUpdate).State = EntityState.Modified;
                db.SaveChanges();

                //locations,corps,pers bosalt
                RequestIssue requestissue_ = db.RequestIssues.Include(p => p.RequestIssueFiles).Include(p => p.Projects).Include(p => p.Locations).Include(p => p.Personnels).Include(p => p.CorporateAccounts).Where(i => i.RequestIssueID == id).SingleOrDefault();
                requestissue_.Locations.Clear();
                requestissue_.Personnels.Clear();
                requestissue_.CorporateAccounts.Clear();
                requestissue_.Projects.Clear();
                db.SaveChanges();

                if (formCollection["DetailLocationID"] != null)
                {
                    foreach (var location_ in formCollection["DetailLocationID"].Split(',').ToList())
                    {
                        try
                        {
                            int location_index = int.Parse(location_.ToString());
                            var x_location = db.Locations.Find(location_index);
                            x_location.RequestIssues.Add(rqToUpdate);
                        }
                        catch
                        { }
                        db.SaveChanges();
                    }
                }

                if (formCollection["DetailCorporateAccountID"] != null)
                {
                    foreach (var corp_ in formCollection["DetailCorporateAccountID"].Split(',').ToList())
                    {
                        try
                        {
                            int corp_index = int.Parse(corp_.ToString());
                            var x_corp = db.CorporateAccounts.Find(corp_index);
                            x_corp.RequestIssues.Add(rqToUpdate);
                        }
                        catch
                        { }

                        db.SaveChanges();
                    }
                }

                if (formCollection["DetailPersonnelID"] != null)
                {
                    foreach (var pers_ in formCollection["DetailPersonnelID"].Split(',').ToList())
                    {
                        try
                        {
                            int pers_index = int.Parse(pers_.ToString());
                            var x_pers = db.Personnels.Find(pers_index);
                            x_pers.RequestIssues.Add(rqToUpdate);
                        }
                        catch
                        { }
                        db.SaveChanges();
                    }
                }

                if (formCollection["DetailProjectID"] != null)
                {
                    foreach (var proj_ in formCollection["DetailProjectID"].Split(',').ToList())
                    {
                        try
                        {
                            int project_index = int.Parse(proj_.ToString());
                            var x_proj = db.Projects.Find(project_index);
                            x_proj.RequestIssues.Add(rqToUpdate);
                        }
                        catch
                        { }
                        db.SaveChanges();
                    }
                }

                int fileindex_ = 0;
                foreach (var file_ in Request.Files)
                {
                    RequestIssueFile file = RetrieveFileFromRequest(fileindex_);

                    if (file.RequestIssueFileName != null
                        && !db.RequestIssueFiles.Any(f => f.RequestIssueFileName.Equals(file.RequestIssueFileName))
                        && file.RequestIssueFileSize > 0)
                    {
                        file.RequestIssue = rqToUpdate;
                        db.RequestIssueFiles.Add(file);
                        db.SaveChanges();
                    }
                    fileindex_++;
                }

                bool xdurum = KlmsnExtensions.BoolPropertyDifferences(rqDBrecord, rqToUpdate);

                if (rqToUpdate.SendEmail == true && xdurum == true)
                {
                    User user_from = db.Users.AsNoTracking().Where(b => b.UserId == rqToUpdate.UserID).SingleOrDefault();
                    User userReq_to = db.Users.AsNoTracking().Where(b => b.UserId == rqToUpdate.UserReqID).SingleOrDefault();
                    string mailsonucstr = SendEmail(new MailAddress(user_from.Email), new MailAddress(userReq_to.Email), "[Klimasan HelpDesk] #" + rqToUpdate.RequestIssueID.ToString() + " no'lu İş isteğiniz hakkında.", "İş İsteğiniz güncellenmiştir. İsteğinizin son durumu görmek isterseniz;  http://192.168.76.176/HelpDesk/RequestIssue/Editp/" + rqToUpdate.RequestIssueID.ToString() + "?show=A&page=1 adresini ziyaret ediniz. Tarih: " + DateTime.Now.ToString() + ". İyi çalışmalar dileriz.");
                    if (mailsonucstr != "OK")
                    {
                        ViewBag.Bilgilendirme = "Mail Gönderiminde Hata: " + mailsonucstr;
                    }
                    else
                    {
                        ViewBag.Bilgilendirme = "Mail Başarıyla Gönderildi";
                    }
                }

                //logging mate
                if (xdurum == true)
                {
                    var Logs = KlmsnExtensions.LogPropertyDifferences(rqDBrecord, rqToUpdate);

                    LogRequestIssue mylog = new LogRequestIssue { RequestIssueID = id, Action = "Güncelleme", ModifyTime = DateTime.Now, UserID = user_wherecondition };
                    db.LogRequestIssues.Add(mylog);
                    db.SaveChanges();
                    foreach (LogRequestIssueViewModel log in Logs)
                    {
                        LogRequestIssueDetail mylogdetail = new LogRequestIssueDetail { LogRequestIssueID = mylog.LogRequestIssueID, PropertyName = log.PropertyName, PropertyOldValue = log.OldValue, PropertyNewValue = log.NewValue };
                        db.LogRequestIssueDetails.Add(mylogdetail);
                        db.SaveChanges();
                        mylog.LogRequestIssueDetails.Add(mylogdetail);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index", new { show = formCollection["show"], page = formCollection["page"] });
            }

            //if (_enddatepostpone == false)
            //{
            //    ViewBag.EndDatePostponeErr = "Bu iş daha önce 2 defa ertelenmiş. Bitiş Tarihi değiştirilemez";
            //}
            //return RedirectToAction("Editp/" + id.ToString());
            rqToUpdate.IsApproved = false;
            ViewBag.RequestTypeID = new SelectList(db.RequestTypes, "RequestTypeID", "Description", rqToUpdate.RequestTypeID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description", rqToUpdate.LocationID);
            ViewBag.PersonnelID = new SelectList(db.Personnels, "PersonnelID", "FullName", rqToUpdate.PersonnelID);
            ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "Description", rqToUpdate.InventoryID);
            ViewBag.WorkshopID = new SelectList(db.Workshops, "WorkshopID", "Description", rqToUpdate.WorkshopID);
            ViewBag.RequestStateID = new SelectList(db.RequestStates, "RequestStateID", "Description", rqToUpdate.RequestStateID);
            ViewBag.UserReqID = new SelectList(db.Users, "UserId", "FullName", rqToUpdate.UserReqID);
            ViewBag.UserID = new SelectList(db.Users, "UserId", "FullName", rqToUpdate.UserID);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", rqToUpdate.ValidationStateID);

            RequestIssue requestissue__ = db.RequestIssues.Include(p => p.RequestIssueFiles).Include(p => p.Projects).Include(p => p.Locations).Include(p => p.Personnels).Include(p => p.CorporateAccounts).Where(i => i.RequestIssueID == rqToUpdate.RequestIssueID).SingleOrDefault();
            rqToUpdate.RequestIssueFiles = requestissue__.RequestIssueFiles;
            rqToUpdate.Projects = requestissue__.Projects;
            rqToUpdate.Locations = requestissue__.Locations;
            rqToUpdate.CorporateAccounts = requestissue__.CorporateAccounts;
            rqToUpdate.Personnels = requestissue__.Personnels;

            ViewBag.DetailProjectID = new MultiSelectList(db.Projects, "ProjectID", "MultiboxDescription", requestissue__.Projects.Select(p => p.ProjectID).ToList());
            ViewBag.DetailLocationID = new MultiSelectList(db.Locations, "LocationID", "Description", requestissue__.Locations.Select(p => p.LocationID).ToList());
            ViewBag.DetailCorporateAccountID = new MultiSelectList(db.CorporateAccounts, "CorporateAccountID", "Title", requestissue__.CorporateAccounts.Select(p => p.CorporateAccountID).ToList());
            ViewBag.DetailPersonnelID = new MultiSelectList(db.Personnels, "PersonnelID", "FullName", requestissue__.Personnels.Select(p => p.PersonnelID).ToList());

            ViewBag.show = formCollection["show"];
            ViewBag.page = formCollection["page"];

            //loglari göstermece
            IList<LogRequestIssue> MyLogs = new List<LogRequestIssue>();
            foreach (LogRequestIssue log_item in db.LogRequestIssues.Where(i => i.RequestIssueID == id).ToList())
            {
                foreach (LogRequestIssueDetail logdetail_item in db.LogRequestIssueDetails.Where(s => s.LogRequestIssueID == log_item.LogRequestIssueID).ToList())
                {
                    log_item.LogRequestIssueDetails.Add(logdetail_item);
                }
                if (log_item.LogRequestIssueDetails.Count() > 0)
                {
                    MyLogs.Add(log_item);
                }
            }
            ViewBag.TheLogs = MyLogs;

            return View(rqToUpdate);
        }

        public bool CheckEndDateIfCanBeUpdated(RequestIssue my_model)
        {
            if (my_model.Pre1EndDate != null && my_model.Pre2EndDate != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //
        // GET: /RequestIssue/Delete/5
        [Authorize(Roles = "administrators")]
        public ActionResult Delete(int id)
        {
            RequestIssue requestıssue = db.RequestIssues.Find(id);
            return View(requestıssue);
        }

        //
        // POST: /RequestIssue/Delete/5
        [Authorize(Roles = "administrators")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            RequestIssue requestıssue = db.RequestIssues.Include(p => p.RequestIssueFiles).Include(p => p.Projects).Include(p => p.Locations).Include(p => p.Personnels).Include(p => p.CorporateAccounts).Where(i => i.RequestIssueID == id).SingleOrDefault();

            //logları siliyoruz
            try
            {
                foreach (LogRequestIssue logsingle in db.LogRequestIssues.Include(i => i.LogRequestIssueDetails).Where(i => i.RequestIssueID == id).ToList())
                {
                    var logdetail_list = logsingle.LogRequestIssueDetails.ToList();

                    foreach (LogRequestIssueDetail xlogdetail in logdetail_list)
                    {
                        db.LogRequestIssueDetails.Remove(xlogdetail);
                    }
                    db.LogRequestIssues.Remove(logsingle);
                    db.SaveChanges();
                }
            }
            catch
            {
            }

            //Projelerden manytomany tabloolardan cikariyoruz
            try
            {
                requestıssue.Locations.Clear();
                requestıssue.Personnels.Clear();
                requestıssue.CorporateAccounts.Clear();
                requestıssue.Projects.Clear();
                db.SaveChanges();
            }
            catch
            {
            }

            MembershipUser currentuser_ = new UserRepository().GetUser(User.Identity.Name);
            int user_wherecondition = int.Parse((currentuser_.ProviderUserKey).ToString());

            var xuser = db.Users.AsNoTracking().Where(i => i.UserId == user_wherecondition).Single();
            if (requestıssue.SendEmail == true)
            {
                string mailsonucstr = SendEmail(new MailAddress(requestıssue.User.Email), new MailAddress(requestıssue.UserReq.Email), "[Klimasan HelpDesk] #" + requestıssue.RequestIssueID.ToString() + " no'lu İş isteğiniz hakkında.", "İş İsteğiniz " + xuser.FullName + " silinmiştir. Tarih: " + DateTime.Now.ToString() + ". İyi çalışmalar dileriz.");
            }
            db.RequestIssues.Remove(requestıssue);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        [Authorize]
        internal static string SendEmail(MailAddress fromAddress, MailAddress toAddress, string subject, string body)
        {
            try
            {
                var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                };

                var client = new SmtpClient("KLMSNEVS.klimasan.msft");
                client.Credentials = new NetworkCredential("MUSAF@klimasan.com.tr", "1213");
                client.Send(message);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public ActionResult ExportXls(int page, string orderBy, string filter)
        {
            //Get the data representing the current grid state - page, sort and filter
            //GridModel model = Model().ToGridModel(page, 10, orderBy, string.Empty, filter);
            //var orders = model.Data.Cast<Order>();

            var requests = from r in db.RequestIssues select r; //.Where(i=>i.ValidationStateID==1).Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState);

            if (!(User.IsInRole("moderators") || User.IsInRole("administrators")))
            {
                MembershipUser currentuser_ = new UserRepository().GetUser(User.Identity.Name);
                int user_wherecondition = int.Parse((currentuser_.ProviderUserKey).ToString());
                requests = requests.Where(i => i.UserReqID == user_wherecondition);//.Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState);
            }
            else
            {
                requests = requests.Where(i => i.ValidationStateID == 1);//.Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState);
            }

            if (filter != null)
            {
                if (filter == "W")
                {
                    requests = requests.Where(i => i.IsApproved == false && i.UserID == null);
                }
                else if (filter == "P")
                {
                    requests = requests.Where(i => i.IsApproved == false && i.UserID != null);
                }
                else if (filter == "C")
                {
                    requests = requests.Where(i => i.IsApproved == true);
                }
            }

            requests = requests.Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Personnel).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState);

            //enter paging:
            //int pageSize = 15;
            //int pageIndex = page;
            //var rlist = requests.AsEnumerable().ToPagedList(pageIndex, pageSize);
            //Create new Excel workbook
            var workbook = new HSSFWorkbook();

            //Create new Excel sheet
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            //sheet.SetColumnWidth(0, 10 * 256);
            //sheet.SetColumnWidth(1, 50 * 256);
            //sheet.SetColumnWidth(2, 50 * 256);
            //sheet.SetColumnWidth(3, 50 * 256);

            //Create a header row
            var headerRow = sheet.CreateRow(0);

            //Set the column names in the header row
            headerRow.CreateCell(0).SetCellValue("İzlem No");
            headerRow.CreateCell(1).SetCellValue("Başlangıç Tarihi");
            headerRow.CreateCell(2).SetCellValue("İsteyen");
            headerRow.CreateCell(3).SetCellValue("Atölye");
            headerRow.CreateCell(4).SetCellValue("Departman");
            headerRow.CreateCell(5).SetCellValue("Sorun Açıklama");
            headerRow.CreateCell(6).SetCellValue("Durum");
            headerRow.CreateCell(7).SetCellValue("Sonuç Notu");
            headerRow.CreateCell(8).SetCellValue("İş Sahibi");
            headerRow.CreateCell(9).SetCellValue("Tamamlandı?");

            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);

            int rowNumber = 1;

            //Populate the sheet with values from the grid data
            foreach (RequestIssue rq in requests)
            {
                //Create a new row
                var row = sheet.CreateRow(rowNumber++);

                //Set values for the cells
                row.CreateCell(0).SetCellValue(rq.RequestIssueID);
                row.CreateCell(1).SetCellValue(rq.StartDate.ToString());
                row.CreateCell(2).SetCellValue(rq.UserReq.getFullName());
                row.CreateCell(3).SetCellValue(rq.Workshop.Description);
                row.CreateCell(4).SetCellValue(rq.Location.Description);
                row.CreateCell(5).SetCellValue(rq.DetailedDescription);
                row.CreateCell(6).SetCellValue(rq.RequestState.Description);
                row.CreateCell(7).SetCellValue(rq.Note);
                if (rq.UserID != null)
                {
                    row.CreateCell(8).SetCellValue(rq.User.getFullName());
                }
                else
                {
                    row.CreateCell(8).SetCellValue("");
                }
                row.CreateCell(9).SetCellValue(rq.IsApproved.ToString());
            }

            //Write the workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user

            return File(output.ToArray(),   //The binary data of the XLS file
                "application/vnd.ms-excel", //MIME type of Excel files
                "GridExcelExport.xls");     //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }

        [Authorize]
        public ActionResult Report(string custommerr)
        {
            ViewBag.RequestTypeID = new SelectList(db.RequestTypes, "RequestTypeID", "Description");
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description");
            ViewBag.LocationGroupID = new SelectList(db.LocationGroups, "LocationGroupID", "Description");
            ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "Description");
            ViewBag.PersonnelID = new SelectList(db.Personnels, "PersonnelID", "FullName");
            ViewBag.RequestTypeID = new SelectList(db.RequestTypes, "RequestTypeID", "Description");
            ViewBag.WorkshopID = new SelectList(db.Workshops, "WorkshopID", "Description", 1);
            ViewBag.RequestStateID = new SelectList(db.RequestStates, "RequestStateID", "Description");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FullName");
            List<IsApproveViewModel> appviewdropdown = new List<IsApproveViewModel>();

            appviewdropdown.Add(new IsApproveViewModel { value = true, Description = "Onaylanmışlar" });
            appviewdropdown.Add(new IsApproveViewModel { value = false, Description = "Onaysızlar" });

            ViewBag.IsApproved = new SelectList(appviewdropdown, "value", "Description");
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description");
            ViewBag.timestamp = DateTime.Now;
            if (!(string.IsNullOrEmpty(custommerr)))
            {
                ViewBag.CustomErr = custommerr;
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Report(RequestIssue requestıssue, FormCollection formcollection)
        {
            ReportDocument rptH = new ReportDocument();
            bool isProjectreport = false;

            try
            {
                if (formcollection["workload"].Length > 0)
                {
                    rptH.FileName = Server.MapPath("~/RDLC/WorkLoadByDepVsUserReport.rpt");
                }
            }
            catch
            {
                try
                {
                    if (formcollection["projects"].Length > 0)
                    {
                        rptH.FileName = Server.MapPath("~/RDLC/ProjectReport.rpt");
                        isProjectreport = true;
                    }
                }
                catch
                {
                    rptH.FileName = Server.MapPath("~/RDLC/RequestIssueReport.rpt");
                }
            }

            rptH.Refresh();
            //rptH.Load();

            var value = new ParameterDiscreteValue();

            //value.Value = requestıssue.WorkshopID;
            //rptH.ParameterFields["Atolye"].CurrentValues.Add(value);
            if (isProjectreport)
            {
                var projects_ = db.Projects.AsNoTracking().Include(p => p.Locations).Include(p => p.Personnels).Include(p => p.CorporateAccounts);

                //kriterleri farkli bir sekilde degerlendirmeyi deneyelim. (sonra hepsi tek bir hale getir!!!!)
                if (formcollection["LocationID"] != null)
                {
                    try
                    {
                        var loc_ = db.Locations.Include(p => p.Projects).Where(i => i.LocationID == requestıssue.LocationID).SingleOrDefault();
                        var loc_projs = loc_.Projects.Select(s => s.ProjectID).ToList();
                        projects_ = projects_.Where(u => loc_projs.Contains(u.ProjectID));
                    }
                    catch
                    {
                        if (formcollection["LocationGroupID"] != null)
                        {
                            try
                            {
                                int locgroup_wherecondition = int.Parse(formcollection["LocationGroupID"]);
                                var locs_ = db.Locations.Include(p => p.Projects).Where(i => i.LocationGroupID == locgroup_wherecondition);
                                List<int> locgroupproids = new List<int>();
                                foreach (Location loc_single in locs_)
                                {
                                    var loc_projs = loc_single.Projects.Select(s => s.ProjectID).ToList();

                                    foreach (int xpr in loc_projs)
                                    {
                                        locgroupproids.Add(xpr);
                                    }
                                }
                                projects_ = projects_.Where(u => locgroupproids.Contains(u.ProjectID));
                            }
                            catch { }
                        }
                    }
                }

                if (formcollection["PersonnelID"] != null)
                {
                    try
                    {
                        int pers_wherecondition = int.Parse(formcollection["PersonnelID"]);
                        var pers_ = db.Personnels.Include(p => p.Projects).Where(i => i.PersonnelID == pers_wherecondition).SingleOrDefault();
                        var pers_projs = pers_.Projects.Select(s => s.ProjectID).ToList();
                        projects_ = projects_.Where(u => pers_projs.Contains(u.ProjectID));
                    }
                    catch
                    { }
                }

                if (formcollection["UserID"] != null)
                {
                    try
                    {
                        int project_responsiblewherecondition = int.Parse(formcollection["UserID"]);
                        projects_ = projects_.Where(u => u.UserID == project_responsiblewherecondition);
                    }
                    catch { }
                }

                if (requestıssue.StartDate != null)
                {
                    projects_ = projects_.Where(u => u.StartDate >= requestıssue.StartDate);
                }

                if (requestıssue.EndDate != null)
                {
                    projects_ = projects_.Where(u => u.EndDate <= requestıssue.EndDate);
                }

                int x_index = 0;
                foreach (int pro_ in projects_.Select(i => i.ProjectID).ToList())
                {
                    value.Value = pro_;

                    rptH.ParameterFields["ProjectIDs"].CurrentValues.Add(value);
                    x_index++;
                }

                if (x_index == 0)
                {
                    return RedirectToAction("Report", new { custommerr = "Belirttiğiniz Kriterlere Uygun Kayıt(lar) Bulunamadı" });
                }
                /*
                rptH.SetDatabaseLogon("sa", "KLMSN_2007", @"DCVMSERVER/(local)", "HELPDESK", true);

                foreach (ReportDocument sub in rptH.Subreports)
                {
                    sub.SetDatabaseLogon("sa", "KLMSN_2007", @"DCVMSERVER/local", "HELPDESK", true);
                }*/
            }
            else
            {
                value.Value = requestıssue.LocationID;
                rptH.ParameterFields["Departman"].CurrentValues.Add(value);

                value.Value = requestıssue.RequestStateID ?? 0;

                rptH.ParameterFields["IsDurum"].CurrentValues.Add(value);

                value.Value = requestıssue.RequestTypeID;
                rptH.ParameterFields["IsTip"].CurrentValues.Add(value);

                value.Value = formcollection["LocationGroupID"];
                if (value.Value.ToString() != "")
                {
                    rptH.ParameterFields["AnaDepartman"].CurrentValues.Add(value);
                }
                else
                {
                    value.Value = 0;
                    rptH.ParameterFields["AnaDepartman"].CurrentValues.Add(value);
                }

                value.Value = formcollection["PersonnelID"].ToString();
                if (value.Value.ToString() != "")
                {
                    rptH.ParameterFields["IsIsteyenPersonel"].CurrentValues.Add(value);
                }
                else
                {
                    value.Value = 0;
                    rptH.ParameterFields["IsIsteyenPersonel"].CurrentValues.Add(value);
                }

                value.Value = formcollection["UserID"].ToString();
                if (value.Value.ToString() != "")
                {
                    rptH.ParameterFields["IsSahibi"].CurrentValues.Add(value);
                }
                else
                {
                    value.Value = 0;
                    rptH.ParameterFields["IsSahibi"].CurrentValues.Add(value);
                }

                value.Value = formcollection["IsApproved"].ToString();
                if (value.Value.ToString() != "")
                {
                    rptH.ParameterFields["OnayDurum"].CurrentValues.Add(value);
                }
                else
                {
                    value.Value = "yok";
                    rptH.ParameterFields["OnayDurum"].CurrentValues.Add(value);
                }

                if (requestıssue.StartDate != null)
                {
                    value.Value = requestıssue.StartDate;
                    rptH.ParameterFields["StartDate"].CurrentValues.Add(value);
                }
                else
                {
                    value.Value = DateTime.Today.AddYears(-20);
                    rptH.ParameterFields["StartDate"].CurrentValues.Add(value);
                }

                if (requestıssue.EndDate != null)
                {
                    value.Value = requestıssue.EndDate;
                    rptH.ParameterFields["EndDate"].CurrentValues.Add(value);
                }
                else
                {
                    value.Value = DateTime.Today.AddYears(20);
                    rptH.ParameterFields["EndDate"].CurrentValues.Add(value);
                }
            }
            // rptH.SetDataSource([datatable]);
            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = "rapor_klimasanHelpDesk.pdf",

                // always prompt the user for downloading, set to true if you want
                // the browser to try to show the file inline
                Inline = false,
            };

            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(stream, "application/pdf");
        }

        private RequestIssueFile RetrieveFileFromRequest(int index)
        {
            string filename = null;
            string fileType = null;
            byte[] fileContents = null;

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[index];
                fileContents = new byte[file.ContentLength];
                Request.Files[index].InputStream.Read(fileContents, 0, file.ContentLength);
                fileType = file.ContentType;
                filename = file.FileName;
            }

            int Position = filename.LastIndexOf("\\");
            filename = filename.Substring(Position + 1);

            return new RequestIssueFile()
            {
                RequestIssueFileName = filename,
                RequestIssueFileContentType = fileType,
                RequestIssueFileSize = fileContents != null ? fileContents.Length : 0,
                RequestIssueFileContents = fileContents
            };
        }

        [Authorize]
        public ActionResult DownloadRequestIssueFile(int id)
        {
            var _file = db.RequestIssueFiles.Find(id);
            byte[] _filedata = (byte[])_file.RequestIssueFileContents;
            return File(_filedata, _file.RequestIssueFileContentType, _file.RequestIssueFileName);
        }

        [Authorize]
        public ActionResult DeleteRequestIssueFile(int id, int rqid, string show, int page)
        {
            RequestIssueFile requestfile = db.RequestIssueFiles.Find(id);
            db.RequestIssueFiles.Remove(requestfile);
            db.SaveChanges();

            return RedirectToAction("Edit", new { id = rqid, show = show, page = page });
        }
    }
}