using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Klmsncamp.Models;

namespace Klmsncamp.Controllers
{
    public class ProjectController : Controller
    {
        private KlmsnContext db = new KlmsnContext();

        //
        // GET: /Project/

        public ViewResult Index()
        {
            var projects = db.Projects.Include(p => p.RequestState).Include(p => p.User).Include(p => p.cUser).Include(p => p.RequestIssues).Include(p => p.Locations).Include(p => p.CorporateAccounts).Include(p => p.Personnels);
            return View(projects.ToList());
        }

        //
        // GET: /Project/Details/5
        [Authorize]
        public ViewResult Details(int id)
        {
            Project project = db.Projects.Find(id);
            return View(project);
        }

        //
        // GET: /Project/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.RequestStateID = new SelectList(db.RequestStates, "RequestStateID", "Description");
            ViewBag.UserID = new SelectList(db.Users, "UserId", "FullName");
            ViewBag.cUserID = new SelectList(db.Users, "UserId", "FullName");
            ViewBag.LocationID = new MultiSelectList(db.Locations, "LocationID", "Description");
            ViewBag.CorporateAccountID = new MultiSelectList(db.CorporateAccounts, "CorporateAccountID", "Title");
            ViewBag.PersonnelID = new MultiSelectList(db.Personnels, "PersonnelID", "FullName");
            return View();
        }

        //
        // POST: /Project/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Project project, FormCollection formcollection)
        {
            MembershipUser currentuser_ = new UserRepository().GetUser(User.Identity.Name);
            int user_wherecondition = int.Parse((currentuser_.ProviderUserKey).ToString());

            if (ModelState.IsValid)
            {
                project.TimeStamp = DateTime.Now;
                project.cUserID = user_wherecondition;
                project.RequestStateID = 4;
                db.Projects.Add(project);
                db.SaveChanges();

                if (formcollection["LocationID"] != null)
                {
                    foreach (var location_ in formcollection["LocationID"].Split(',').ToList())
                    {
                        try
                        {
                            int location_index = int.Parse(location_.ToString());
                            var x_location = db.Locations.Find(location_index);
                            x_location.Projects.Add(project);
                        }
                        catch
                        { }
                        db.SaveChanges();
                    }
                }

                if (formcollection["CorporateAccountID"] != null)
                {
                    foreach (var corp_ in formcollection["CorporateAccountID"].Split(',').ToList())
                    {
                        try
                        {
                            int corp_index = int.Parse(corp_.ToString());
                            var x_corp = db.CorporateAccounts.Find(corp_index);
                            x_corp.Projects.Add(project);
                        }
                        catch
                        { }

                        db.SaveChanges();
                    }
                }

                if (formcollection["PersonnelID"] != null)
                {
                    foreach (var pers_ in formcollection["PersonnelID"].Split(',').ToList())
                    {
                        try
                        {
                            int pers_index = int.Parse(pers_.ToString());
                            var x_pers = db.Personnels.Find(pers_index);
                            x_pers.Projects.Add(project);
                        }
                        catch
                        { }
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }

            ViewBag.RequestStateID = new SelectList(db.RequestStates, "RequestStateID", "Description", project.RequestStateID);
            ViewBag.UserID = new SelectList(db.Users, "UserId", "FullName", project.UserID);
            ViewBag.cUserID = new SelectList(db.Users, "UserId", "UserName", project.cUserID);

            if (formcollection["LocationID"] != null)
            {
                ViewBag.LocationID = new MultiSelectList(db.Locations, "LocationID", "Description", formcollection["LocationID"].Split(',').ToList());
            }
            else
            {
                ViewBag.LocationID = new MultiSelectList(db.Locations, "LocationID", "Description");
            }

            if (formcollection["CorporateAccountID"] != null)
            {
                ViewBag.CorporateAccountID = new MultiSelectList(db.CorporateAccounts, "CorporateAccountID", "Title", formcollection["CorporateAccountID"].Split(',').ToList());
            }
            else
            {
                ViewBag.CorporateAccountID = new MultiSelectList(db.CorporateAccounts, "CorporateAccountID", "Title");
            }

            if (formcollection["PersonnelID"] != null)
            {
                ViewBag.PersonnelID = new MultiSelectList(db.Personnels, "PersonnelID", "FullName", formcollection["PersonnelID"].Split(',').ToList());
            }
            else
            {
                ViewBag.PersonnelID = new MultiSelectList(db.Personnels, "PersonnelID", "FullName");
            }
            return View(project);
        }

        //
        // GET: /Project/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Project project = db.Projects.Include(p => p.Locations).Include(p => p.CorporateAccounts).Include(p => p.Personnels).Where(i => i.ProjectID == id).SingleOrDefault();
            ViewBag.RequestStateID = new SelectList(db.RequestStates.Where(i => i.RequestStateID == 4 || i.RequestStateID == 5), "RequestStateID", "Description", project.RequestStateID);
            ViewBag.UserID = new SelectList(db.Users, "UserId", "FullName", project.UserID);
            ViewBag.cUserID = new SelectList(db.Users, "UserId", "FullName", project.cUserID);
            ViewBag.LocationID = new MultiSelectList(db.Locations, "LocationID", "Description", project.Locations.Select(p => p.LocationID).ToList());
            ViewBag.CorporateAccountID = new MultiSelectList(db.CorporateAccounts, "CorporateAccountID", "Title", project.CorporateAccounts.Select(p => p.CorporateAccountID).ToList());
            ViewBag.PersonnelID = new MultiSelectList(db.Personnels, "PersonnelID", "FullName", project.Personnels.Select(p => p.PersonnelID).ToList());

            return View(project);
        }

        //
        // POST: /Project/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Project project, FormCollection formcollection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();

                //locations bosalt
                Project project_ = db.Projects.Include(p => p.Locations).Include(p => p.Personnels).Include(p => p.CorporateAccounts).Where(i => i.ProjectID == project.ProjectID).SingleOrDefault();
                project_.Locations.Clear();
                project_.Personnels.Clear();
                project_.CorporateAccounts.Clear();
                db.SaveChanges();

                if (formcollection["LocationID"] != null)
                {
                    foreach (var location_ in formcollection["LocationID"].Split(',').ToList())
                    {
                        try
                        {
                            int location_index = int.Parse(location_.ToString());
                            var x_location = db.Locations.Find(location_index);
                            x_location.Projects.Add(project);
                        }
                        catch
                        { }
                        db.SaveChanges();
                    }
                }

                if (formcollection["CorporateAccountID"] != null)
                {
                    foreach (var corp_ in formcollection["CorporateAccountID"].Split(',').ToList())
                    {
                        try
                        {
                            int corporation_index = int.Parse(corp_.ToString());
                            var x_corp = db.CorporateAccounts.Find(corporation_index);
                            x_corp.Projects.Add(project);
                        }
                        catch
                        { }
                        db.SaveChanges();
                    }
                }

                if (formcollection["PersonnelID"] != null)
                {
                    foreach (var pers_ in formcollection["PersonnelID"].Split(',').ToList())
                    {
                        try
                        {
                            int pers_index = int.Parse(pers_.ToString());
                            var x_pers = db.Personnels.Find(pers_index);
                            x_pers.Projects.Add(project);
                        }
                        catch
                        { }
                        db.SaveChanges();
                    }
                }

                if (formcollection["AddNewRq"] != null)
                {
                    return RedirectToAction("Create", "RequestIssue", new { projectid = project.ProjectID });
                }
                return RedirectToAction("Index");
            }
            ViewBag.RequestStateID = new SelectList(db.RequestStates, "RequestStateID", "Description", project.RequestStateID);
            ViewBag.UserID = new SelectList(db.Users, "UserId", "FullName", project.UserID);
            ViewBag.cUserID = new SelectList(db.Users, "UserId", "FullName", project.cUserID);

