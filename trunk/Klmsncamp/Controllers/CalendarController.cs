using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Klmsncamp.Models;
using Klmsncamp.ViewModels;

namespace Klmsncamp.Controllers
{
    public class CalendarController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /Calendar/

        [Authorize(Roles = "administrators,moderators")]
        public ViewResult Index()
        {
            var requestıssues = db.RequestIssues.Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState).ToList();
            return View();
        }

        [Authorize(Roles = "administrators,moderators")]
        public ActionResult Json()
        {
            var rq_list = db.RequestIssues.Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState).ToList();

            List<RequestIssueCalendarViewModel> _rjsonlist = new List<RequestIssueCalendarViewModel>();
            foreach (var item in rq_list)
            {
                if (item.EndDate != null)
                {
                    _rjsonlist.Add(new RequestIssueCalendarViewModel
                    {
                        id = item.RequestIssueID,
                        //title = item.Location.Description + "," + item.RequestType.Description + "," + item.DetailedDescription,
                        title = item.Location.Description,
                        start = DateTime.Parse(item.StartDate.ToString()).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        end = DateTime.Parse(item.EndDate.ToString()).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        allDay = item.IsAllDay,
                        url = "/RequestIssue/Editp/" + item.RequestIssueID.ToString() + "?show=A&page=1"
                    });
                }
                else
                {
                    _rjsonlist.Add(new RequestIssueCalendarViewModel
                    {
                        id = item.RequestIssueID,
                        //title = item.Location.Description + "," + item.RequestType.Description + "," + item.DetailedDescription,
                        title = item.Location.Description,
                        start = DateTime.Parse(item.StartDate.ToString()).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        allDay = item.IsAllDay,
                        url = "/RequestIssue/Editp/" + item.RequestIssueID.ToString()
                    });
                }
            }

            return Json(_rjsonlist, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "administrators,moderators")]
        public ActionResult Update(string xisid, string xtip, string xgun, string xdk, string xtumGun)
        {
            //tarih manipualsyonu
            RequestIssue rq = db.RequestIssues.Find(int.Parse(xisid));

            try
            {
                if (rq.EndDate == null)
                {
                    rq.EndDate = rq.StartDate.AddHours(2);
                }

                //baslangic mi bitis mi?
                if (xtip == "E")
                {
                    rq.EndDate = rq.EndDate.Value.AddDays(double.Parse(xgun));
                    rq.EndDate = rq.EndDate.Value.AddMinutes(double.Parse(xdk));
                }
                else if (xtip == "S")
                {
                    rq.StartDate = rq.StartDate.AddDays(double.Parse(xgun));
                    rq.StartDate = rq.StartDate.AddMinutes(double.Parse(xdk));

                    rq.EndDate = rq.EndDate.Value.AddDays(double.Parse(xgun));
                    rq.EndDate = rq.EndDate.Value.AddMinutes(double.Parse(xdk));
                }

                if (xtumGun == "E")
                {
                    rq.IsAllDay = true;
                }
                else
                {
                    rq.IsAllDay = false;
                }

                db.Entry(rq).State = EntityState.Modified;
                db.SaveChanges();

                if (rq.SendEmail == true)
                {
                    string mailsonucstr = SendEmail(new MailAddress("musaf@klimasan.com.tr"), new MailAddress(rq.UserReq.Email), "[Klimasan HelpDesk] #" + rq.RequestIssueID.ToString() + " no'lu İş isteğiniz hakkında.", "İş İsteğinizin ön görülen başlangıç ve bitiş tarihleri değiştirilmiş/güncellenmiştir. İsteğinizin son durumu görmek isterseniz; http:/127.0.0.1:43970/RequestIssue/Editp/" + rq.RequestIssueID.ToString() + " adresini ziyaret ediniz. Tarih: " + DateTime.Now.ToString() + ". İyi çalışmalar dileriz.");
                    if (mailsonucstr != "OK")
                    {
                        ViewBag.Bilgilendirme = "Mail Gönderiminde Hata: " + mailsonucstr;
                    }
                    else
                    {
                        ViewBag.Bilgilendirme = "Mail Başarıyla Gönderildi";
                    }
                }

                return Content("<font color=\"red\">Güncelleme başarılı</font>");
            }
            catch
            {
                return Content("<font color=\"red\">Hata Oluştu</font>");
            }
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
                client.Send(message);

                return "OK";
            }

            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}