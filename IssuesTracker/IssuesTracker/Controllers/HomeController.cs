using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IssuesTracker.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using IssuesTracker.Models.IdentityViews;
using System.Web.Security;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Net;

namespace IssuesTracker.Controllers
{
    public class HomeController : Controller
    {
        Repository repository = new Repository();
        SecureString ss = new SecureString();
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var validation = repository.isValidUser(model.Email, ss.Encryptor(model.Password));
                if (validation)
                {
                    var ident = new ClaimsIdentity(
                        new[] {
                            new Claim(ClaimTypes.Name, model.Email.Split('@')[0]),
                            new Claim(ClaimTypes.Email, model.Email)
                        }, DefaultAuthenticationTypes.ApplicationCookie);
                    HttpContext.GetOwinContext().Authentication.SignIn(
                        new AuthenticationProperties { IsPersistent = false }, ident);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "User with same Email and Password doesn't exist");
                }

            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var validation = repository.checkUserByEmail(model.Email);
                if (!validation)
                {
                    repository.addUser(new AppUser() { UserName = model.Email.Split('@')[0], Email = model.Email, PasswordHash = ss.Encryptor(model.Password) });
                    validation = repository.isValidUser(model.Email, ss.Encryptor(model.Password));
                    if (validation)
                    {

                        var ident = new ClaimsIdentity(
                            new[] {
                                new Claim(ClaimTypes.Name, model.Email.Split('@')[0]),
                                new Claim(ClaimTypes.Email, model.Email)
                            }, DefaultAuthenticationTypes.ApplicationCookie);
                        HttpContext.GetOwinContext().Authentication.SignIn(
                            new AuthenticationProperties { IsPersistent = false }, ident);
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User with same Email lready exists");
                }
            }

            return View(model);
        }
        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Index()
        {
            return View(repository.getProjects());
        }
        [HttpPost]
        public ActionResult AddIssue(Issue_for_View issue)
        {
            Issue iss = new Issue()
            {
                Summary = issue.Summary,
                Description = issue.Description,
                Assignee = issue.Assignee,
                ProjectId = issue.ProjectId,
                Type = (Models.Type)Enum.Parse(typeof(Models.Type), issue.Type, true),
                Priority = (Priority)Enum.Parse(typeof(Priority), issue.Priority, true)
            };
            var str = repository.addIssue(iss);
            return Json(str);
        }
        [HttpPost]
        public ActionResult EditIssue(Issue_for_View issue)
        {
            Issue iss = new Issue()
            {
                Id = issue.Id,
                Summary = issue.Summary,
                Description = issue.Description,
                Assignee = issue.Assignee,
                ProjectId = repository.getProjectIdByName(issue.ProjectName),
                Type = (Models.Type)Enum.Parse(typeof(Models.Type), issue.Type, true),
                Priority = (Priority)Enum.Parse(typeof(Priority), issue.Priority, true)
            };
            repository.editIssue(iss);
            return Json("Successfully edited");
        }
        [HttpGet]
        public ActionResult GetGridIssues(int id) //id = projectId
        {
            return PartialView(repository.getIssues(id));
        }
        [HttpGet]
        public ActionResult GetIssues(int id)//id = projectId
        {
            return Json(repository.getIssues(id), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetIssue(int id)//id = issueId
        {
            return Json(repository.getIssue(id), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult IssueModal(int projectId, int id = -1)
        {
            if (id == -1)
            {
                ViewBag.Action = "Create";
                var issue = new Issue();
                issue.ProjectId = projectId;
                var issue_for_view = repository.ViewIssues(new List<Issue>() { issue }).First();
                issue_for_view.Priority = "Trivial";
                return PartialView(issue_for_view);
            }
            ViewBag.Action = "Edit";
            return PartialView(repository.getIssue(id));
        }
        [HttpGet]
        public ActionResult KanbanCards(int id)//id=projectId
        {
            ViewBag.Types = Enum.GetNames(typeof(Models.Type)).ToList();
            return PartialView(repository.getIssues(id));
        }
        [HttpPost]
        public ActionResult UpdateType(int issueId, string issueType)//id = issueId
        {
            return Json(repository.changeType(issueId, issueType));
        }
        public ActionResult GetPriorities()
        {
            return Json(Enum.GetNames(typeof(Priority)).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Kanban()
        {
            ViewBag.projects = repository.getProjects();
            return View();
        }
    }
}