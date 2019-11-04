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
        public ActionResult Index()
        {
            myDBContext db = new myDBContext();
            db.Projects.Add(new Project() { Name = "MyProject" });
            db.SaveChanges();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}