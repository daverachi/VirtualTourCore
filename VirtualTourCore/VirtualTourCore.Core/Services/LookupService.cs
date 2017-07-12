using System;
using System.Collections.Generic;
using System.Linq;
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
        private IRegistrationCodeRepository _registrationCodeRepository;
        private ISecurityUserRepository _securityUserRepository;
        public LookupService(
            IClientRepository clientRepository,
            ILocationRepository locationRepository,
            IAreaRepository areaRepository,
            ITourRepository tourRepository,
            IAssetStoreRepository assetStoreRepository,
            IRegistrationCodeRepository registrationCodeRepository,
            ISecurityUserRepository securityUserRepository
            )
        {
            _clientRepository = clientRepository;
            _locationRepository = locationRepository;
            _areaRepository = areaRepository;
            _tourRepository = tourRepository;
            _assetStoreRepository = assetStoreRepository;
            _registrationCodeRepository = registrationCodeRepository;
            _securityUserRepository = securityUserRepository;
        }
        public Client GetClientByGuid(Guid guid)
        {
            var client = _clientRepository.GetByGuid(guid);
            if (client != null)
            {
                client.AssetLogo = PopulateAssetStoreById(client.AssetLogoId);
                client.AssetProfile = PopulateAssetStoreById(client.AssetProfileId);
            }
            return client;
        }
        public Client GetClientById(int id)
        {
            var client = _clientRepository.GetById(id);
            if(client != null)
            {
                client.AssetLogo = PopulateAssetStoreById(client.AssetLogoId);
                client.AssetProfile = PopulateAssetStoreById(client.AssetProfileId);
            }
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
            var locations = _locationRepository.GetByClientId(id);
            if (locations != null)
            {
                foreach(var location in locations.Where(x=>x.AssetLocationId != null).ToList())
                {
                    location.AssetLocation = PopulateAssetStoreById(location.AssetLocationId);
                }
            }
            return locations;
        }
        public Location GetLocationByIdAndClientId(IEnumerable<string> clientIds, int id)
        {
            var location = _locationRepository.GetByIdFilteredByClientIds(id, GetValidClientIds(clientIds));
            if(location != null)
            {
                location.AssetLocation = PopulateAssetStoreById(location.AssetLocationId);
            }
            return location;
        }

        public IEnumerable<Area> GetAreasByClientId(int id)
        {
            return _areaRepository.GetByClientId(id);
        }

        public Area GetAreaByIdAndClientId(IEnumerable<string> clientIds, int id)
        {
            var area = _areaRepository.GetByIdAndByClientId(id, GetValidClientIds(clientIds));
            if(area != null)
            {
                area.AssetArea = PopulateAssetStoreById(area.AssetAreaId);
            }
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
            if(tour != null)
            {
                tour.AssetTourThumbnail = PopulateAssetStoreById(tour.AssetTourThumbnailId);
                tour.KrPanoTour = PopulateAssetStoreById(tour.KrPanoTourId);
            }
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

        public bool ValidRegistrationCode(string code)
        {
            bool valid = false;
            Guid guid = Guid.NewGuid();
            if(Guid.TryParse(code, out guid))
            {
                valid = _registrationCodeRepository.GetByGuid(guid) != null ? true : false;
            }
            return valid;
        }
        public bool ValidEmail(string email)
        {
            return _securityUserRepository.GetByEmail(email) == null ? true : false;
        }
        public bool ValidUsername(string username)
        {
            return _securityUserRepository.GetByUsername(username) == null ? true : false;
        }
    }
}
