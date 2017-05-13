using VirtualTourCore.Common.Helper;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace VirtualTourCore.Common.Unity
{
    public class UnityHelper
    {
        public List<Assembly> _AssembliesToScan = new List<Assembly>();
        IUnityContainer container = new UnityContainer();

        public UnityHelper(List<Assembly> AssembliesToScan)
        {
            _AssembliesToScan = AssembliesToScan;
        }

        public IUnityContainer Container { get { return container; } }

        public virtual IUnityContainer BuildUnityContainer()
        {
            ConventionConfiguration(container);
            return container;
        }

        private void ConventionConfiguration(IUnityContainer container)
        {
            foreach (var asm in _AssembliesToScan)
            {
                var interfaces = asm.GetInterfaces();

                foreach (var interfaceType in interfaces)
                {
                    var currentInterfaceType = interfaceType;
                    var implementations = asm.GetImplementationsOfInterface(interfaceType);
                    if (implementations.Count > 0)
                    {
                        if (!container.IsRegistered(currentInterfaceType))
                        {
                            container.RegisterType(currentInterfaceType, implementations.ToList().First());
                        }
                    }
                }
            }
        }
    }

    public class PerHttpRequestLifetime : LifetimeManager
    {
        private readonly Guid _key = Guid.NewGuid();

        public override object GetValue()
        {
            if (HttpContext.Current == null) return null; //effectively makes it TransientLifetimeManager
            return HttpContext.Current.Items[_key];
        }

        public override void SetValue(object newValue)
        {
            if (HttpContext.Current == null) return;
            HttpContext.Current.Items[_key] = newValue;
        }

        public override void RemoveValue()
        {
            if (HttpContext.Current == null) return;
            var obj = GetValue();
            HttpContext.Current.Items.Remove(obj);
        }
    }
}
