using System;
using System.Collections.Generic;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Services
{
    public class LookupService : ILookupService
    {
        private IClientRepository _clientRepository;
        private ILocationRepository _locationRepository;
        private IAreaRepository _areaRepository;
        private ITourRepository _tourRepository;
        private IAssetStoreRepository _assetStoreRepository;
        public LookupService(
            IClientRepository clientRepository,
            ILocationRepository locationRepository,
            IAreaRepository areaRepository,
            ITourRepository tourRepository,
            IAssetStoreRepository assetStoreRepository
            )
        {
            _clientRepository = clientRepository;
            _locationRepository = locationRepository;
            _areaRepository = areaRepository;
            _tourRepository = tourRepository;
            _assetStoreRepository = assetStoreRepository;
        }
        public Client GetClientByGuid(Guid guid)
        {
            var client = _clientRepository.GetByGuid(guid);
            client.AssetLogo = PopulateAssetStoreById(client.AssetLogoId);
            client.AssetProfile = PopulateAssetStoreById(client.AssetProfileId);
            return client;
        }
        public Client GetClientById(int id)
        {
            var client = _clientRepository.GetById(id);
            client.AssetLogo = PopulateAssetStoreById(client.AssetLogoId);
            client.AssetProfile = PopulateAssetStoreById(client.AssetProfileId);
            return client;
        }
        public IEnumerable<Client> GetClientByGuids(IEnumerable<Guid> guids)
        {
            return _clientRepository.GetByGuids(guids);
        }
        public IEnumerable<Client> GetClients()
        {
            return _clientRepository.Get();
        }
        public IEnumerable<Location> GetLocationsByClientId(int id)
        {
            return _locationRepository.GetByClientId(id);
        }
        public Location GetLocationByIdAndClientId(IEnumerable<string> clientIds, int id)
        {
            var location = _locationRepository.GetByIdFilteredByClientIds(id, GetValidClientIds(clientIds));
            location.AssetLocation = PopulateAssetStoreById(location.AssetLocationId);
            return location;
        }

        public IEnumerable<Area> GetAreasByClientId(int id)
        {
            return _areaRepository.GetByClientId(id);
        }

        public Area GetAreaByIdAndClientId(IEnumerable<string> clientIds, int id)
        {
            var area = _areaRepository.GetByIdAndByClientId(id, GetValidClientIds(clientIds));
            area.AssetArea = PopulateAssetStoreById(area.AssetAreaId);
            return area;
        }

        public IEnumerable<Area> GetAreasByLocationId(int id)
        {
            return _areaRepository.GetByLocationId(id);
        }

        public IEnumerable<Tour> GetToursByAreaId(int areaId)
        {
            return _tourRepository.GetByAreaId(areaId);
        }

        public Tour GetTourByIdAndClientId(IEnumerable<string> clientIds, int id)
        {
            var tour = _tourRepository.GetByIdAndClientId(id, GetValidClientIds(clientIds));
            tour.AssetTourThumbnail = PopulateAssetStoreById(tour.AssetTourThumbnailId);
            tour.KrPanoTour = PopulateAssetStoreById(tour.KrPanoTourId);
            return tour;
        }

        private AssetStore PopulateAssetStoreById(int? assetStoreId)
        {
            AssetStore asset = null;
            if (assetStoreId != null)
            {
                asset = _assetStoreRepository.GetById(assetStoreId.Value);
            }
            return asset;
        }

        // this should probably just be a helper method somewhere for parsing.
        private IEnumerable<int> GetValidClientIds(IEnumerable<string> clientIds)
        {
            List<int> _validClientIds = new List<int>();
            foreach (var clientId in clientIds)
            {
                int validClientId = 0;
                if (int.TryParse(clientId, out validClientId))
                {
                    _validClientIds.Add(validClientId);
                }
            }
            return _validClientIds;
        }
    }
}
