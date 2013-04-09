using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using PusherRESTDotNet;

namespace Klmsncamp.Controllers
{
	public class ChatController : Controller
	{
		//
		// GET: /Chat/

		private static readonly PusherProvider Provider = new PusherProvider
	  (
		  ConfigurationManager.AppSettings["pusher_app_id"],
		  ConfigurationManager.AppSettings["pusher_key"],
		  ConfigurationManager.AppSettings["pusher_secret"]
	  );

		public ActionResult Index(string chatMessage, string username)
		{
			var now = DateTime.UtcNow;
            ObjectPusherRequest request = new ObjectPusherRequest(
			    "presence-channel",
				"message_received",
				new
				{
					message = chatMessage,
					user = username,
					timestamp = now.ToShortDateString() + " " + now.ToShortTimeString()
				});
           // var socketID =HttpContext.Request["socket_id"].ToString();
          //  Provider.Authenticate("presence-channel",request.SocketId.ToString());
			Provider.Trigger(request);

			return View();
		}
	}
}
