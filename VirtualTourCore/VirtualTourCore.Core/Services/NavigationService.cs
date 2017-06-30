using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualTourCore.Common.ValueObjects;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Services
{
    public class NavigationService : INavigationService
    {
        private IClientRepository _clientRepository;
        private ILocationRepository _locationRepository;
        private IAreaRepository _areaRepository;
        private ITourRepository _tourRepository;
        private Dictionary<Type, Func<INavigableEntity, NavigationBreadCrumbVO>> _breadCrumbFactory; 
        public NavigationService(
            IClientRepository clientRepository,
            ILocationRepository locationRepository,
            IAreaRepository areaRepository,
            ITourRepository tourRepository)
        {
            _clientRepository = clientRepository;
            _locationRepository = locationRepository;
            _areaRepository = areaRepository;
            _tourRepository = tourRepository;
            _breadCrumbFactory = new Dictionary<Type, Func<INavigableEntity, NavigationBreadCrumbVO>> {
                { typeof(Client), (x) => GetClientBreadCrumb(x, new NavigationBreadCrumbVO()) },
                { typeof(Location), (x) => GetLocationBreadCrumb(x, new NavigationBreadCrumbVO()) },
                { typeof(Area), (x) => GetAreaBreadCrumb(x, new NavigationBreadCrumbVO()) },
                { typeof(Tour), (x) => GetTourBreadCrumb(x, new NavigationBreadCrumbVO()) },
            };
        }

        public NavigationBreadCrumbVO GetBreadCrumbs(INavigableEntity item)
        {
            return _breadCrumbFactory[item.GetType()](item);
        }

        private NavigationBreadCrumbVO GetTourBreadCrumb(INavigableEntity _tour, NavigationBreadCrumbVO nav)
        {
            var tour = _tour as Tour;
            nav.TourId = tour.Id;
            nav.TourName = tour.Name;
            var area = _areaRepository.GetById(tour.AreaId);
            return GetAreaBreadCrumb(area, nav);
        }

        private NavigationBreadCrumbVO GetAreaBreadCrumb(INavigableEntity _area, NavigationBreadCrumbVO nav)
        {
            var area = _area as Area;
            nav.AreaId = area.Id;
            nav.AreaName = area.Name;
            var location = _locationRepository.GetById(area.LocationId);
            return GetLocationBreadCrumb(location, nav);
        }

        private NavigationBreadCrumbVO GetLocationBreadCrumb(INavigableEntity _location, NavigationBreadCrumbVO nav)
        {
            var location = _location as Location;
            nav.LocationId = location.Id;
            nav.LocationName = location.Name;
            var client = _clientRepository.GetById(location.ClientId);
            return GetClientBreadCrumb(client, nav);
        }

        private NavigationBreadCrumbVO GetClientBreadCrumb(INavigableEntity _client, NavigationBreadCrumbVO nav)
        {
            var client = _client as Client;
            nav.ClientId = client.Id;
            nav.ClientName = client.Name;
            return nav;
        }
    }
}
