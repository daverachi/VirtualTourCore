using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualTourCore.Common.ValueObjects;

namespace VirtualTourCore.Core.Interfaces
{
    public interface INavigationService
    {
        NavigationBreadCrumbVO GetBreadCrumbs(INavigableEntity item);
    }
}
