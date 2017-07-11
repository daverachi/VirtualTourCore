using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using VirtualTourCore.Api.Filter;
using VirtualTourCore.Api.Identity;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Api.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly ILookupService _lookupService;
        private readonly IAdminService _adminService;
        private readonly ISecurityService _securityService;
        private readonly INavigationService _navigationService;

        //private readonly IFileService _fileService;

        public ClientController(
              ILookupService lookupService
            , IAdminService adminService
            , ISecurityService securityService
            , INavigationService navigationService
            //, IFileService fileService
            )
        {
            _lookupService = lookupService;
            _adminService = adminService;
            _securityService = securityService;
            _navigationService = navigationService;
            //_fileService = fileService;
        }
        // GET: Client
        public ActionResult Index()
        {
            var clients = _lookupService.GetClients().SetClientAccess(User);
            return View("ClientList", clients);
        }

        [Authorize(Roles="Admin")]
        public ActionResult Create()
        {
            return View("ClientCreateEdit", new Client());
        }

        [VTAuthFilter("Client")]
        public ActionResult Edit(int id)
        {
            var clientIds = IdentityService.GetClientIdsFromClaim(User);
            Client client = _lookupService.GetClientById(id);
            // TODO Error handling!
            return View("ClientCreateEdit", client);
        }
        [HttpPost]
        public ActionResult ModifyClients(Client client)
        {
            var logo = Request.Files["logo"];
            var profile = Request.Files["profile"];
            if (client.Id == 0)
            {
                client.CreateUserId = IdentityService.GetUserIdFromClaim(User);
                var clientId = _adminService.CreateClient(client, logo, profile);
                if(clientId != null)
                {
                    IdentityService.AddClientClaim(User, clientId.Value);
                }
            }
            else
            {
                client.UpdateUserId = IdentityService.GetUserIdFromClaim(User);
                _adminService.UpdateClient(client, logo, profile);
            }
            return RedirectToAction("Index");
        }

        [VTAuthFilter("Client")]
        [HttpPost]
        public string Delete(int id)
        {
            var clientIds = IdentityService.GetClientIdsFromClaim(User);
            Client client = _lookupService.GetClientById(id);
            return _adminService.DeleteClient(client);
        }


        [VTAuthFilter("Client")]
        [HttpPost]
        public string IssueRegistration(int id, [System.Web.Http.FromBody]string email)
        {
            return "";
        }

        [VTAuthFilter("Client")]
        public ActionResult Details(int id)
        {
            Client client = _lookupService.GetClientById(id);
            // error handling
            return View("ClientDetails", client);
        }
    }
}