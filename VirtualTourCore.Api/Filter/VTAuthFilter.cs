using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VirtualTourCore.Api.Identity;

namespace VirtualTourCore.Api.Filter
{
    public class VTAuthFilter : AuthorizeAttribute
    {
        public VTAuthFilter(string entityType)
        {
            this.EntityType = entityType;
        }

        public string EntityType { get; set; }
        private int ErrorCode { get; set; }
        //Called when access is denied
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //User isn't logged in
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Account", action = "Login" })
                );
            }
            //User is logged in but has no access
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Account", action = "NotAuthorized" })
                );
            }
        }

        //Core authentication, called before each action
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = HttpContext.Current.User;
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }
            var rd = httpContext.Request.RequestContext.RouteData;
            var id = rd.Values["id"] as string;
            if (string.IsNullOrEmpty(id))
            {
                ErrorCode = 1;
                return false;
            }
            switch(EntityType)
            {
                case ("Client"):
                    return (IdentityService.GetClientIdsFromClaim(user)).Contains(id);
            }
            return false;
        }
    }
}