using Microsoft.Practices.Unity;
using System.Web.Http;
using System.Web.Mvc;

namespace VirtualTourCore.Api.Unity
{
    public class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = UnityApiCommon.GetUnityContainer();
            DependencyResolver.SetResolver(new Microsoft.Practices.Unity.Mvc.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}