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
                _adminService.CreateClient(client, logo, profile);
            }
            else
            {
                client.UpdateUserId = IdentityService.GetUserIdFromClaim(User);
                _adminService.UpdateClient(client, logo, profile);
            }
            return RedirectToAction("Index");
        }

        [VTAuthFilter("Client")]
        public ActionResult Delete(int id)
        {
            var clientIds = IdentityService.GetClientIdsFromClaim(User);
            Client client = _lookupService.GetClientById(id);
            _adminService.DeleteClient(client);
            // TODO Error handling!
            return RedirectToAction("Index");
        }

        [VTAuthFilter("Client")]
        public ActionResult Details(int id)
        {
            Client client = _lookupService.GetClientById(id);
            // error handling
            return View("ClientDetails", client);
        }
        //private bool CreateClient(ClientView clientView)
        //{
        //    Client client = (Client)SetDefaults(ClientViewMapper.ToClient(clientView));
        //    using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
        //    {
        //        IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
        //        Timeout = CommonConfiguration.Configuration.TransactionScope_Timeout
        //    }))
        //    {
        //        int? clientId = _adminService.CreateClient(client);
        //        if (clientId.HasValue && clientId > 0)
        //        {
        //            var logo = Request.Files["Logo"];
        //            int? logoId = null;
        //            var profile = Request.Files["Profile"];
        //            int? profileId = null;
        //            if (logo != null && !string.IsNullOrWhiteSpace(logo.FileName))
        //            {
        //                logoId = UploadAsset(logo, clientId.Value, string.IsNullOrWhiteSpace(clientView.LogoName) ? logo.FileName : clientView.LogoName);
        //            }
        //            if (profile != null && !string.IsNullOrWhiteSpace(profile.FileName))
        //            {
        //                profileId = UploadAsset(profile, clientId.Value, string.IsNullOrWhiteSpace(clientView.LogoName) ? profile.FileName : clientView.LogoName);
        //            }
        //            if (profileId != null || logoId != null)
        //            {
        //                client.AssetLogoID = logoId;
        //                client.AssetProfileID = profileId;
        //                var success = _adminService.UpdateClient(client);
        //            }
        //        }
        //        transaction.Complete();
        //        return true;
        //    }
        //}
        //private bool EditClient(ClientView clientView)
        //{
        //    Client client = (Client)SetDefaults(ClientViewMapper.ToClient(clientView));
        //    using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
        //    {
        //        IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
        //        Timeout = CommonConfiguration.Configuration.TransactionScope_Timeout
        //    }))
        //    {
        //        if (client.ClientID > 0)
        //        {
        //            if (client.AssetLogoID == null || client.AssetLogoID == 0)
        //            {
        //                var logo = Request.Files["Logo"];
        //                if (logo != null && !string.IsNullOrWhiteSpace(logo.FileName))
        //                {
        //                    client.AssetLogoID = UploadAsset(logo, client.ClientID, string.IsNullOrWhiteSpace(clientView.LogoName) ? logo.FileName : clientView.LogoName);
        //                }

        //                var profile = Request.Files["Profile"];
        //                if (profile != null && !string.IsNullOrWhiteSpace(profile.FileName))
        //                {
        //                    client.AssetProfileID = UploadAsset(profile, client.ClientID, string.IsNullOrWhiteSpace(clientView.LogoName) ? profile.FileName : clientView.LogoName);
        //                }
        //            }
        //            var success = _adminService.UpdateClient(client);
        //        }
        //        transaction.Complete();
        //        return true;
        //    }
        //}
    }
}