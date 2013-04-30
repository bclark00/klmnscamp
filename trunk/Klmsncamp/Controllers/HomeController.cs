using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using Klmsncamp.DAL;
using Klmsncamp.Models;
using Klmsncamp.ViewModels;
using System.Net.Mail;

namespace Klmsncamp.Controllers
{
	public class HomeController : Controller
	{
		private KlmsnContext db = new KlmsnContext();

		public ActionResult Index(string err)
		{
			if (err == "404")
			{
				ViewBag.ErrMessage = "Aradığınız Arıza no bulunamadı..";
			}

			ViewBag.WelcomeString = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 11).SingleOrDefault().ParameterValue;
			return View();
		}


		Klmsncamp.Models.DownloadModel downloadModel;
		public HomeController()
		{
			downloadModel = new DownloadModel();
		}

     
		public ActionResult About()
		{
            
			//var tempData = TempData["Sevko"];
			//var viewBag = ViewBag.Sevko;
			ViewBag.MarqueeString = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 13).SingleOrDefault().ParameterValue;
			//List<Klmsncamp.Models.FileNames> list = downloadModel.GetFiles();

			//ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "Description");

			//	ViewBag.CorporateAccountID = new MultiSelectList(db.CorporateAccounts, "CorporateAccountID", "Title", project.CorporateAccounts.Select(p => p.CorporateAccountID).ToList());

