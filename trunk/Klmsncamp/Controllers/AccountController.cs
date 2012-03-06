using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Linq;
using System.Data;
using System.Data.Entity;
using Klmsncamp.Models;

namespace Klmsncamp.Controllers
{
    public class AccountController : Controller
    {
        private KlmsnContext db = new KlmsnContext();
        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register
        [Authorize]
        public ActionResult Register()
        {
            ViewBag.UserResetID = new SelectList(db.Users, "UserID", "FullName");
            return View();
        }

        //
        // POST: /Account/Register
        [Authorize]
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, model.FirstName, model.LastName, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            
            User user_ = db.Users.Include(p=>p.Roles).Include(p=>p.Workshops).Include(p => p.UserGroups).Include(p => p.CustomPermissions).Where(i => i.UserId == id).SingleOrDefault();

            ViewBag.RoleId = new MultiSelectList(db.Roles, "RoleID", "Description", user_.Roles.Select(p => p.RoleID).ToList());
            ViewBag.WorkshopID = new MultiSelectList(db.Workshops, "WorkshopID", "Description",user_.Workshops.Select(p=>p.WorkshopID).ToList());
            ViewBag.UserGroupID = new MultiSelectList(db.UserGroups, "UserGroupID", "Name",user_.UserGroups.Select(p=>p.UserGroupID).ToList());
            ViewBag.CustomPermissionID = new MultiSelectList(db.CustomPermissions, "CustomPermissionID", "Description",user_.CustomPermissions.Select(p=>p.CustomPermissionID).ToList());
            
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult PasswordReset(FormCollection formcollection)
        {
            try
            {
                User usr_ = db.Users.Find(int.Parse(formcollection["UserResetID"]));
                MembershipUser currentUser = Membership.GetUser(usr_.UserName, false/* userIsOnline */);
                string newPassword = currentUser.ResetPassword(usr_.UserName);
                ViewBag.NewPassword = newPassword;
                ViewBag.TheUser = usr_.FullName + "( " + usr_.UserName + " )";
            }
            catch (Exception exx)
            {
                ViewBag.NewPassword = "Hata Oluştu, (Detaylar --> " + exx.Message + ")";
            }
            return View();
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    if (model.NewPassword == model.ConfirmPassword)
                    {
                        changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                    }
                    else
                    {
                        changePasswordSucceeded = false;
                    }
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        #endregion Status Codes

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}