            Project project__ = db.Projects.Include(p => p.Locations).Include(p => p.Personnels).Include(p => p.CorporateAccounts).Where(i => i.ProjectID == project.ProjectID).SingleOrDefault();
            /*project.Locations = project__.Locations;
            project.CorporateAccounts = project__.CorporateAccounts;
            project.Personnels = project__.Personnels;
            */

            ViewBag.LocationID = new MultiSelectList(db.Locations, "LocationID", "Description", project__.Locations.Select(p => p.LocationID).ToList());
            ViewBag.CorporateAccountID = new MultiSelectList(db.CorporateAccounts, "CorporateAccountID", "Title", project__.CorporateAccounts.Select(p => p.CorporateAccountID).ToList());
            ViewBag.PersonnelID = new MultiSelectList(db.Personnels, "PersonnelID", "FullName", project__.Personnels.Select(p => p.PersonnelID).ToList());

            return View(project);
        }

        //
        // GET: /Project/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            Project project = db.Projects.Find(id);
            return View(project);
        }

        //
        // POST: /Project/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project_ = db.Projects.Include(p => p.Locations).Include(p => p.Personnels).Include(p => p.CorporateAccounts).Where(i => i.ProjectID == id).SingleOrDefault();

            var requesttracks = new HashSet<int>(project_.RequestIssues.Select(i => i.RequestIssueID));
            foreach (int rq_id in requesttracks)
            {
                db.RequestIssues.Include(s => s.Projects).Where(a => a.RequestIssueID == rq_id).SingleOrDefault().Projects.Remove(project_);
                db.SaveChanges();
            }

            project_.Locations.Clear();
            project_.Personnels.Clear();
            project_.CorporateAccounts.Clear();
            db.SaveChanges();

            db.Projects.Remove(project_);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}