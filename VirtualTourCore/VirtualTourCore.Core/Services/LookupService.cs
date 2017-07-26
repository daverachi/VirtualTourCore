using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Services
{
    public class LookupService : ILookupService
    {
        #region Private Members
        private IClientRepository _clientRepository;
        private ILocationRepository _locationRepository;
        private IAreaRepository _areaRepository;
        private ITourRepository _tourRepository;
        private IAssetStoreRepository _assetStoreRepository;
        private IRegistrationCodeRepository _registrationCodeRepository;
        private ISecurityUserRepository _securityUserRepository;
        private IItemStatusRepository _itemStatusRepository;
        private ICustomizationRepository _customizationRepository;
        #endregion
        public LookupService(
            IClientRepository clientRepository,
            ILocationRepository locationRepository,
            IAreaRepository areaRepository,
            ITourRepository tourRepository,
            IAssetStoreRepository assetStoreRepository,
            IRegistrationCodeRepository registrationCodeRepository,
            ISecurityUserRepository securityUserRepository,
            IItemStatusRepository itemStatusRepository,
            ICustomizationRepository customizationRepository
            )
        {
            _clientRepository = clientRepository;
            _locationRepository = locationRepository;
            _areaRepository = areaRepository;
            _tourRepository = tourRepository;
            _assetStoreRepository = assetStoreRepository;
            _registrationCodeRepository = registrationCodeRepository;
            _securityUserRepository = securityUserRepository;
            _itemStatusRepository = itemStatusRepository;
            _customizationRepository = customizationRepository;
        }

        #region Client
        public Client GetClientByGuid(Guid guid)
        {
            var client = _clientRepository.GetByGuid(guid);
            if (client != null)
            {
                client.AssetLogo = PopulateAssetStoreById(client.AssetLogoId);
                client.AssetProfile = PopulateAssetStoreById(client.AssetProfileId);
                client.Customization = PopulateCustomizationById(client.CustomizationId);
                client.ItemStatus = PopulateItemStatusById(client.ItemStatusId);
                client.ItemStatuses = BuildSelectList(client.ItemStatusId);
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
                client.Customization = PopulateCustomizationById(client.CustomizationId);
                client.ItemStatus = PopulateItemStatusById(client.ItemStatusId);
                client.ItemStatuses = BuildSelectList(client.ItemStatusId);
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
        #endregion

        #region Location
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
                location.Customization = PopulateCustomizationById(location.CustomizationId);
                location.ItemStatus = PopulateItemStatusById(location.ItemStatusId);
                location.ItemStatuses = BuildSelectList(location.ItemStatusId);
            }
            return location;
        }
        #endregion

        #region Area
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
                area.Customization = PopulateCustomizationById(area.CustomizationId);
                area.ItemStatus = PopulateItemStatusById(area.ItemStatusId);
                area.ItemStatuses = BuildSelectList(area.ItemStatusId);
            }
            return area;
        }

        public IEnumerable<Area> GetAreasByLocationId(int id)
        {
            var areas = _areaRepository.GetByLocationId(id).ToList();
            foreach (var area in areas.Where(x => x.AssetAreaId != null))
            {
                area.AssetArea = PopulateAssetStoreById(area.AssetAreaId);
            }
            return areas;
        }

        #endregion

        #region Tour
        public IEnumerable<Tour> GetToursByAreaId(int areaId)
        {
            var tours = _tourRepository.GetByAreaId(areaId).ToList();
            foreach(var tour in tours.Where(x=>x.AssetTourThumbnailId != null))
            {
                tour.AssetTourThumbnail = PopulateAssetStoreById(tour.AssetTourThumbnailId);
            }
            return tours;
        }

        public Tour GetTourByIdAndClientId(IEnumerable<string> clientIds, int id)
        {
            var tour = _tourRepository.GetByIdAndClientId(id, GetValidClientIds(clientIds));
            if(tour != null)
            {
                tour.AssetTourThumbnail = PopulateAssetStoreById(tour.AssetTourThumbnailId);
                tour.KrPanoTour = PopulateAssetStoreById(tour.KrPanoTourId);
                tour.Customization = PopulateCustomizationById(tour.CustomizationId);
                tour.ItemStatus = PopulateItemStatusById(tour.ItemStatusId);
                tour.ItemStatuses = BuildSelectList(tour.ItemStatusId);
            }
            return tour;
        }

        #endregion

        #region ItemStatus
        public IEnumerable<SelectListItem> BuildSelectList(int? currentId)
        {
            List<SelectListItem> itemStatusSelectList = new List<SelectListItem>();
            var itemStatuses = _itemStatusRepository.Get();
            foreach(var status in itemStatuses)
            {
                itemStatusSelectList.Add(new SelectListItem {
                    Text = status.Name,
                    Value = status.Id.ToString(),
                    Selected = status.Id == currentId
                });
            }
            return itemStatusSelectList;
        }

        #endregion

        #region Customization
        public Customization GetCustomizationById(int? id)
        {
            return PopulateCustomizationById(id);
        }

        public Customization PopulateCustomizationBasedOnEntityValues(int clientId, int locationId, int areaId, int tourId, int? customizationId)
        {
            Customization customization = PopulateCustomizationById(customizationId) ?? new Customization();
            if (tourId > 0)
            {
                customization.EntityName = _tourRepository.GetById(tourId)?.Name;
            }
            else if (areaId > 0)
            {
                customization.EntityName = _areaRepository.GetById(areaId)?.Name;
            }
            else if (locationId > 0)
            {
                customization.EntityName = _locationRepository.GetById(locationId)?.Name;
            }
            else if (clientId > 0)
            {
                customization.EntityName = _clientRepository.GetById(clientId)?.Name;
            }
            if(string.IsNullOrWhiteSpace(customization.EntityName))
            {
                customization.EntityName = "Unknown";
            }
            return customization;
        }

        #endregion

        #region Validity Helpers
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

        #endregion

        #region Private Helper Methods
        private ItemStatus PopulateItemStatusById(int? itemStatusId)
        {
            ItemStatus itemStatus = null;
            if (itemStatusId != null && itemStatusId > 0)
            {
                itemStatus = _itemStatusRepository.GetById(itemStatusId.Value);
            }
            return itemStatus;
        }

        private Customization PopulateCustomizationById(int? customizationId)
        {
            Customization customization = new Customization();
            if (customizationId != null && customizationId > 0)
            {
                customization = _customizationRepository.GetById(customizationId.Value);
            }
            return customization;
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
        #endregion
    }
}
