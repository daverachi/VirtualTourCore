using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VirtualTourCore.Api.Models;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Api.Controllers
{
    public class HomeController : Controller
    {
        private ILookupService _lookupService;
        public HomeController(ISecurityService securityService, ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }
        
    }
}
