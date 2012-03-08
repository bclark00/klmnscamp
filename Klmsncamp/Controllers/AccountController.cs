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
        public ActionResult Edit(string username_,string err)
        {
            int _userid = db.Users.Where(i => i.UserName == username_).SingleOrDefault().UserId;
            User user_ = db.Users.Include(p=>p.Roles).Include(p=>p.Workshops).Include(p=>p.WorkshopPermissions).Include(p => p.UserGroups).Include(p => p.CustomPermissions).Where(i => i.UserId == _userid).SingleOrDefault();


            IList<WorkshopPermission> MyWorkshopPermissions = new List<WorkshopPermission>();

            foreach (WorkshopPermission wrp_item in user_.WorkshopPermissions.ToList())
            {
                MyWorkshopPermissions.Add(wrp_item);    
            }
            ViewBag.TheWorkshopPermissionsCount = MyWorkshopPermissions.Count;
            ViewBag.TheWorkshopPermissions = MyWorkshopPermissions;
            ViewBag.ErrorMessage = err;
            ViewBag.WorkshopID = new SelectList(db.Workshops, "WorkshopID", "Description");
            ViewBag.UserID = new SelectList(db.Users, "UserId", "FullNameWithUsername");

            ViewBag.RoleId = new MultiSelectList(db.Roles, "RoleID", "Description", user_.Roles.Select(p => p.RoleID).ToList());
            ViewBag.WorkshopMultiSelectID = new MultiSelectList(db.Workshops, "WorkshopID", "Description",user_.Workshops.Select(p=>p.WorkshopID).ToList());
            ViewBag.UserGroupID = new MultiSelectList(db.UserGroups, "UserGroupID", "Name",user_.UserGroups.Select(p=>p.UserGroupID).ToList());
            ViewBag.CustomPermissionID = new MultiSelectList(db.CustomPermissions, "CustomPermissionID", "Description",user_.CustomPermissions.Select(p=>p.CustomPermissionID).ToList());
            
            return View(user_);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(User user, FormCollection formcollection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                //roles bosalt
                User user_ = db.Users.Include(p => p.Roles).Include(p => p.Workshops).Include(p => p.UserGroups).Include(p => p.CustomPermissions).Where(i => i.UserId == user.UserId).SingleOrDefault();
                user_.Roles.Clear();
                user_.UserGroups.Clear();
                user_.Workshops.Clear();
                user_.CustomPermissions.Clear();
                db.SaveChanges();

                if (formcollection["RoleID"] != null)
                {
                    foreach (var role_int in formcollection["RoleID"].Split(',').ToList())
                    {
                        try
                        {
                            int role_index = int.Parse(role_int.ToString());
                            var role_ = db.Roles.Find(role_index);
                            role_.Users.Add(user_);
                        }
                        catch
                        { }
                        db.SaveChanges();
                    }
                }

                if (formcollection["WorkshopMultiSelectID"] != null)
                {
                    foreach (var workshop_int in formcollection["WorkshopMultiSelectID"].Split(',').ToList())
                    {
                        try
                        {
                            int workshop_index = int.Parse(workshop_int.ToString());
                            var workshop_ = db.Workshops.Find(workshop_index);
                            workshop_.Users.Add(user_);
                        }
                        catch
                        { }
                        db.SaveChanges();
                    }
                }

                if (formcollection["UserGroupID"] != null)
                {
                    foreach (var usergroup_int in formcollection["UserGroupID"].Split(',').ToList())
                    {
                        try
                        {
                            int usergroup_index = int.Parse(usergroup_int.ToString());
                            var usergroup_ = db.UserGroups.Find(usergroup_index);
                            usergroup_.Users.Add(user_);
                        }
                        catch
                        { }
                        db.SaveChanges();
                    }
                }

                if (formcollection["CustomPermissionID"] != null)
                {
                    foreach (var custom_int in formcollection["CustomPermissionID"].Split(',').ToList())
                    {
                        try
                        {
                            int custom_index = int.Parse(custom_int.ToString());
                            var customperm_ = db.CustomPermissions.Find(custom_index);
                            customperm_.Users.Add(user_);
                        }
                        catch
                        { }
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Edit", new { username_ = user.UserName });
        }

        [Authorize(Roles = "administrators")]
        public ActionResult RedirectToEdit(int UserID)
        {
            string username = db.Users.AsNoTracking().Where(i => i.UserId == UserID).SingleOrDefault().UserName;
            return RedirectToAction("Edit", new { username_ = username });

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