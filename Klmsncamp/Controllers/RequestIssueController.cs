using System;
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
                return RedirectToAction("Edit", new { id = rq.RequestIssueID });
            }
            catch
            {
                return Redirect("/?err=404");
            }
        }

        [Authorize]
        public ViewResult Index(string show, int? page)
        {
            var requests = from r in db.RequestIssues select r; //.Where(i=>i.ValidationStateID==1).Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState);

            if (!(User.IsInRole("moderators") || User.IsInRole("administrators")))
            {
                MembershipUser currentuser_ = new UserRepository().GetUser(User.Identity.Name);
                int user_wherecondition = int.Parse((currentuser_.ProviderUserKey).ToString());
                requests = requests.Where(i => i.UserReqID == user_wherecondition);//.Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState);
            }

            //else
            //{
            //    requests = requests.Where(i => i.ValidationStateID == 1);//.Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState);
            //}

            if (show != null)
            {
                if (show == "W")
                {
                    requests = requests.Where(i => i.IsApproved == false && i.UserID == null);
                }
                else if (show == "P")
                {
                    requests = requests.Where(i => i.IsApproved == false && i.UserID != null);
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
            requests = requests.Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Personnel).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState);

            //enter paging:
            int pageSize = 15;
            int pageIndex = (page ?? 1);
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

            if ((requestissue.UserReq.UserName == User.Identity.Name) && (requestissue.ValidationStateID != 1))
            {
                requestissue.ValidationStateID = 1;
                db.Entry(requestissue).State = EntityState.Modified;
                db.SaveChanges();

                if (requestissue.SendEmail == true)
                {
                    MembershipUser currentuser_ = new UserRepository().GetUser(User.Identity.Name);
                    string mailsonucstr = SendEmail(new MailAddress("musaf@klimasan.com.tr"), new MailAddress(currentuser_.Email), "[Klimasan HelpDesk] İş isteğiniz hakkında.", "İsteğiniz doğrulanarak kayıt altına alınmıştır.Tarih: " + DateTime.Now.ToString() + " - İş No: #" + (requestissue.RequestIssueID).ToString() + ". İyi çalışmalar dileriz.");
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
        public ActionResult Create()
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
            ViewBag.timestamp = DateTime.Now;
            return View();
        }

        //
        // POST: /RequestIssue/Create

        [Authorize]
        [HttpPost]
        public ActionResult Create(RequestIssue requestıssue)
        {
            if (ModelState.IsValid)
            {
                requestıssue.TimeStamp = DateTime.Now;
                requestıssue.ValidationStateID = 2;
                db.RequestIssues.Add(requestıssue);
                db.SaveChanges();
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
            return View(requestıssue);
        }

        //
        // GET: /RequestIssue/Edit/5
        [Authorize]
        public ActionResult Edit(int id, string show, int page)
        {
            RequestIssue requestıssue = db.RequestIssues.Find(id);

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
                return RedirectToAction("Validate", new { id = requestıssue.RequestIssueID, show = formCollection["show"], page = formCollection["page"] });
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
            ViewBag.show = formCollection["show"];
            ViewBag.page = formCollection["page"];
            return View(requestıssue);
        }

        [Authorize]
        public ActionResult Editp(int id, string show, int page)
        {
            RequestIssue rq = db.RequestIssues.Find(id);
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
                if (rqToUpdate.IsApproved)
                {
                    rqToUpdate.RequestStateID = 5;
                }
                db.Entry(rqToUpdate).State = EntityState.Modified;

                db.SaveChanges();
                bool xdurum = KlmsnExtensions.BoolPropertyDifferences(rqDBrecord, rqToUpdate);

                if (rqToUpdate.SendEmail == true && xdurum == true)
                {
                    string mailsonucstr = SendEmail(new MailAddress("musaf@klimasan.com.tr"), new MailAddress(rqDBrecord.UserReq.Email), "[Klimasan HelpDesk] #" + rqToUpdate.RequestIssueID.ToString() + " no'lu İş isteğiniz hakkında.", "İş İsteğiniz güncellenmiştir. İsteğinizin son durumu görmek isterseniz; http:/127.0.0.1:43970/RequestIssue/Editp/" + rqToUpdate.RequestIssueID.ToString() + " adresini ziyaret ediniz. Tarih: " + DateTime.Now.ToString() + ". İyi çalışmalar dileriz.");
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
            RequestIssue requestıssue = db.RequestIssues.Find(id);
            MembershipUser currentuser_ = new UserRepository().GetUser(User.Identity.Name);
            int user_wherecondition = int.Parse((currentuser_.ProviderUserKey).ToString());

            var xuser = db.Users.AsNoTracking().Where(i => i.UserId == user_wherecondition).Single();
            if (requestıssue.SendEmail == true)
            {
                string mailsonucstr = SendEmail(new MailAddress("musaf@klimasan.com.tr"), new MailAddress(requestıssue.UserReq.Email), "[Klimasan HelpDesk] #" + requestıssue.RequestIssueID.ToString() + " no'lu İş isteğiniz hakkında.", "İş İsteğiniz " + xuser.FullName + " silinmiştir. Tarih: " + DateTime.Now.ToString() + ". İyi çalışmalar dileriz.");
            }
            db.RequestIssues.Remove(requestıssue);
            db.SaveChanges();

            LogRequestIssue mylog = new LogRequestIssue { RequestIssueID = id, Action = "Silme", ModifyTime = DateTime.Now, UserID = user_wherecondition };
            db.LogRequestIssues.Add(mylog);
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
                client.Credentials = new NetworkCredential("MUSAF@klimasan.com.tr", "1212");
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

        public ActionResult Report()
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
            return View();
        }

        [HttpPost]
        public ActionResult Report(RequestIssue requestıssue, FormCollection formcollection)
        {
            ReportClass rptH = new ReportClass();
            try
            {
                if (formcollection["workload"].Length > 0)
                {
                    rptH.FileName = Server.MapPath("~/RDLC/WorkLoadByDepVsUserReport.rpt");
                }
            }
            catch
            {
                rptH.FileName = Server.MapPath("~/RDLC/RequestIssueReport.rpt");
            }

            rptH.Load();

            var value = new ParameterDiscreteValue();

            //value.Value = requestıssue.WorkshopID;
            //rptH.ParameterFields["Atolye"].CurrentValues.Add(value);

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
            value.Value = requestıssue.StartDate;
            rptH.ParameterFields["StartDate"].CurrentValues.Add(value);

            value.Value = requestıssue.EndDate ?? DateTime.Today.AddYears(20);
            rptH.ParameterFields["EndDate"].CurrentValues.Add(value);

            // rptH.SetDataSource([datatable]);
            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
    }
}