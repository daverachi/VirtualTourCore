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
        private ILookupService _lookupService;
        public AccountController(ISecurityService securityService, ILookupService lookupService)
        {
            _securityService = securityService;
            _lookupService = lookupService;
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
            var regModel = new RegisterBindingModel();
            if (User.IsInRole("Admin"))
            {
                regModel.RegistrationCode = IdentityService.GetUserCodeFromClaim(User);
            }
            return View(regModel);
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
                return RedirectToAction("Login"); // should be forbidden or unauth
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
            if (_securityService.CreateUser(securityUser, model.RegistrationCode) && !(User.Identity.IsAuthenticated && User.IsInRole("Admin")))
            {
                IdentityService.AuthorizeUser(securityUser);
            }
            return !string.IsNullOrEmpty(ReturnUrl) ? Redirect(ReturnUrl) : Redirect("/Home/Index");
        }

        [HttpPost]
        public string ValidateRegistrationCode([System.Web.Http.FromBody]string code)
        {
            return _lookupService.ValidRegistrationCode(code) ? "" : "Registration Code is not valid!";
        }

        [HttpPost]
        public string ValidateUsername([System.Web.Http.FromBody]string username)
        {
            return _lookupService.ValidUsername(username) ? "" : "Username is not available, please choose another!";
        }
        [HttpPost]
        public string ValidateUserEmail([System.Web.Http.FromBody]string email)
        {
            return _lookupService.ValidEmail(email) ? "" : "There is already an account associated with that email address, please choose another!";
        }
        public ActionResult NotAuthorized()
        {
            return View();
        }
    }
}