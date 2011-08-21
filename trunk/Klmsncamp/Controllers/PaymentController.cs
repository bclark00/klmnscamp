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
    public class PaymentController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /Default1/

        public ViewResult Index()
        {
            var payments = db.Payments.Include(p => p.CorporateAccount).Include(p => p.RequestIssue);
            return View(payments.ToList());
        }

        //
        // GET: /Default1/Details/5

        public ViewResult Details(int id)
        {
            Payment payment = db.Payments.Find(id);
            return View(payment);
        }

        //
        // GET: /Default1/Create

        public ActionResult Create()
        {
            ViewBag.CorporateAccountID = new SelectList(db.CorporateAccounts, "CorporateAccountID", "Title");
            ViewBag.RequestIssueID = new SelectList(db.RequestIssues, "RequestIssueID", "DetailedDescription");
            return View();
        }

        //
        // POST: /Default1/Create

        [HttpPost]
        public ActionResult Create(Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Payments.Add(payment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CorporateAccountID = new SelectList(db.CorporateAccounts, "CorporateAccountID", "Title", payment.CorporateAccountID);
            ViewBag.RequestIssueID = new SelectList(db.RequestIssues, "RequestIssueID", "DetailedDescription", payment.RequestIssueID);
            return View(payment);
        }

        //
        // GET: /Default1/Edit/5

        public ActionResult Edit(int id)
        {
            Payment payment = db.Payments.Find(id);
            ViewBag.CorporateAccountID = new SelectList(db.CorporateAccounts, "CorporateAccountID", "Title", payment.CorporateAccountID);
            ViewBag.RequestIssueID = new SelectList(db.RequestIssues, "RequestIssueID", "DetailedDescription", payment.RequestIssueID);
            return View(payment);
        }

        //
        // POST: /Default1/Edit/5

        [HttpPost]
        public ActionResult Edit(Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CorporateAccountID = new SelectList(db.CorporateAccounts, "CorporateAccountID", "Title", payment.CorporateAccountID);
            ViewBag.RequestIssueID = new SelectList(db.RequestIssues, "RequestIssueID", "DetailedDescription", payment.RequestIssueID);
            return View(payment);
        }

        //
        // GET: /Default1/Delete/5

        public ActionResult Delete(int id)
        {
            Payment payment = db.Payments.Find(id);
            return View(payment);
        }

        //
        // POST: /Default1/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment payment = db.Payments.Find(id);
            db.Payments.Remove(payment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //customviewmodel deneme
        public ActionResult CreateCustomex()
        {
            ViewBag.CorporateAccountID = new SelectList(db.CorporateAccounts, "CorporateAccountID", "Title");
            ViewBag.RequestIssueID = new SelectList(db.RequestIssues, "RequestIssueID", "DetailedDescription");
            return View();
        }

        [HttpPost]
        public ActionResult CreateCustomex(PaymentViewModel viewModel)
        {
            var payment_ = new Payment
            {
                BudgetNum = viewModel.BudgetNum,
                PurchaseNum = viewModel.PurchaseNum,
                CorporateAccountID = viewModel.CorporateAccountID,
                InvoiceDate = viewModel.InvoiceDate,
                InvoiceNum = viewModel.InvoiceNum,
                InvoiceTotal = viewModel.InvoiceTotal,
                PaymentDate = viewModel.PaymentDate,
                RequestIssueID = viewModel.RequestIssueID,
                Description = viewModel.Description
            };

            db.Payments.Add(payment_);
            db.SaveChanges();

            int _paymentid = payment_.PaymentID;
            int i = 0;
            foreach (var file_ in Request.Files)
            {
                PaymentFile file = RetrieveFileFromRequest(i);

                if (file.PaymentFileName != null
                    && !db.PaymentFiles.Any(f => f.PaymentFileName.Equals(file.PaymentFileName))
                    && file.PaymentFileSize > 0)
                {
                    file.Payment = payment_;
                    db.PaymentFiles.Add(file);
                    db.SaveChanges();
                }
                i++;
            }
            // save product
            // var product = new Product { Name = viewModel.Name, Price = viewModel.Price });
            // repo.Save(product);

            // now do something with the files
            //foreach (var file in viewModel.PaymentFiles)
            //{
            //    if (file.PaymentFileSize > 0)
            //    {
            //        var fileName = Path.GetFileName(file.PaymentFileName);
            //        var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
            //        file.SaveAs(path);
            //    }
            //}
            return RedirectToAction("Index");
        }

        private PaymentFile RetrieveFileFromRequest(int index)
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

            return new PaymentFile()
            {
                PaymentFileName = filename,
                PaymentFileContentType = fileType,
                PaymentFileSize = fileContents != null ? fileContents.Length : 0,
                PaymentFileContents = fileContents
            };
        }

        public ActionResult Download(int id)
        {
            var _file = db.PaymentFiles.Find(id);
            byte[] _filedata = (byte[])_file.PaymentFileContents;
            return File(_filedata, _file.PaymentFileContentType, _file.PaymentFileName);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}