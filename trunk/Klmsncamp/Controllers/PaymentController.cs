using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Klmsncamp.Models;
using Klmsncamp.ViewModels;
using NPOI.HSSF.UserModel;

namespace Klmsncamp.Controllers
{
    public class PaymentController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /Default1/
        [Authorize(Roles = "administrators")]
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
        [Authorize(Roles = "administrators")]
        public ActionResult Create()
        {
            ViewBag.CorporateAccountID = new SelectList(db.CorporateAccounts, "CorporateAccountID", "Title");
            ViewBag.RequestIssueID = new SelectList(db.RequestIssues, "RequestIssueID", "DetailedDescription");
            return View();
        }

        //
        // POST: /Default1/Create

        [HttpPost]
        [Authorize(Roles = "administrators")]
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
        [Authorize(Roles = "administrators")]
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
        [Authorize(Roles = "administrators")]
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
        [Authorize(Roles = "administrators")]
        public ActionResult Delete(int id)
        {
            Payment payment = db.Payments.Find(id);
            return View(payment);
        }

        //
        // POST: /Default1/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "administrators")]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment payment = db.Payments.Find(id);

            db.Payments.Remove(payment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //customviewmodel deneme
        [Authorize(Roles = "administrators")]
        public ActionResult CreateCustomex()
        {
            ViewBag.CorporateAccountID = new SelectList(db.CorporateAccounts, "CorporateAccountID", "Title");
            ViewBag.RequestIssueID = new SelectList(db.RequestIssues, "RequestIssueID", "DetailedDescription");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "administrators")]
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

        [Authorize(Roles = "administrators")]
        public ActionResult Download(int id)
        {
            var _file = db.PaymentFiles.Find(id);
            byte[] _filedata = (byte[])_file.PaymentFileContents;
            return File(_filedata, _file.PaymentFileContentType, _file.PaymentFileName);
        }

        [Authorize(Roles = "administrators")]
        public ActionResult FileDelete(int id, int paymentid)
        {
            PaymentFile paymentfile = db.PaymentFiles.Find(id);
            db.PaymentFiles.Remove(paymentfile);
            db.SaveChanges();
            return RedirectToAction("Edit", new { id = paymentid });
        }

        [HttpPost]
        [Authorize(Roles = "administrators")]
        public ActionResult UploadFileOnly(FormCollection formcollection)
        {
            int id_ = int.Parse(formcollection["PaymentID"]);
            var payment_ = db.Payments.Find(id_);

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

            return RedirectToAction("Edit", new { id = id_ });
        }

        public ActionResult ExportXls()
        {
            //Get the data representing the current grid state - page, sort and filter
            //GridModel model = Model().ToGridModel(page, 10, orderBy, string.Empty, filter);
            //var orders = model.Data.Cast<Order>();

            var payments = from r in db.Payments select r; //.Where(i=>i.ValidationStateID==1).Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState);
            payments = payments.Include(p => p.CorporateAccount).Include(p => p.RequestIssue);
            var workbook = new HSSFWorkbook();

            //Create new Excel sheet
            var sheet = workbook.CreateSheet();

            //Create a header row
            var headerRow = sheet.CreateRow(0);

            //Set the column names in the header row
            headerRow.CreateCell(0).SetCellValue("Bütçe Ref No");
            headerRow.CreateCell(1).SetCellValue("Satınalma No");
            headerRow.CreateCell(2).SetCellValue("Firma");
            headerRow.CreateCell(3).SetCellValue("Fatura Tarihi");
            headerRow.CreateCell(4).SetCellValue("Fatura No");
            headerRow.CreateCell(5).SetCellValue("Fatura Toplam");
            headerRow.CreateCell(6).SetCellValue("Ödeme Tarihi");
            headerRow.CreateCell(7).SetCellValue("Açıklama");

            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);

            int rowNumber = 1;

            //Populate the sheet with values from the grid data
            foreach (Payment pay in payments)
            {
                //Create a new row
                var row = sheet.CreateRow(rowNumber++);

                //Set values for the cells
                row.CreateCell(0).SetCellValue(pay.BudgetNum);
                row.CreateCell(1).SetCellValue(pay.PurchaseNum);
                row.CreateCell(2).SetCellValue(pay.CorporateAccount.Title);
                row.CreateCell(3).SetCellValue(pay.InvoiceDate.ToString());
                row.CreateCell(4).SetCellValue(pay.InvoiceNum);
                row.CreateCell(5).SetCellValue(pay.InvoiceTotal.ToString());
                row.CreateCell(6).SetCellValue(pay.PaymentDate.ToString());
                row.CreateCell(7).SetCellValue(pay.Description);
            }

            //Write the workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user

            return File(output.ToArray(),   //The binary data of the XLS file
                "application/vnd.ms-excel", //MIME type of Excel files
                "OdemelerExcelExport.xls");     //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}