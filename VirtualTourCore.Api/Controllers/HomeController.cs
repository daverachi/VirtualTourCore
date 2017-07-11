using System.Web.Mvc;
using VirtualTourCore.Core.Interfaces;

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
