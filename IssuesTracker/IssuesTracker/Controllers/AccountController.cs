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
                    SetClaimsIdentity(model.Email, repository.getUserRoleName(model.Email));
                    return RedirectToAction("Index", "Home");
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
                    userManager.AddToRole(userManager.FindByEmail(model.Email).Id, model.Role);
                    if (validation)
                    {
                        SetClaimsIdentity(model.Email, model.Role);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User with same Email already exists");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        void SetClaimsIdentity(string email, string role)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                       new[] {
                            new Claim(ClaimTypes.Name, email.Split('@')[0]),
                            new Claim(ClaimTypes.Email, email),
                            new Claim(ClaimTypes.Role, role)
                       }, DefaultAuthenticationTypes.ApplicationCookie);
            HttpContext.GetOwinContext().Authentication.SignIn(
                new AuthenticationProperties { IsPersistent = false }, identity);
        }
    }
}