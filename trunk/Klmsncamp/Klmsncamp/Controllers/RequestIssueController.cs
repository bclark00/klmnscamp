using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.Net.Mail;
using Klmsncamp.Models;

namespace Klmsncamp.Controllers
{ 
    public class RequestIssueController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /RequestIssue/

        public ViewResult Index()
        {
            var requestıssues = db.RequestIssues.Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState);
            return View(requestıssues.ToList());
        }

        //
        // GET: /RequestIssue/Details/5

        public ViewResult Details(int id)
        {
            RequestIssue requestıssue = db.RequestIssues.Find(id);
            return View(requestıssue);
        }

        [Authorize]
        public ActionResult Validate(int id)
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
                    SendEmail(new MailAddress("musaf@klimasan.com.tr"), new MailAddress(currentuser_.Email), "[Klimasan HelpDesk] İş isteğiniz hakkında.", "İsteğiniz doğrulanarak kayıt altına alınmıştır.Tarih: "+DateTime.Now.ToString()+" - İş No: #" + (requestissue.RequestIssueID).ToString() + ". İyi çalışmalar dileriz.");
                }
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.UserError = "Bu İş için doğrulama için yetkisi yok ya da zaten doğrulanmış.";
            }
            return RedirectToAction("Index");
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
            ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "Description");
            ViewBag.WorkshopID = new SelectList(db.Workshops, "WorkshopID", "Description");
            ViewBag.RequestStateID = new SelectList(db.RequestStates, "RequestStateID", "Description",1);
            ViewBag.UserReqID = new SelectList(db.Users.Where(x => x.UserId == user_wherecondition), "UserId", "UserName", currentuser_.ProviderUserKey);
            ViewBag.UserID = new SelectList(db.Users, "UserId", "UserName",currentuser_.ProviderUserKey);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description");
            ViewBag.timestamp = DateTime.Now;
            return View();
        } 

        //
        // POST: /RequestIssue/Create

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
            ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "Description", requestıssue.InventoryID);
            ViewBag.WorkshopID = new SelectList(db.Workshops, "WorkshopID", "Description", requestıssue.WorkshopID);
            ViewBag.RequestStateID = new SelectList(db.RequestStates, "RequestStateID", "Description", requestıssue.RequestStateID);
            ViewBag.UserReqID = new SelectList(db.Users.Where(x => x.UserId == user_wherecondition), "UserId", "UserName", requestıssue.UserReqID);
            ViewBag.UserID = new SelectList(db.Users, "UserId", "UserName", requestıssue.UserID);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", requestıssue.ValidationStateID);
            return View(requestıssue);
        }
        
        //
        // GET: /RequestIssue/Edit/5
 
        public ActionResult Edit(int id)
        {
            RequestIssue requestıssue = db.RequestIssues.Find(id);
            ViewBag.RequestTypeID = new SelectList(db.RequestTypes, "RequestTypeID", "Description", requestıssue.RequestTypeID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description", requestıssue.LocationID);
            ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "Description", requestıssue.InventoryID);
            ViewBag.WorkshopID = new SelectList(db.Workshops, "WorkshopID", "Description", requestıssue.WorkshopID);
            ViewBag.RequestStateID = new SelectList(db.RequestStates, "RequestStateID", "Description", requestıssue.RequestStateID);
            ViewBag.UserReqID = new SelectList(db.Users, "UserId", "UserName", requestıssue.UserReqID);
            ViewBag.UserID = new SelectList(db.Users, "UserId", "UserName", requestıssue.UserID);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", requestıssue.ValidationStateID);
            return View(requestıssue);
        }

        //
        // POST: /RequestIssue/Edit/5

        [HttpPost]
        public ActionResult Edit(RequestIssue requestıssue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requestıssue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RequestTypeID = new SelectList(db.RequestTypes, "RequestTypeID", "Description", requestıssue.RequestTypeID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Description", requestıssue.LocationID);
            ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "Description", requestıssue.InventoryID);
            ViewBag.WorkshopID = new SelectList(db.Workshops, "WorkshopID", "Description", requestıssue.WorkshopID);
            ViewBag.RequestStateID = new SelectList(db.RequestStates, "RequestStateID", "Description", requestıssue.RequestStateID);
            ViewBag.UserReqID = new SelectList(db.Users, "UserId", "UserName", requestıssue.UserReqID);
            ViewBag.UserID = new SelectList(db.Users, "UserId", "UserName", requestıssue.UserID);
            ViewBag.ValidationStateID = new SelectList(db.ValidationStates, "ValidationStateID", "Description", requestıssue.ValidationStateID);
            return View(requestıssue);
        }

        //
        // GET: /RequestIssue/Delete/5
 
        public ActionResult Delete(int id)
        {
            RequestIssue requestıssue = db.RequestIssues.Find(id);
            return View(requestıssue);
        }

        //
        // POST: /RequestIssue/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            RequestIssue requestıssue = db.RequestIssues.Find(id);
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
        internal static void SendEmail(MailAddress fromAddress, MailAddress toAddress, string subject, string body)
        {
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };

            var client = new SmtpClient("KLMSNEVS.klimasan.msft");
            client.Send(message);
        }

    }
}