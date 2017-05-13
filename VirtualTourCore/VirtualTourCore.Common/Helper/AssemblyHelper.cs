using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VirtualTourCore.Common.Helper
{
    public static class AssemblyHelper
    {
        public static IEnumerable<Type> GetInterfaces(this Assembly assembly)
        {
            return assembly.GetTypes().Where(t => t.IsInterface);
        }

        public static IList<Type> GetImplementationsOfInterface(this Assembly assembly, Type interfaceType)
        {
            var implementations = new List<Type>();

            var concreteTypes = assembly.GetTypes().Where(t =>
                !t.IsInterface &&
                !t.IsAbstract &&
                interfaceType.IsAssignableFrom(t));

            concreteTypes.ToList().ForEach(implementations.Add);

            return implementations;
        }
    }
}
