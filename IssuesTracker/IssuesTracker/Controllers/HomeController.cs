using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using IssuesTracker.Models;

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
        public ActionResult AddIssue(Issue_for_View issue)
        {
            Issue _issue = new Issue()
            {
                Summary = issue.Summary,
                Description = issue.Description,
                Assignee = issue.Assignee,
                ProjectId = issue.ProjectId,
                Type = (Models.Type)Enum.Parse(typeof(Models.Type), issue.Type, true),
                Priority = (Priority)Enum.Parse(typeof(Priority), issue.Priority, true)
            };
            var str = repository.addIssue(_issue);
            return Json(str);
        }

        [HttpPost]
        public ActionResult EditIssue(Issue_for_View issue)
        {
            Issue _issue = new Issue()
            {
                Id = issue.Id,
                Summary = issue.Summary,
                Description = issue.Description,
                Assignee = issue.Assignee,
                ProjectId = repository.getProjectIdByName(issue.ProjectName),
                Type = (Models.Type)Enum.Parse(typeof(Models.Type), issue.Type, true),
                Priority = (Priority)Enum.Parse(typeof(Priority), issue.Priority, true)
            };
            repository.editIssue(_issue);
            return Json("Successfully edited");
        }
        [HttpGet]
        public ActionResult GetGridIssues(int id) //id = projectId
        {
            return PartialView(repository.getIssues(id));
        }
        [HttpPost]
        public ActionResult IssueModal(int projectId, int id = -1)
        {
            ViewBag.Users = repository.getUsersByProjectId(projectId);
            if (id == -1)
            {
                ViewBag.Action = "Create";
                Issue issue = new Issue();
                issue.ProjectId = projectId;
                Issue_for_View issue_for_view = repository.ViewIssues(new List<Issue>() { issue }).First();
                issue_for_view.Priority = "Trivial";
                return PartialView(issue_for_view);
            }
            ViewBag.Action = "Edit";
            return PartialView(repository.getIssue(id));
        }

        public ActionResult Management()
        {
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
