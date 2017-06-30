using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VirtualTourCore.Api.Identity;
using VirtualTourCore.Api.Models;
using VirtualTourCore.Common.ValueObjects;
using VirtualTourCore.Core.Interfaces;

namespace VirtualTourCore.Api.Controllers
{
    public class NavigationController : Controller
    {
        private readonly INavigationService _navigationService;
        private readonly ILookupService _lookupService;

        public NavigationController(INavigationService navigationService, ILookupService lookupService)
        {
            _navigationService = navigationService;
            _lookupService = lookupService;
        }
        
        [ChildActionOnly]
        public ActionResult GetBreadCrumbs()
        {
            RouteValueDictionary navigationRouteValues = new RouteValueDictionary(ControllerContext.ParentActionViewContext.RouteData.Values);
            string currentController = ((string)navigationRouteValues["controller"]);
            string currentAction = ((string)navigationRouteValues["action"]);
            int? currentId = (((string)navigationRouteValues["id"]) != null) ? Convert.ToInt32((string)navigationRouteValues["id"]) : (int?)null;
            NavigationBreadCrumbVO breadCrumbs = new NavigationBreadCrumbVO();
            switch (currentController.ToLower())
            {
                case "client":
                    //breadCrumbs = GetClientBreadCrumbs(currentAction, currentId);
                    // probably no need for these
                    break;
                case "location":
                    breadCrumbs = GetLocationBreadCrumbs(currentAction, currentId);
                    break;
                case "area":
                    breadCrumbs = GetAreaBreadCrumbs(currentAction, currentId);
                    break;
                case "tour":
                    breadCrumbs = GetTourBreadCrumbs(currentAction, currentId);
                    break;
            }
            return PartialView("_BreadCrumbs", breadCrumbs);
        }

        //private NavigationBreadCrumbVO GetClientBreadCrumbs(string action, int? id)
        //{
        //    if(!string.IsNullOrWhiteSpace(action) || id == null)
        //    {
        //        return new NavigationBreadCrumbVO();
        //    }
        //    else
        //    {
        //        return _navigationService.GetBreadCrumbs(_lookupService.GetClientById(id.Value));
        //    }
        //}
        private NavigationBreadCrumbVO GetLocationBreadCrumbs(string action, int? id)
        {
            var breadCrumb = new NavigationBreadCrumbVO();
            switch (action.ToLower())
            {
                case "clientlocations":
                    breadCrumb = _navigationService.GetBreadCrumbs(_lookupService.GetClientById(id.Value));
                    break;
                case "edit":
                case "details":
                case "create":
                    var location = _lookupService.GetLocationByIdAndClientId(IdentityService.GetClientIdsFromClaim(User), id.Value);
                    breadCrumb = _navigationService.GetBreadCrumbs(_lookupService.GetClientById(location.ClientId));
                    break;
            }
            return breadCrumb;
        }
        private NavigationBreadCrumbVO GetAreaBreadCrumbs(string action, int? id)
        {
            var breadCrumb = new NavigationBreadCrumbVO();
            var userClientAccess = IdentityService.GetClientIdsFromClaim(User);
            switch (action.ToLower())
            {
                case "clientareas":
                    breadCrumb = _navigationService.GetBreadCrumbs(_lookupService.GetLocationByIdAndClientId(userClientAccess, id.Value));
                    break;
                case "edit":
                case "details":
                case "create":
                    var area = _lookupService.GetAreaByIdAndClientId(userClientAccess, id.Value);
                    var clientId = Request.QueryString["cId"];
                    // should we confirm the client id matches the area result?
                    breadCrumb = _navigationService.GetBreadCrumbs(_lookupService.GetLocationByIdAndClientId(userClientAccess, area.LocationId));
                    break;
            }
            return breadCrumb;
        }
        private NavigationBreadCrumbVO GetTourBreadCrumbs(string action, int? id)
        {
            var breadCrumb = new NavigationBreadCrumbVO();
            var userClientAccess = IdentityService.GetClientIdsFromClaim(User);
            switch (action.ToLower())
            {
                case "clienttours":
                    breadCrumb = _navigationService.GetBreadCrumbs(_lookupService.GetAreaByIdAndClientId(userClientAccess, id.Value));
                    break;
                case "edit":
                case "details":
                case "create":
                    var tour = _lookupService.GetTourByIdAndClientId(userClientAccess, id.Value);
                    var clientId = Request.QueryString["cId"];
                    // should we confirm the client id matches the area result?
                    breadCrumb = _navigationService.GetBreadCrumbs(_lookupService.GetAreaByIdAndClientId(userClientAccess, tour.AreaId));
                    break;
            }
            return breadCrumb;
        }
    }
}