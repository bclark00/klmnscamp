using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
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

        private static Random random = new Random((int)DateTime.Now.Ticks);//thanks to McAden
        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

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


            /*if (!(User.IsInRole("moderators") || User.IsInRole("administrators")))
            {
                requests = requests.Where(i => i.UserReqID == user_wherecondition);//.Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState);
            }
            */
            User theuser_ = db.Users.Include(p => p.WorkshopPermissions).Where(i => i.UserId == user_wherecondition).SingleOrDefault();
            var theuser_wrps = theuser_.WorkshopPermissions.Where(i=>i.Select==true).Select(s => s.WorkshopID).ToList();
            
            requests = requests.Where(u => theuser_wrps.Contains(u.WorkshopID));
            
            
            if (!(new UserRepository().HasPerm(user_wherecondition, "ana_birim_tum_isleri_goruntuleyebilir")))
            {
                requests = requests.Where(i => i.UserReqID == user_wherecondition || i.UserID== user_wherecondition);
            }
            

            //sadece kendilerinin taslaklari + valideler
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

            //parametrelerim
            string xincompletedstatestr_ = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 6).SingleOrDefault().ParameterValue;
            int xincompletedstateint_ = int.Parse(xincompletedstatestr_);
            string xcompletedstatestr_ = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 5).SingleOrDefault().ParameterValue;
            int xcompletedstateint_ = int.Parse(xcompletedstatestr_);
            ViewBag.CompletedStateID = xcompletedstateint_;
            ViewBag.InCompletedStateID = xincompletedstateint_;

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

                    string softwaretitle = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 14).SingleOrDefault().ParameterValue;

                    string fromemailaddress = db.ParameterSettings.AsNoTracking().Where(i=> i.ParameterSettingID == 15).SingleOrDefault().ParameterValue;
                    try
                    {
                        fromemailaddress = user_from.Email;
                    }
                    catch
                    {
                    }


                    string mailsonucstr = SendEmail(new MailAddress(fromemailaddress), new MailAddress(currentuser_.Email), "[" + softwaretitle + "] İş isteğiniz hakkında.", "İsteğiniz doğrulanarak kayıt altına alınmıştır.Tarih: " + DateTime.Now.ToString() + " - İş No: #" + (requestissue.RequestIssueID).ToString() + ". İyi çalışmalar dileriz.", requestissue.Personnel.Email, false,requestissue.RequestIssueID);
                    
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
            ViewBag.RequestActualReasonID = new SelectList(db.RequestActualReasons, "RequestActualReasonID", "Description");
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description");

            //bagli personel bilgisi var mi?
            int xcount = db.Personnels.Where(i => i.ValidationStateID==1 && i.UserID == user_wherecondition).ToList().Count;
            
            if (xcount > 0)
            {
                ViewBag.PersonnelID = new SelectList(db.Personnels.Where(s => s.ValidationStateID == 1), "PersonnelID", "FullName", db.Personnels.Where(i => i.UserID == user_wherecondition).SingleOrDefault().PersonnelID);
            }
            else
            {
                ViewBag.PersonnelID = new SelectList(db.Personnels.Where(s => s.ValidationStateID == 1), "PersonnelID", "FullName");
            }


            User theuser_ = db.Users.Include(p => p.WorkshopPermissions).Where(i => i.UserId == user_wherecondition).SingleOrDefault();
            var theuser_wrps = theuser_.WorkshopPermissions.Where(i => i.Insert == true).Select(s => s.WorkshopID).ToList();

            ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "Description");
            ViewBag.WorkshopID = new SelectList(db.Workshops.Where(u=> theuser_wrps.Contains(u.WorkshopID)), "WorkshopID", "Description");
            ViewBag.RequestStateID = new SelectList(db.RequestStates, "RequestStateID", "Description", 1);
            ViewBag.UserReqID = new SelectList(db.Users.Where(x => x.UserId == user_wherecondition), "UserId", "FullName", currentuser_.ProviderUserKey);
            ViewBag.UserID = new SelectList(db.Users, "UserId", "FullName", currentuser_.ProviderUserKey);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description");
            
            string multipleworkshops = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 16).SingleOrDefault().ParameterValue;

            if (multipleworkshops == "1")
            {
                ViewBag.MultipleWorkshops = true;
            }
            else
            {
                ViewBag.MultipleWorkshops = false;
            }

            
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
            ViewBag.RequestActualReasonID = new SelectList(db.RequestActualReasons, "RequestActualReasonID", "Description");
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description", requestıssue.LocationID);
            ViewBag.PersonnelID = new SelectList(db.Personnels.Where(s => s.ValidationStateID == 1), "PersonnelID", "FullName", requestıssue.PersonnelID);
            ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "Description", requestıssue.InventoryID);

            User theuser_ = db.Users.Include(p => p.WorkshopPermissions).Where(i => i.UserId == user_wherecondition).SingleOrDefault();
            var theuser_wrps = theuser_.WorkshopPermissions.Where(i => i.Insert == true).Select(s => s.WorkshopID).ToList();

            ViewBag.WorkshopID = new SelectList(db.Workshops.Where(u=> theuser_wrps.Contains(u.WorkshopID)), "WorkshopID", "Description", requestıssue.WorkshopID);

            ViewBag.RequestStateID = new SelectList(db.RequestStates, "RequestStateID", "Description", requestıssue.RequestStateID);
            ViewBag.UserReqID = new SelectList(db.Users.Where(x => x.UserId == user_wherecondition), "UserId", "FullName", requestıssue.UserReqID);
            ViewBag.UserID = new SelectList(db.Users, "UserId", "FullName", requestıssue.UserID);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", requestıssue.ValidationStateID);

            string multipleworkshops = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 16).SingleOrDefault().ParameterValue;

            if (multipleworkshops == "1")
            {
                ViewBag.MultipleWorkshops = true;
            }
            else
            {
                ViewBag.MultipleWorkshops = false;
            }

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
            ViewBag.RequestActualReasonID = new SelectList(db.RequestActualReasons, "RequestActualReasonID", "Description", requestıssue.RequestActualReasonID);
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

            string multipleworkshops = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 16).SingleOrDefault().ParameterValue;

            if (multipleworkshops == "1")
            {
                ViewBag.MultipleWorkshops = true;
            }
            else
            {
                ViewBag.MultipleWorkshops = false;
            }

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
            ViewBag.RequestActualReasonID = new SelectList(db.RequestActualReasons, "RequestActualReasonID", "Description", requestıssue.RequestActualReasonID);
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

            string multipleworkshops = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 16).SingleOrDefault().ParameterValue;

            if (multipleworkshops == "1")
            {
                ViewBag.MultipleWorkshops = true;
            }
            else
            {
                ViewBag.MultipleWorkshops = false;
            }

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
                ViewBag.RequestActualReasonID = new SelectList(db.RequestActualReasons, "RequestActualReasonID", "Description", rq.RequestActualReasonID);
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
                ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", rq.ValidationStateID);
            
                string multipleworkshops = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 16).SingleOrDefault().ParameterValue;

                if (multipleworkshops == "1")
                {
                    ViewBag.MultipleWorkshops = true;
                }
                else
                {
                    ViewBag.MultipleWorkshops = false;
                }

                ViewBag.show = show;
                ViewBag.page = page;

                MembershipUser currentuser_ = new UserRepository().GetUser(User.Identity.Name);
                int user_wherecondition = int.Parse((currentuser_.ProviderUserKey).ToString());

                if (User.IsInRole("administrators") || User.IsInRole("moderators"))
                {
                    ViewBag.UserID = new SelectList(db.Users, "UserId", "FullName", rq.UserID);
                }
                else
                {
                    
                    ViewBag.UserID = new SelectList(db.Users.Where(i => i.UserId == user_wherecondition), "UserId", "FullName", rq.UserID);
                }

                //ozel yetkiler
                User theuser_ = db.Users.AsNoTracking().Include(p => p.WorkshopPermissions).Where(i => i.UserId == user_wherecondition).SingleOrDefault();
                WorkshopPermission thewrp_ = theuser_.WorkshopPermissions.Where(i => i.WorkshopID == rq.WorkshopID).SingleOrDefault();

                ViewBag.UpdatePermission = thewrp_.Update;

                ViewBag.ApprovePermission = thewrp_.Approve;

                if (!(new UserRepository().HasPerm(user_wherecondition, "is_bildiriminde_eklenmis_dosyalari_duzenleyebilir")))
                {
                    ViewBag.FileEditPermission = false;
                }
                else
                {
                    ViewBag.FileEditPermission = true;
                }
                //ozel yetkiler bitis

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

                //anketleri göstermece
                IList<SurveyTable> MySurveys = new List<SurveyTable>();
                foreach (SurveyTable surveytable_item in db.SurveyTables.Where(s => s.RequestIssueID == id).ToList())
                {
                    if (surveytable_item.IsApproved)
                    {
                        MySurveys.Add(surveytable_item);
                    }
                }
                if (MySurveys.ToList().Count > 0)
                {
                    ViewBag.TheSurveys = MySurveys;
                    ViewBag.HasSurvey = true;
                }
                else
                {
                    ViewBag.HasSurvey = false;
                }
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

            User theuser_ = db.Users.AsNoTracking().Include(p => p.WorkshopPermissions).Where(i => i.UserId == user_wherecondition).SingleOrDefault();
            WorkshopPermission thewrp_ = theuser_.WorkshopPermissions.Where(i => i.WorkshopID == rqDBrecord.WorkshopID).SingleOrDefault();

            if (!thewrp_.Update)
            {
                return RedirectToAction("Index", new { show = formCollection["show"], page = formCollection["page"] });
            }


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

            if (rqToUpdate.IsApproved == true && rqDBrecord.IsApproved == true)
            {
                return RedirectToAction("Index", new { show = formCollection["show"], page = formCollection["page"] });
            }

            //bool _enddatepostpone = CheckEndDateIfCanBeUpdated(rqToUpdate);

            if (ModelState.IsValid)
            {
                //durum parameterlerim
                string xincompletedstatestr_ = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 6).SingleOrDefault().ParameterValue;
                int xincompletedstateint_ = int.Parse(xincompletedstatestr_);

                string xcompletedstatestr_ = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 5).SingleOrDefault().ParameterValue;
                int xcompletedstateint_ = int.Parse(xcompletedstatestr_);

                if (rqToUpdate.IsApproved && rqToUpdate.RequestStateID != xincompletedstateint_)
                {
                    rqToUpdate.RequestStateID = xcompletedstateint_;
                }

                rqToUpdate.mTimeStamp = DateTime.Now;

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
                    Personnel pers_from = db.Personnels.AsNoTracking().Where(b => b.PersonnelID == rqToUpdate.PersonnelID).SingleOrDefault();

                    string softwaretitle = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 14).SingleOrDefault().ParameterValue;

                    string fromemailaddress = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 15).SingleOrDefault().ParameterValue;
                    try
                    {
                        fromemailaddress = user_from.Email;
                    }
                    catch
                    {
                    }

                    string mailsonucstr = SendEmail(new MailAddress(fromemailaddress), new MailAddress(userReq_to.Email), "[" + softwaretitle + "] #" + rqToUpdate.RequestIssueID.ToString() + " no'lu İş isteğiniz hakkında.", "İş İsteğiniz güncellenmiştir. İsteğinizin son durumu görmek isterseniz;  " + Url.Action("Editp", "RequestIssue", new { id = rqToUpdate.RequestIssueID, show = "A", page = 1 }, "http") + " adresini ziyaret ediniz. Tarih: " + DateTime.Now.ToString() + ". İyi çalışmalar dileriz.", pers_from.Email, false,rqToUpdate.RequestIssueID);
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

                //anket olustursun eger onayli ve anket sistemi aktif ve ebadan geliyorsa
                if (rqToUpdate.IsApproved)
                {
                    string issurveryactive_ = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 7).SingleOrDefault().ParameterValue;
                    if (issurveryactive_ == "1")
                    {
                        string ebauserstr_ = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 9).SingleOrDefault().ParameterValue;
                        int ebauserint_ = int.Parse(ebauserstr_);

                        if (rqToUpdate.UserReqID == ebauserint_)
                        {
                            bool xrejected = false;
                            //olumsuz kapanmis mi?
                            if (rqToUpdate.RequestStateID == xincompletedstateint_)
                            {
                                xrejected = true;
                            }
                            CreateSurvey(rqToUpdate.PersonnelID.Value, rqToUpdate.RequestTypeID, rqToUpdate.RequestIssueID, rqToUpdate.DetailedDescription, rqToUpdate.TimeStamp, xrejected);
                        }
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
            ViewBag.RequestActualReasonID = new SelectList(db.RequestActualReasons, "RequestActualReasonID", "Description", rqToUpdate.RequestActualReasonID);
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
            
            string multipleworkshops = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 16).SingleOrDefault().ParameterValue;

            if (multipleworkshops == "1")
            {
                ViewBag.MultipleWorkshops = true;
            }
            else
            {
                ViewBag.MultipleWorkshops = false;
            }

            ViewBag.show = formCollection["show"];
            ViewBag.page = formCollection["page"];

            //ozel yetkiler
           
            ViewBag.UpdatePermission = thewrp_.Update;

            ViewBag.ApprovePermission = thewrp_.Approve;

            if (!(new UserRepository().HasPerm(user_wherecondition, "is_bildiriminde_eklenmis_dosyalari_duzenleyebilir")))
            {
                ViewBag.FileEditPermission = false;
            }
            else
            {
                ViewBag.FileEditPermission = true;
            }
            //ozelyetkiler bitiş


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

            //anketleri göstermece
            IList<SurveyTable> MySurveys = new List<SurveyTable>();
            foreach (SurveyTable surveytable_item in db.SurveyTables.Where(s => s.RequestIssueID == id).ToList())
            {
                if (surveytable_item.IsApproved)
                {
                    MySurveys.Add(surveytable_item);
                }
            }
            if (MySurveys.ToList().Count > 0)
            {
                ViewBag.TheSurveys = MySurveys;
                ViewBag.HasSurvey = true;
            }
            else
            {
                ViewBag.HasSurvey = false;
            }
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

            //anketleri siliyoruz
            try
            {
                foreach (SurveyTable st in db.SurveyTables.Where(i => i.RequestIssueID == id).ToList())
                {
                    db.SurveyTables.Remove(st);
                    db.SaveChanges();
                }
            }
            catch
            { }

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
                string softwaretitle = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 14).SingleOrDefault().ParameterValue;

                string fromemailaddress = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 15).SingleOrDefault().ParameterValue;
                try
                {
                    fromemailaddress = requestıssue.User.Email;
                }
                catch
                {
                }

                string mailsonucstr = SendEmail(new MailAddress(fromemailaddress), new MailAddress(requestıssue.UserReq.Email), "[" + softwaretitle + "] #" + requestıssue.RequestIssueID.ToString() + " no'lu İş isteğiniz hakkında.", "İş İsteğiniz " + xuser.FullName + " silinmiştir. Tarih: " + DateTime.Now.ToString() + ". İyi çalışmalar dileriz.", requestıssue.Personnel.Email, false,requestıssue.RequestIssueID);
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
        public void CreateSurvey(int xpersID, int xrequesttypeID, int xrequestissueID, string xrequestissueDesc, DateTime xtimestamp, bool isreject)
        {
            int survtempid_ = 1;
            try
            {
                Personnel pers_ = db.Personnels.AsNoTracking().Where(i => i.PersonnelID == xpersID).SingleOrDefault();
                RequestType reqtype_ = db.RequestTypes.AsNoTracking().Where(i => i.RequestTypeID == xrequesttypeID).SingleOrDefault();
                string longdescription = xtimestamp.ToLongDateString() + " tarihli #" + xrequestissueID.ToString() + " No'lu, " + reqtype_.Description.ToLower() + " tipindeki iş talebinize yönelik Anket.";
                if (pers_.Email != null)
                {
                    //eğer arıza tipine bir anket acilmissa ! yok degilse parametre default olan
                    try
                    {
                        survtempid_ = db.SurveyTemplates.Include(u => u.SurveyRecords).Where(i => i.RequestTypeID == xrequesttypeID && i.PreDefined == true).SingleOrDefault().SurveyTemplateID;
                    }
                    catch
                    {
                        string survtempidstr_ = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 8).SingleOrDefault().ParameterValue;
                        survtempid_ = int.Parse(survtempidstr_);
                    }

                    var mastersurvtemp = db.SurveyTemplates.Include(u => u.SurveyRecords).Where(i => i.SurveyTemplateID == survtempid_ && i.PreDefined == true).SingleOrDefault();

                    List<SurveyRecord> survrecs = new List<SurveyRecord>();

                    if (isreject)
                    {
                        string rejectedsurveynodestr_ = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 10).SingleOrDefault().ParameterValue;
                        int rejectedsurveynodeint_ = int.Parse(rejectedsurveynodestr_);
                        SurveyRecord mysurveyrec = new SurveyRecord { SurveyNodeID = rejectedsurveynodeint_, SurveyRecordTypeID = 2, OrderNum = 1, ApprovalStatus = false, Score = null, Note = null };
                        survrecs.Add(mysurveyrec);
                        db.SurveyRecords.Add(mysurveyrec);
                    }
                    else
                    {
                        foreach (SurveyRecord sr_ in mastersurvtemp.SurveyRecords.ToList())
                        {
                            SurveyRecord mysurveyrec = new SurveyRecord { SurveyNodeID = sr_.SurveyNodeID, SurveyRecordTypeID = sr_.SurveyRecordTypeID, OrderNum = sr_.OrderNum, ApprovalStatus = sr_.ApprovalStatus, Score = sr_.Score, Note = sr_.Note };
                            survrecs.Add(mysurveyrec);
                            db.SurveyRecords.Add(mysurveyrec);
                        }
                    }
                    db.SaveChanges();

                    SurveyTemplate mysurvtemp = new SurveyTemplate()
                    {
                        Description = longdescription,
                        PreDefined = false,
                        RequestTypeID = xrequesttypeID,
                        SurveyRecords = survrecs
                    };
                    db.SurveyTemplates.Add(mysurvtemp);
                    db.SaveChanges();

                    SurveyTable mysurvey = new SurveyTable()
                    {
                        RequestIssueID = xrequestissueID,
                        Description = longdescription,
                        TimeStamp = DateTime.Now,
                        SurveyTemplateID = mysurvtemp.SurveyTemplateID,
                        IsApproved = false,
                        HashKey = RandomString(20)
                    };
                    db.SurveyTables.Add(mysurvey);
                    db.SaveChanges();

                    string defemailaddparam_ = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 15).SingleOrDefault().ParameterValue;
                    string softwaretitle = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 14).SingleOrDefault().ParameterValue;
                    string maildurum_ = SendEmail(new MailAddress(defemailaddparam_), new MailAddress(pers_.Email), "[" + softwaretitle + "] #" + xrequestissueID.ToString() + " no'lu İş isteğiniz hakkında.", "İş Talep/İsteğiniz tamamlanmış ve memnuniyet anketi oluşturulmuştur. " + "\n" + "Anketi Doldurmak için; " + Url.Action("Edit", "SurveyTable", new { id = mysurvey.SurveyTableID }, "http") + " adresini ziyaret ediniz. " + "\n" + "Anket Şifresi : " + mysurvey.HashKey + " \n" + "Tarih: " + DateTime.Now.ToString() + ". İyi çalışmalar dileriz.", null, false,xrequestissueID);
                }
            }
            catch
            {
            }
        }

        
        [Authorize(Roles = "administrators,moderators")]
        public ActionResult Search()
        {
            var requests = from r in db.RequestIssues select r; //.Where(i=>i.ValidationStateID==1).Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState);
            requests = requests.Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Personnel).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState).OrderByDescending(o => o.RequestIssueID);
            var rlist = requests.Where(i=>i.RequestIssueID==-1).AsEnumerable();

            ViewBag.UserID = new SelectList(db.Users, "UserId", "FullName");

            ViewBag.CurrentShow = "A";
            ViewBag.CurrentPage = 1;

            return View(rlist);
        }

        [Authorize(Roles = "administrators, moderators")]
        [HttpPost]
        public ActionResult Search(FormCollection formcollection)
        {
            bool hasDateCrit = true;
            DateTime datecriteria_ = DateTime.Now;
            try
            {
               datecriteria_ = DateTime.Parse(formcollection["TargetDate"]).AddDays(1);
            }
            catch { hasDateCrit = false; }

            var requests = from r in db.RequestIssues select r;

            if (hasDateCrit)
            {
                requests = from r in db.RequestIssues where r.EndDate < datecriteria_ select r; //.Where(i=>i.ValidationStateID==1).Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState);
            }
            

            requests = requests.Where(u =>u.IsApproved == false);

            try
            {
                if (int.Parse(formcollection["UserID"]) > 0)
                {
                    int usercrit_ = int.Parse(formcollection["UserID"]);
                    requests = requests.Where(i => i.UserID.Value == usercrit_);

                    ViewBag.UserID = new SelectList(db.Users, "UserId", "FullName", usercrit_);
                }
                else
                {

                    ViewBag.UserID = new SelectList(db.Users, "UserId", "FullName");
                }
            }
            catch
            {

                ViewBag.UserID = new SelectList(db.Users, "UserId", "FullName");
            }

            

            if (!string.IsNullOrEmpty(formcollection["SearchString"].Trim()))
            {
               var searchcrit_ = formcollection["SearchString"].Trim().ToLower();
               requests = requests.Where(i => i.DetailedDescription.ToLower().Contains(searchcrit_) || i.Note.ToLower().Contains(searchcrit_));
            }


            requests = requests.Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Personnel).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState).OrderByDescending(o => o.EndDate);

            var rlist = requests.AsEnumerable();

            ViewBag.FilterString = formcollection["SearchString"].ToString();

            ViewBag.CurrentShow = "A";
            ViewBag.CurrentPage = 1;

            return View(rlist);
        }

        [Authorize]
        public string SendEmail(MailAddress fromAddress, MailAddress toAddress, string subject, string body, string fromPersonnel, bool fromEBA,int xrqID)
        {
            try
            {
                var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                };

                try
                {
                    message.CC.Add(new MailAddress(fromPersonnel));
                }
                catch { }

                string addProjectPplonCC_ = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 17).SingleOrDefault().ParameterValue;

                if (addProjectPplonCC_ == "1")
                {
                    var projectppl_ = db.RequestIssues.AsNoTracking().Include(p => p.Personnels).Where(i=>i.RequestIssueID==xrqID).SingleOrDefault().Personnels.ToList();

                    foreach (Personnel pppl_ in projectppl_)
                    {
                        if (pppl_.Email!=null)
                        {
                            message.CC.Add(new MailAddress(pppl_.Email));
                        }
                    }
                }
                string mailaccount_ = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 1).SingleOrDefault().ParameterValue;
                string mailpassword_ = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 2).SingleOrDefault().ParameterValue;
                string mailhost_ = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 3).SingleOrDefault().ParameterValue;

                var client = new SmtpClient(mailhost_);
                try
                {
                    string mailhostport_ = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 4).SingleOrDefault().ParameterValue;
                    int mailhostportint_ = int.Parse(mailhostport_);
                    client.Port = mailhostportint_;
                }
                catch
                {
                }
                client.Credentials = new NetworkCredential(mailaccount_, mailpassword_);
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
                    try
                    {
                        if (formcollection["matrix"].Length > 0)
                        {
                            rptH.FileName = Server.MapPath("~/RDLC/RequestIssueMatrixReport.rpt");
                        }
                    }
                    catch
                    {
                        rptH.FileName = Server.MapPath("~/RDLC/RequestIssueReport.rpt");
                    }
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