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
        public ViewResult Index(string ms, string show, string ProjectID, string IncludeRq)
        {
            var requestıssues = db.RequestIssues.Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState).ToList();
            ViewBag.AbsolutePATH = this.Request.Url.AbsolutePath.ToString();

            if (show == null || show == "R")
            {
                ViewBag.ParameterString = "show=R";
                ViewBag.ActiveFilterDescription = "İş/Arıza Bildirim Kayıtları";
            }
            else if (show == "P")
            {
                ViewBag.ParameterString = "show=P";
                ViewBag.ActiveFilterDescription = "Proje Kayıtları";
            }
            else if (ProjectID != null)
            {
                ViewBag.ParameterString = "show=S&ProjectID=" + ProjectID + "&IncludeRq=Y";
                string abs_path = Url.Action("Edit", "Project", new { id = ProjectID }).ToString();
                ViewBag.ActiveFilterDescription = "<a href=\"" + abs_path + "\">#" + ProjectID + " nolu Proje</a> ve ona bağlı kayıtlar";
            }

            return View();
        }

        [Authorize(Roles = "administrators,moderators")]
        public ActionResult Json(string ms, string show, string ProjectID, string IncludeRq)
        {
            List<RequestIssueCalendarViewModel> rjsonlist_ = new List<RequestIssueCalendarViewModel>();

            if (show == null || show == "R")
            {
                rjsonlist_ = GetRequestIssuesJson();
            }
            else if (show == "P")
            {
                rjsonlist_ = GetProjectsJson();
            }
            else if (ProjectID != null)
            {
                rjsonlist_ = GetProjectRelated(int.Parse(ProjectID));
            }

            return Json(rjsonlist_, JsonRequestBehavior.AllowGet);
        }

        private List<RequestIssueCalendarViewModel> GetRequestIssuesJson()
        {
            List<RequestIssueCalendarViewModel> _rjsonlist = new List<RequestIssueCalendarViewModel>();
            var rq_list = db.RequestIssues.Include(r => r.RequestType).Include(r => r.Location).Include(r => r.Inventory).Include(r => r.Workshop).Include(r => r.RequestState).Include(r => r.UserReq).Include(r => r.User).Include(r => r.ValidationState).Where(i => i.ValidationStateID == 1).ToList();
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
                        url = Url.Action("Editp", "RequestIssue").ToString() + "/" + item.RequestIssueID.ToString() + "?show=A&page=1",
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

            return _rjsonlist;
        }

        private List<RequestIssueCalendarViewModel> GetProjectsJson()
        {
            List<RequestIssueCalendarViewModel> _rjsonlist = new List<RequestIssueCalendarViewModel>();

            var rq_list = db.Projects.Include(r => r.Locations).Include(r => r.Personnels).Include(r => r.RequestIssues).Include(p => p.User).Include(p => p.cUser).Include(p => p.CorporateAccounts).ToList();

            string color_ = "#CCFF33";
            string textcolor_ = "#FFFFFF";
            bool editable_ = false;
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
                        id = item.ProjectID,
                        //title = item.Location.Description + "," + item.RequestType.Description + "," + item.DetailedDescription,
                        title = item.MultiboxDescription + "," + x_issahibi,
                        start = DateTime.Parse(item.StartDate.AddMilliseconds(-1).ToString()).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        end = DateTime.Parse(item.EndDate.ToString()).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        allDay = false,
                        url = Url.Action("Index", "Calendar").ToString() + "?ms=" + DateTime.Now.ToString() + "&show=S&ProjectID=" + item.ProjectID.ToString() + "&IncludeRq=Y",
                        color = color_,
                        textColor = textcolor_,
                        disableResizing = true,
                        editable = editable_
                    });
                }
                color_ = "#CCFF33";
                textcolor_ = "#FFFFFF";
            }

            return _rjsonlist;
        }

        private List<RequestIssueCalendarViewModel> GetProjectRelated(int pr_id)
        {
            List<RequestIssueCalendarViewModel> _rjsonlist = new List<RequestIssueCalendarViewModel>();
            var item = db.Projects.Include(r => r.Locations).Include(r => r.Personnels).Include(r => r.RequestIssues).Include(p => p.User).Include(p => p.cUser).Include(p => p.CorporateAccounts).Where(i => i.ProjectID == pr_id).Single();
            string color_ = "#CCFF33";
            string textcolor_ = "#FFFFFF";
            bool editable_ = false;
            TimeSpan span = new TimeSpan();

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
                id = item.ProjectID,
                //title = item.Location.Description + "," + item.RequestType.Description + "," + item.DetailedDescription,
                title = item.MultiboxDescription + "," + x_issahibi,
                start = DateTime.Parse(item.StartDate.AddMilliseconds(-1).ToString()).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                end = DateTime.Parse(item.EndDate.ToString()).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                allDay = false,
                url = Url.Action("Edit", "Project").ToString() + "/" + item.ProjectID.ToString(),
                color = color_,
                textColor = textcolor_,
                disableResizing = true,
                editable = editable_
            });

            color_ = "#CCFF33";
            textcolor_ = "#FFFFFF";
            editable_ = true;
            span = new TimeSpan();

            foreach (var rqitem in item.RequestIssues.Where(i => i.ValidationStateID == 1).ToList())
            {
                if (rqitem.EndDate != null)
                {
                    //DateTime xendtarihi = DateTime.ParseExact(item.EndDate.ToString(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    span = rqitem.EndDate.Value.Subtract(DateTime.Now);
                    diff = span.Days;
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

                    if (rqitem.IsApproved == true)
                    {
                        color_ = "#33FF66";
                        textcolor_ = "#000000";
                        editable_ = false;
                    }

                    //bazı değişikliklere gidiyoruz. eğer istenmemmişse sadece termin trihleri gösterilecek
                    //ve ötelemeler sadece termin tarihleriyle alakalı olacak
                    //2den fazla ise otelenemeyecek

                    if (rqitem.Pre1EndDate != null && rqitem.Pre2EndDate != null)
                    {
                        editable_ = false;
                    }

                    x_issahibi = "";
                    try
                    {
                        x_issahibi = rqitem.User.FullName;
                    }
                    catch
                    {
                        x_issahibi = "--";
                    }
                    _rjsonlist.Add(new RequestIssueCalendarViewModel
                    {
                        id = rqitem.RequestIssueID,
                        //title = item.Location.Description + "," + item.RequestType.Description + "," + item.DetailedDescription,
                        title = rqitem.Location.Description + "," + rqitem.RequestType.Description + "," + x_issahibi,
                        start = DateTime.Parse(rqitem.EndDate.Value.AddMilliseconds(-1).ToString()).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        end = DateTime.Parse(rqitem.EndDate.ToString()).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        allDay = rqitem.IsAllDay,
                        url = Url.Action("Editp", "RequestIssue").ToString() + "/" + rqitem.RequestIssueID.ToString() + "?show=A&page=1",
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

            return _rjsonlist;
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
                    string softwaretitle = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 14).SingleOrDefault().ParameterValue;

                    string mailsonucstr = SendEmail(new MailAddress(rq.User.Email), new MailAddress(rq.UserReq.Email), "[" + softwaretitle + "] #" + rq.RequestIssueID.ToString() + " no'lu İş isteğiniz hakkında.", "İş İsteğinizin ön görülen başlangıç ve bitiş tarihleri değiştirilmiş/güncellenmiştir. İsteğinizin son durumu görmek isterseniz; " + Url.Action("Editp", "RequestIssue", new { id = rq.RequestIssueID, show = "A", page = 1 }, "http") + " adresini ziyaret ediniz. Tarih: " + DateTime.Now.ToString() + ". İyi çalışmalar dileriz.", rq.Personnel.Email, false);
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
        public string SendEmail(MailAddress fromAddress, MailAddress toAddress, string subject, string body, string fromPersonnel, bool fromEBA)
        {
            try
            {
                var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                };

                try
                {
                    message.CC.Add(new MailAddress(fromPersonnel));
                }
                catch { }

                string mailaccount_ = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 1).SingleOrDefault().ParameterValue;
                string mailpassword_ = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 2).SingleOrDefault().ParameterValue;
                string mailhost_ = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 3).SingleOrDefault().ParameterValue;
                var client = new SmtpClient(mailhost_);
                client.Credentials = new NetworkCredential(mailaccount_, mailpassword_);
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