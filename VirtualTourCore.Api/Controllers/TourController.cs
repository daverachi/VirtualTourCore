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
    public class TourController : Controller
    {
        private readonly ILookupService _lookupService;
        private readonly IAdminService _adminService;
        private readonly ISecurityService _securityService;

        //private readonly IFileService _fileService;

        public TourController(
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
        // GET: Tour
        public ActionResult Index()
        {
            // get available clients -> if only one redirect to client tours with client id
            // else redirect to clients and have user click the client
            var clients = _lookupService.GetClients();
            return View(clients);
        }
        [VTAuthFilter("Tour")]
        public ActionResult ClientTours(int id, int areaId)
        {
            // get available clients -> if only one redirect to client tours with client id
            // else redirect to clients and have user click the client
            ViewBag.ClientId = id;
            ViewBag.AreaId = areaId;
            // update to call including client id for better security.
            var tours = _lookupService.GetToursByAreaId(areaId);
            return View(tours);
        }

        public ActionResult Create(int id, int areaId)
        {
            ModelState.Clear();
            return View("TourCreateEdit", new Tour { Id = default(int), AreaId = areaId, ClientId = id });
        }

        public ActionResult Edit(int id)
        {
            var clientIds = IdentityService.GetClientIdsFromClaim(User);
            Tour tour = _lookupService.GetTourByIdAndClientId(clientIds, id);
            // TODO Error handling!
            return View("TourCreateEdit", tour);
        }
        public ActionResult Details(int id)
        {
            var clientIds = IdentityService.GetClientIdsFromClaim(User);
            Tour tour = _lookupService.GetTourByIdAndClientId(clientIds, id);
            // TODO Error handling!
            return View("TourDetails", tour);
        }

        [HttpPost]
        public ActionResult ModifyTours(Tour tour)
        {
            if (tour.Id == 0)
            {
                tour.CreateUserId = IdentityService.GetUserIdFromClaim(User);
                _adminService.CreateTour(tour);
            }
            else
            {
                tour.UpdateUserId = IdentityService.GetUserIdFromClaim(User);
                _adminService.UpdateTour(tour);
            }
            // todo : do something about success or failure
            return RedirectToAction("ClientTours", new { id = tour.ClientId, areaId = tour.AreaId });
        }

        //[VTAuthFilter("Client")]
        public ActionResult Delete(int id)
        {
            var clientIds = IdentityService.GetClientIdsFromClaim(User);
            Tour tour = _lookupService.GetTourByIdAndClientId(clientIds, id);
            _adminService.DeleteTour(tour);
            // TODO Error handling!
            return RedirectToAction("ClientTours", new { id = tour.ClientId, areaId = tour.AreaId });
        }
    }
}