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
        public ActionResult ClientTours(int cId, int id)
        {
            // get available clients -> if only one redirect to client tours with client id
            // else redirect to clients and have user click the client
            ViewBag.ClientId = cId;
            ViewBag.AreaId = id;
            // update to call including client id for better security.
            var tours = _lookupService.GetToursByAreaId(id);
            return View(tours);
        }

        public ActionResult Create(int cId, int id)
        {
            ModelState.Clear();
            ViewBag.HasKrPano = false;
            return View("TourCreateEdit", new Tour { Id = default(int), AreaId = id, ClientId = cId });
        }

        public ActionResult Edit(int id)
        {
            var clientIds = IdentityService.GetClientIdsFromClaim(User);
            Tour tour = _lookupService.GetTourByIdAndClientId(clientIds, id);
            // TODO Error handling!
            ViewBag.HasKrPano = tour.KrPanoTourId != null && !string.IsNullOrWhiteSpace(tour.KrPanoTour.FullPath());
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
            var tourThumb = Request.Files["tourThumb"];
            var krPanoZip = Request.Files["krPanoZip"];
            if (tour.Id == 0)
            {
                tour.CreateUserId = IdentityService.GetUserIdFromClaim(User);
                _adminService.CreateTour(tour, tourThumb, krPanoZip);
            }
            else
            {
                tour.UpdateUserId = IdentityService.GetUserIdFromClaim(User);
                _adminService.UpdateTour(tour, tourThumb, krPanoZip);
            }
            // todo : do something about success or failure
            return RedirectToAction("ClientTours", new { cId = tour.ClientId, id = tour.AreaId });
        }

        //[VTAuthFilter("Client")]
        public ActionResult Delete(int id)
        {
            var clientIds = IdentityService.GetClientIdsFromClaim(User);
            Tour tour = _lookupService.GetTourByIdAndClientId(clientIds, id);
            _adminService.DeleteTour(tour);
            // TODO Error handling!
            return RedirectToAction("ClientTours", new { cId = tour.ClientId, id = tour.AreaId });
        }
    }
}