using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VirtualTourCore.Api.Filter;
using VirtualTourCore.Api.Identity;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Api.Controllers
{
    [Authorize]
    public class LocationController : Controller
    {
        private readonly ILookupService _lookupService;
        private readonly IAdminService _adminService;
        private readonly ISecurityService _securityService;

        //private readonly IFileService _fileService;

        public LocationController(
              ILookupService lookupService
            , IAdminService adminService
            , ISecurityService securityService
            //, IFileService fileService
            )
        {
            _lookupService = lookupService;
            _adminService = adminService;
            _securityService = securityService;
            //_fileService = fileService;
        }
        // GET: Location
        public ActionResult Index()
        {
            // get available clients -> if only one redirect to client locations with client id
            // else redirect to clients and have user click the client
            var clients = _lookupService.GetClients();
            return View(clients);
        }
        [VTAuthFilter("Client")]
        public ActionResult ClientLocations(int id)
        {
            // get available clients -> if only one redirect to client locations with client id
            // else redirect to clients and have user click the client
            ViewBag.ClientId = id;
            var locations = _lookupService.GetLocationsByClientId(id);
            return View(locations);
        }

        [VTAuthFilter("Client")]
        public ActionResult Create(int id)
        {
            ModelState.Clear();
            return View("LocationCreateEdit", new Location { Id = default(int), ClientId = id });
        }

        [VTAuthFilter("Location")]
        public ActionResult Edit(int id)
        {
            var clientIds = IdentityService.GetClientIdsFromClaim(User);
            Location location = _lookupService.GetLocationByIdAndClientId(clientIds, id);
            // TODO Error handling!
            return View("LocationCreateEdit", location);
        }

        [VTAuthFilter("Location")]
        public ActionResult Details(int id)
        {
            var clientIds = IdentityService.GetClientIdsFromClaim(User);
            Location location = _lookupService.GetLocationByIdAndClientId(clientIds, id);
            // error handling
            return View("LocationDetails", location);
        }

        [HttpPost]
        public ActionResult ModifyLocations(Location location)
        {
            var locationImage = Request.Files["locationImage"];
            if (location.Id == 0)
            {
                location.CreateUserId = IdentityService.GetUserIdFromClaim(User);
                _adminService.CreateLocation(location, locationImage);

            }
            else
            {
                location.UpdateUserId = IdentityService.GetUserIdFromClaim(User);
                _adminService.UpdateLocation(location, locationImage);
            }
            return RedirectToAction("ClientLocations", new { id = location.ClientId });
        }

        [HttpPost]
        public string Delete(int id)
        {
            var clientIds = IdentityService.GetClientIdsFromClaim(User);
            Location location = _lookupService.GetLocationByIdAndClientId(clientIds, id); 
            // TODO Error handling!
            return _adminService.DeleteLocation(location);
        }
    }
}