using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IssuesTracker.Models;

namespace IssuesTracker.Controllers
{
    public class HomeController : Controller
    {
        Repository r = new Repository();
        public ActionResult Index()
        {
            return View(r.getProjects());
        }
        [HttpPost]
        public ActionResult AddIssue(Issue issue)
        {
            return Json(issue);
        }
        [HttpPost]
        public ActionResult EditIssue(Issue issue)
        {
            r.editIssue(issue);
            return Json(issue);
        }
        [HttpGet]
        public ActionResult GetIssues(int id)
        {
            return Json(r.getIssues(id), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Kanban()
        {

            return Json(null, JsonRequestBehavior.AllowGet);
        }
       
    }
}