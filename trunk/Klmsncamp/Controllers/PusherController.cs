using PusherRESTDotNet;
using PusherRESTDotNet.Authentication;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Klmsncamp.Controllers
{
    public class PusherController : Controller
    {
        //
        // GET: /Pusher/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Auth(string channel_name, string socket_id)
        {
            var applicationId = ConfigurationManager.AppSettings["pusher_app_id"];
            var applicationKey = ConfigurationManager.AppSettings["pusher_key"];
            var applicationSecret = ConfigurationManager.AppSettings["pusher_secret"];
          
            //var channelData = new PresenceChannelData()
            //{
            //    user_id = Guid.NewGuid().ToString()
            //};
            var channelData = new PresenceChannelData();
            if (User.Identity.IsAuthenticated)
            {
                channelData.user_id = User.Identity.Name;
            }
            else
            {
                channelData.user_id = Guid.NewGuid().ToString();
            }
            channelData.user_info = new PusherUserInfo();

            var provider = new PusherProvider(applicationId, applicationKey, applicationSecret);
            string authJson = provider.Authenticate(channel_name, socket_id, channelData);

            return new ContentResult { Content = authJson, ContentType = "application/json" };
        }

        public class PusherUserInfo
        {
            public DateTime timestamp { get { return DateTime.Now; } set { this.timestamp = value; } }
        }
    }
}