			ViewBag.Users = new MultiSelectList(db.Users.ToList(), "Email", "UserName");
			ViewBag.UserList = new MultiSelectList(db.Users.ToList(), "Email", "UserName");
			List<UploadedFile> list = db.UploadedFiles.Where(s => s.IsActive == true).ToList();
			return View(list);
		}

		public FileContentResult Download(string id)
		{
			int fid = Convert.ToInt32(id);
			//FileNames file = downloadModel.GetFiles().FirstOrDefault(s => s.FileID == fid);
			//string fileName = file.FileName;
			//string contentType = file.FileContentType;
			//string filePath = file.FilePath;

			UploadedFile selectedFile = db.UploadedFiles.FirstOrDefault(s => s.ID == fid);
			byte[] fileByte = System.IO.File.ReadAllBytes(selectedFile.FilePath);
			return File(fileByte, selectedFile.FileContentType, selectedFile.FileName);
		}

		public ActionResult Remove(string id)
		{
			int secilenId = Convert.ToInt32(id);
			UploadedFile secilenFile = db.UploadedFiles.FirstOrDefault(s => s.ID == secilenId);
			secilenFile.IsActive = false;
			db.SaveChanges();

			List<UploadedFile> list = db.UploadedFiles.Where(s => s.IsActive == true).ToList();
			ViewBag.Users = new MultiSelectList(db.Users.ToList(), "Email", "UserName");
			return View("About", list);
		}


		[HttpPost]
		public ActionResult Upload(IEnumerable<HttpPostedFileBase> files, string Aciklama, FormCollection formcollection, string UserList, string Users)
		{
			string sd = formcollection["Aciklama"].ToString();
			//string Email = formcollection["UserList"].ToString();
			//int userID = int.Parse(formcollection["UserList"]);
			string mailGonderilecekler = formcollection["Users"].ToString();
			string[] mails = mailGonderilecekler.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var file in files)
			{
				if (file != null && file.ContentLength > 0)
				{
					try
					{
						string extension = System.IO.Path.GetExtension(file.FileName);
						int index = file.FileName.IndexOf(".");
						string fileName = Path.GetFileName(file.FileName.Substring(0, index) + "-" + Guid.NewGuid().ToString().Substring(0, 4).Replace(".", "-") + extension);
						string path = Path.Combine(Server.MapPath("~/App_Data/UploadedFiles"), fileName);

						UploadedFile uf = new UploadedFile();
						uf.FileName = fileName;
						uf.FilePath = path;
						uf.Description = Aciklama;
						uf.IsActive = true;
						uf.FileContentType = file.ContentType;

						db.UploadedFiles.Add(uf);
						db.SaveChanges();

						file.SaveAs(path);
					}
					catch (Exception ex)
					{
						throw ex;
					}
				} 
			}
			try
			{
				System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage("musa.fedakar@arelektronik.com", mails[0]);

				for (int i = 1; i < mails.Length; i++)
				{
					mail.CC.Add(mails[i]);
				}

				mail.Subject = "Ar Elektronik Bilgilendirme";
				mail.Body = Aciklama;

				SmtpClient client = new SmtpClient("mail.arelektronik.com", 587);
				client.Credentials = new NetworkCredential("musa.fedakar@arelektronik.com", "M123456f.");
				client.Send(mail);
				client.Dispose();
				mail.Dispose();
			}
			catch (Exception ex)
			{
				throw ex;
			}

			List<UploadedFile> list = db.UploadedFiles.Where(s => s.IsActive == true).ToList();
			//List<Klmsncamp.Models.FileNames> list = downloadModel.GetFiles();
			ViewBag.Users = new MultiSelectList(db.Users.ToList(), "Email", "UserName");
			return View("About", list);
		}


		public ActionResult WeatherWidget()
		{
			//string _location = Request.QueryString["location"];
			//string _metric = Request.QueryString["metric"];

			////string _url = string.Format("http://rainmeter.accu-weather.com/widget/rainmeter/weather-data.asp?location={0}&metric={1}", _location, _metric);
			//string _url = string.Format("http://wwwa.accuweather.com/adcbin/forecastfox/weather_data.asp?location={0}&metric={1}", _location, _metric);

			//string _xml = DownloadWebPage(_url);

			//XmlDocument _xmlDocument = new XmlDocument();
			//_xmlDocument.LoadXml(_xml);

			//XmlNamespaceManager _mgr = new XmlNamespaceManager(_xmlDocument.NameTable);
			//_mgr.AddNamespace("pf", _xmlDocument.DocumentElement.NamespaceURI);

			Weather _weather = new Weather();

			//_weather.city =
			//    _xmlDocument.SelectSingleNode("/pf:adc_database/pf:local/pf:city", _mgr).InnerText;
			//_weather.curr_temp = Convert.ToInt32(
			//    _xmlDocument.SelectSingleNode("/pf:adc_database/pf:currentconditions/pf:temperature", _mgr).InnerText);
			//_weather.curr_text =
			//    _xmlDocument.SelectSingleNode("/pf:adc_database/pf:currentconditions/pf:weathertext", _mgr).InnerText;
			//_weather.curr_icon = Convert.ToInt32(
			//    _xmlDocument.SelectSingleNode("/pf:adc_database/pf:currentconditions/pf:weathericon", _mgr).InnerText);

			//XmlNodeList _xmlNodeList = _xmlDocument.SelectNodes("/pf:adc_database/pf:forecast/pf:day", _mgr);
			//int _day = _xmlNodeList.Count;
			//int i = 0;
			//foreach (XmlNode _dayItem in _xmlNodeList)
			//{
			//    Forecast _forecast = new Forecast();

			//    _forecast.day_date = _dayItem["obsdate"].InnerXml;
			//    _forecast.day_text = _dayItem.SelectSingleNode("pf:daytime", _mgr)["txtshort"].InnerXml;
			//    _forecast.day_icon =
			//        Convert.ToInt32(_dayItem.SelectSingleNode("pf:daytime", _mgr)["weathericon"].InnerXml);
			//    _forecast.day_htemp =
			//        Convert.ToInt32(_dayItem.SelectSingleNode("pf:daytime", _mgr)["hightemperature"].InnerXml);
			//    _forecast.day_ltemp =
			//        Convert.ToInt32(_dayItem.SelectSingleNode("pf:daytime", _mgr)["lowtemperature"].InnerXml);

			//    _weather.forecast.Add(_forecast);

			//    i++;
			//    // 5 day forecast
			//    if (i == 5) break;
			//}

			var viewModel = new WJson();
			viewModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(_weather);
			return Json(_weather, JsonRequestBehavior.AllowGet);
		}

		public string DownloadWebPage(string Url)
		{
			// Open a connection
			HttpWebRequest WebRequestObject = (HttpWebRequest)HttpWebRequest.Create(Url);

			// You can also specify additional header values like
			// the user agent or the referer:
			WebRequestObject.UserAgent = ".NET Framework/2.0";
			WebRequestObject.Referer = "http://www.example.com/";

			// Request response:
			WebResponse Response = WebRequestObject.GetResponse();

			// Open data stream:
			Stream WebStream = Response.GetResponseStream();

			// Create reader object:
			StreamReader Reader = new StreamReader(WebStream);

			// Read the entire stream content:
			string PageContent = Reader.ReadToEnd();

			// Cleanup
			Reader.Close();
			WebStream.Close();
			Response.Close();

			return PageContent;
		}

		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}

        public ActionResult OnlineUser()
        {
            return PartialView("_OnlineUsers");
        }
	}
}