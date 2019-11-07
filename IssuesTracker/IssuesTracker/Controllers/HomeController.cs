﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IssuesTracker.Models;

namespace IssuesTracker.Controllers
{
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
            Issue iss = new Issue()
            {
                Summary = issue.Summary,
                Description = issue.Description,
                Assignee = issue.Assignee,
                ProjectId = issue.ProjectId,
                Type = (Models.Type)Enum.Parse(typeof(Models.Type), issue.Type, true),
                Priority = (Priority)Enum.Parse(typeof(Priority), issue.Priority, true)
            };
            repository.addIssue(iss);
            return Json("Successfully created");
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
                Priority = (Priority) Enum.Parse(typeof(Priority), issue.Priority, true)
            };
            repository.editIssue(iss);
            return Json("Successfully edited");
        }
        [HttpGet]
        public ActionResult GetIssues(int id)//id = ProjectId
        {
            return Json(repository.getIssues(id), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetIssue(int id)//id = IssueId
        {
            return Json(repository.getIssue(id), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Kanban()
        {

            return Json(null, JsonRequestBehavior.AllowGet);
        }
       
    }
}