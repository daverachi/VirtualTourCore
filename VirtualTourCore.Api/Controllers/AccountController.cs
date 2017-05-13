using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VirtualTourCore.Api.Identity;
using VirtualTourCore.Api.Models;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Models;
using VirtualTourCore.Core.Security.Principal;

namespace VirtualTourCore.Api.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        private ISecurityService _securityService;
        public AccountController(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        [Authorize]
        public ActionResult Identity()
        {
            ViewBag.Username = HttpContext.User.Identity.Name;
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = IdentityService.GetUserIdFromClaim(User);
            List<Claim> claims = claimsIdentity.Claims.ToList();
            return View(claims);
        }
        
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View(new RegisterBindingModel());
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            FederatedAuthentication.SessionAuthenticationModule.SignOut();
            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public ActionResult Login(string username, string password, string returnUrl)
        {
            if (string.IsNullOrEmpty(username))
                username = "Unknown";
            //Default value that is set if nothing is entered
            var user = _securityService.GetSecurityUserByLogin(username, password);

            if (user != null)
            {
                IdentityService.AuthorizeUser(user);

                if (!string.IsNullOrEmpty(returnUrl))
                    Response.Redirect(returnUrl);

                return RedirectToAction("Index","Home");
            }
            else
            {
                return RedirectToAction("Login"); // should be forbidden or unauth, some shit liek that
            }

        }

        [HttpPost]
        public RedirectResult Register(RegisterBindingModel model, string ReturnUrl)
        {

            var securityUser = new SecurityUser
            {
                Email = model.Email,
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PasswordPlaintext = model.Password,
                PasswordPlaintextConfirm = model.ConfirmPassword,
                Admin = User.Identity.IsAuthenticated && User.IsInRole("Admin") && model.Admin
            };
            if (_securityService.CreateUser(securityUser, model.RegistrationCode))
            {
                IdentityService.AuthorizeUser(securityUser);
            }
            return !string.IsNullOrEmpty(ReturnUrl) ? Redirect(ReturnUrl) : Redirect("/Home/Index");
        }

        public ActionResult NotAuthorized()
        {
            return View();
        }
    }
}