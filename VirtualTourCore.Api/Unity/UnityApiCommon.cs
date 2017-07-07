using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using VirtualTourCore.Api.Areas.HelpPage.Controllers;
using VirtualTourCore.Api.Filter;
using VirtualTourCore.Common.DataAccess;
using VirtualTourCore.Common.DataAccess.Interfaces;
using VirtualTourCore.Common.Unity;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Repositories;
using VirtualTourCore.Core.Services;

namespace VirtualTourCore.Api.Unity
{
    public static class UnityApiCommon
    {
        public static IUnityContainer GetUnityContainer()
        {
            var helper = new UnityHelper(GetAssembliesToLoad());
            var container = helper.Container;

            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerHttpRequestLifetime());
            container.RegisterType<HttpContextBase, HttpContextWrapper>();
            container.RegisterType<HttpContext>(new InjectionFactory(c => HttpContext.Current));
            container.RegisterType<HelpController>(new TransientLifetimeManager());
            container.RegisterInstance<HelpController>(new HelpController(), new TransientLifetimeManager());
            container = helper.BuildUnityContainer();
            return container;
        }

        internal static List<Assembly> GetAssembliesToLoad()
        {
            List<Assembly> assembliesToLoad = new List<Assembly>();

            assembliesToLoad.Add(typeof(EntityBase).Assembly);
            assembliesToLoad.Add(typeof(SecurityService).Assembly); //Initial Repository
            assembliesToLoad.Add(Assembly.GetExecutingAssembly());

            return assembliesToLoad;
        }
    }
}