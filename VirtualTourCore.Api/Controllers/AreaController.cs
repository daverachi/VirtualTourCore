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
    public class AreaController : Controller
    {
        private readonly ILookupService _lookupService;
        private readonly IAdminService _adminService;
        private readonly ISecurityService _securityService;

        //private readonly IFileService _fileService;

        public AreaController(
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
        // GET: Area
        public ActionResult Index()
        {
            // get available clients -> if only one redirect to client areas with client id
            // else redirect to clients and have user click the client
            var clients = _lookupService.GetClients();
            return View(clients);
        }
        [VTAuthFilter("Location")]
        public ActionResult ClientAreas(int cId, int id)
        {
            // get available clients -> if only one redirect to client areas with client id
            // else redirect to clients and have user click the client
            ViewBag.ClientId = cId;
            ViewBag.LocationId = id;
            // update to call including client id for better security.
            var areas = _lookupService.GetAreasByLocationId(id);
            return View(areas);
        }

        public ActionResult Create(int cId, int id)
        {
            ModelState.Clear();
            return View("AreaCreateEdit", new Area { Id = default(int), LocationId = id, ClientId = cId });
        }

        public ActionResult Edit(int id)
        {
            var clientIds = IdentityService.GetClientIdsFromClaim(User);
            Area area = _lookupService.GetAreaByIdAndClientId(clientIds, id);
            // TODO Error handling!
            return View("AreaCreateEdit", area);
        }
        public ActionResult Details(int id)
        {
            var clientIds = IdentityService.GetClientIdsFromClaim(User);
            Area area = _lookupService.GetAreaByIdAndClientId(clientIds, id);
            // TODO Error handling!
            return View("AreaDetails", area);
        }

        [HttpPost]
        public ActionResult ModifyAreas(Area area)
        {
            if (area.Id == 0)
            {
                area.CreateUserId = IdentityService.GetUserIdFromClaim(User);
                _adminService.CreateArea(area);
            }
            else
            {
                area.UpdateUserId = IdentityService.GetUserIdFromClaim(User);
                _adminService.UpdateArea(area);
            }
            // todo : do something about success or failure
            return RedirectToAction("ClientAreas", new { cId = area.ClientId, id = area.LocationId });
        }

        //[VTAuthFilter("Client")]
        public ActionResult Delete(int id)
        {
            var clientIds = IdentityService.GetClientIdsFromClaim(User);
            Area area = _lookupService.GetAreaByIdAndClientId(clientIds, id);
            _adminService.DeleteArea(area);
            // TODO Error handling!
            return RedirectToAction("ClientAreas", new { cId = area.ClientId, id = area.LocationId });
        }
    }
}