using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Api.Controllers
{
    public class CustomizationController : Controller
    {
        private readonly ILookupService _lookupService;
        private readonly IAdminService _adminService;

        public CustomizationController(
              ILookupService lookupService
            , IAdminService adminService
            )
        {
            _lookupService = lookupService;
            _adminService = adminService;
        }
        // GET: Customization
        public ActionResult Index(int clientId, int locationId, int areaId, int tourId, int? id)
        {
            var customization = _lookupService.PopulateCustomizationBasedOnEntityValues(clientId, locationId, areaId, tourId, id);
            return View(customization);
        }
    }
}