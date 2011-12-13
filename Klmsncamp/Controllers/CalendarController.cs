using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
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
            var rq_list = db.RequestIssues.Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState).Where(i => i.ValidationStateID == 1).ToList();

            List<RequestIssueCalendarViewModel> _rjsonlist = new List<RequestIssueCalendarViewModel>();
            string color_ = "#CCFF33";
            string textcolor_ = "#FFFFFF";
            bool editable_ = true;
            TimeSpan span = new TimeSpan();
            foreach (var item in rq_list)
            {
                if (item.EndDate != null)
                {
                    //DateTime xendtarihi = DateTime.ParseExact(item.EndDate.ToString(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    span = item.EndDate.Value.Subtract(DateTime.Now);
                    int diff = span.Days;
                    if (diff < 5 && diff > 2)
                    {
                        color_ = "#FFFF99";
                        textcolor_ = "#000000";
                    }
                    else if (diff <= 2 && diff > 0)
                    {
                        color_ = "FF6600";
                        textcolor_ = "#000000";
                    }
                    else if (diff == 0)
                    {
                        color_ = "#FF0000";
                    }
                    else if (diff < 0)
                    {
                        color_ = "#CC0000";
                    }
                    else if (diff >= 5)
                    {
                        color_ = "#6600FF";
                        textcolor_ = "#FFFFFF";
                    }

                    if (item.IsApproved == true)
                    {
                        color_ = "#33FF66";
                        textcolor_ = "#000000";
                        editable_ = false;
                    }

                    //bazı değişikliklere gidiyoruz. eğer istenmemmişse sadece termin trihleri gösterilecek
                    //ve ötelemeler sadece termin tarihleriyle alakalı olacak
                    //2den fazla ise otelenemeyecek

                    if (item.Pre1EndDate != null && item.Pre2EndDate != null)
                    {
                        editable_ = false;
                    }

                    string x_issahibi = "";
                    try
                    {
                        x_issahibi = item.User.FullName;
                    }
                    catch
                    {
                        x_issahibi = "--";
                    }
                    _rjsonlist.Add(new RequestIssueCalendarViewModel
                    {
                        id = item.RequestIssueID,
                        //title = item.Location.Description + "," + item.RequestType.Description + "," + item.DetailedDescription,
                        title = item.Location.Description + "," + item.RequestType.Description + "," + x_issahibi,
                        start = DateTime.Parse(item.EndDate.Value.AddMilliseconds(-1).ToString()).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        end = DateTime.Parse(item.EndDate.ToString()).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        allDay = item.IsAllDay,
                        url = "/HelpDesk/RequestIssue/Editp/" + item.RequestIssueID.ToString() + "?show=A&page=1",
                        color = color_,
                        textColor = textcolor_,
                        disableResizing = true,
                        editable = editable_
                    });
                }
                color_ = "#CCFF33";
                textcolor_ = "#FFFFFF";
                editable_ = true;
            }

            return Json(_rjsonlist, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "administrators,moderators")]
        public ActionResult Update(string xisid, string xtip, string xgun, string xdk, string xtumGun, string ms)
        {
            //tarih manipualsyonu
            RequestIssue rq = db.RequestIssues.Find(int.Parse(xisid));
            //değişiklikler:
            //öteleme sadece enddate icin geçerli bi durum olacak

            try
            {
                //baslangic mi bitis mi? <-- bu kontrol iptal. resize hiç bir zaman kullanmayacagiz.
                // cunkü her taşımamız resize olacak zaten..
                //rq.StartDate = rq.StartDate.AddDays(double.Parse(xgun));
                //rq.StartDate = rq.StartDate.AddMinutes(double.Parse(xdk));

                if (rq.Pre1EndDate == null)
                {
                    rq.Pre1EndDate = rq.EndDate;
                    rq.EndDate = rq.EndDate.Value.AddDays(double.Parse(xgun));
                    rq.EndDate = rq.EndDate.Value.AddMinutes(double.Parse(xdk));
                }
                else if (rq.Pre2EndDate == null)
                {
                    rq.Pre2EndDate = rq.EndDate;
                    rq.EndDate = rq.EndDate.Value.AddDays(double.Parse(xgun));
                    rq.EndDate = rq.EndDate.Value.AddMinutes(double.Parse(xdk));
                }
                else
                {
                    return Content("Termin Oteleme Limiti Dolmustur");
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

                return Content("Güncelleme başarılı");
            }
            catch
            {
                return Content("Hata Oluştu");
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
                client.Credentials = new NetworkCredential("MUSAF@klimasan.com.tr", "1212");
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