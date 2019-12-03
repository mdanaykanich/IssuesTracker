using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using IssuesTracker.Models;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace IssuesTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        Repository repository = new Repository();

        public ActionResult Index()
        {
            return View(repository.getProjects());
        }

        [HttpPost]
        public ActionResult AddIssue(Issue_for_View issueForView)
        {
            Issue _issue = new Issue()
            {
                Summary = issueForView.Summary,
                Description = issueForView.Description,
                Assignee = issueForView.Assignee,
                ProjectId = issueForView.ProjectId,
                Type = (Models.Type)Enum.Parse(typeof(Models.Type), issueForView.Type, true),
                Priority = (Priority)Enum.Parse(typeof(Priority), issueForView.Priority, true)
            };
            return Json(repository.addIssue(_issue));
        }

        [HttpPost]
        public ActionResult EditIssue(Issue_for_View issueForView)
        {
            Issue _issue = new Issue()
            {
                Id = issueForView.Id,
                Summary = issueForView.Summary,
                Description = issueForView.Description,
                Assignee = issueForView.Assignee,
                ProjectId = repository.getProjectIdByName(issueForView.ProjectName),
                Type = (Models.Type)Enum.Parse(typeof(Models.Type), issueForView.Type, true),
                Priority = (Priority)Enum.Parse(typeof(Priority), issueForView.Priority, true)
            };
            return Json(repository.editIssue(_issue));
        }

        [HttpGet]
        public ActionResult GetGridIssues(int id)
        {
            return PartialView(repository.getIssues(id));
        }

        [HttpPost]
        public ActionResult IssueModal(int projectId, int id = -1)
        {
            const int nonexistentIssueId = -1;
            int issueId = id;
            ViewBag.Users = repository.getUsersByProjectId(projectId);
            if (issueId == nonexistentIssueId)
            {
                ViewBag.Action = "Create";
                Issue issue = new Issue();
                issue.ProjectId = projectId;
                Issue_for_View issueForView = repository.ViewIssues(new List<Issue>() { issue }).First();
                issueForView.Priority = "Trivial";
                return PartialView(issueForView);
            }
            ViewBag.Action = "Edit";
            return PartialView(repository.getIssue(id));
        }

        [HttpGet]
        public ActionResult Roles()
        {
            return View(repository.getRoleNames());
        }

        [HttpPost]
        public ActionResult AddRole(string roleName)
        {
            var roleManager = HttpContext.GetOwinContext().GetUserManager<RoleManager<AppRole>>();
            if (!roleManager.RoleExists(roleName))
                roleManager.Create(new AppRole(roleName));
            return Json($"Role {roleName} was successfully added");
        }

        public ActionResult Management()
        {
            if (!User.IsInRole("Manager"))
            {
                return RedirectToAction("Index");
            }
            ViewBag.Projects = repository.getProjects();
            return View(repository.getUsers());
        }

        [HttpPost]
        public ActionResult AddUserToProject(int projectId, string email)
        {
            return Json(repository.addUserToProject(projectId, email));
        }

        [HttpPost]
        public ActionResult AddProject(string projectName)
        {
            return Json(repository.addProject(new Project() { Name = projectName }));
        }

        [HttpGet]
        public ActionResult KanbanCards(int id)
        {
            Dictionary<string, string> priorityColors = new Dictionary<string, string>();

            priorityColors.Add("Trivial", "lightyellow");
            priorityColors.Add("Low", "yellow");
            priorityColors.Add("Medium", "orange");
            priorityColors.Add("High", "orangered");
            priorityColors.Add("Critical", "red");

            ViewBag.PriorityColors = priorityColors;
            ViewBag.Types = Enum.GetNames(typeof(Models.Type)).ToList();
            return PartialView(repository.getIssues(id));
        }

        [HttpPost]
        public ActionResult UpdateType(int issueId, string issueType)
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