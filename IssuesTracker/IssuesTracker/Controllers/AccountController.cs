using IssuesTracker.Models;
using IssuesTracker.Models.IdentityViews;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace IssuesTracker.Controllers
{
    public class AccountController : Controller
    {
        Repository repository = new Repository();
        SecureString secureService = new SecureString();

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
                bool validation = repository.isValidUser(model.Email, secureService.Encryptor(model.Password));
                if (validation)
                {
                    if(model.Email.ToLower().Contains("admin"))
                    {
                        SetClaimsIdentity(repository.getUsernameByEmail(model.Email), model.Email, "Admin");
                    }
                    else
                    {
                        SetClaimsIdentity(repository.getUsernameByEmail(model.Email), model.Email, repository.getUserRoleName(model.Email));
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "User with the same Email and Password doesn't exist");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            ViewBag.Roles = repository.getRoleNames();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            AppUserManager userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            IAuthenticationManager authManager = HttpContext.GetOwinContext().Authentication;
            if (ModelState.IsValid)
            {
                bool validation = repository.checkUserByEmail(model.Email);
                if (!validation)
                {
                    AppUser user = new AppUser()
                    {
                        UserName = model.Email.Split('@')[0],
                        Email = model.Email,
                        PasswordHash = secureService.Encryptor(model.Password),
                    };
                    repository.addUser(user);
                    validation = repository.isValidUser(model.Email, secureService.Encryptor(model.Password));
                    if (model.Role != "--None--")
                    {
                        userManager.AddToRole(userManager.FindByEmail(model.Email).Id, model.Role);
                    }                
                    if (validation)
                    {
                        if(model.Email.ToLower().Contains("admin"))
                        {
                            SetClaimsIdentity(model.Email.Split('@')[0], model.Email, "Admin");
                        }
                        else
                        {
                            SetClaimsIdentity(model.Email.Split('@')[0], model.Email, model.Role);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User with the same Email already exists");
                }
            }
            ViewBag.Roles = repository.getRoleNames();
            return View(model);
        }

        [HttpGet]
        public ActionResult UpdateUser()
        {
            if(!repository.getEmailByUsername(User.Identity.Name).ToLower().Contains("admin"))
            {
                ViewBag.Role = repository.getUserRoleName(repository.getEmailByUsername(User.Identity.Name));
            }
            else
            {
                ViewBag.Role = "Admin";
            }
            ViewBag.Roles = repository.getRoleNames();
            ViewBag.Email = repository.getEmailByUsername(User.Identity.Name);
            return View();
        }

        [HttpPost]
        public ActionResult UpdateUser(UpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool validation = repository.isValidUser(model.Email, secureService.Encryptor(model.oldPassword));
                if (validation)
                {
                    repository.updateUser(model.Email, model.Username, secureService.Encryptor(model.newPassword));
                    if (model.Email.ToLower().Contains("admin"))
                    {
                        SetClaimsIdentity(model.Username, model.Email, "Admin");
                    }
                    else
                    {                       
                        SetClaimsIdentity(model.Username, model.Email, repository.getUserRoleName(model.Email));
                    }                   
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Wrong data");
                }
            }
            ViewBag.Roles = repository.getRoleNames();
            ViewBag.Email = repository.getEmailByUsername(User.Identity.Name);
            return View(model);
        }

        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        private void SetClaimsIdentity(string userName, string userEmail, string roleName)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                       new[] {
                            new Claim(ClaimTypes.Name, userName),
                            new Claim(ClaimTypes.Email, userEmail),
                            new Claim(ClaimTypes.Role, roleName)
                       }, DefaultAuthenticationTypes.ApplicationCookie);
            HttpContext.GetOwinContext().Authentication.SignIn(
                new AuthenticationProperties { IsPersistent = false }, claimsIdentity);
        }
    }
}