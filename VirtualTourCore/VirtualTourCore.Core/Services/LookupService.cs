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
        public LookupService(
            IClientRepository clientRepository,
            ILocationRepository locationRepository,
            IAreaRepository areaRepository,
            ITourRepository tourRepository)
        {
            _clientRepository = clientRepository;
            _locationRepository = locationRepository;
            _areaRepository = areaRepository;
            _tourRepository = tourRepository;
        }
        public Client GetClientByGuid(Guid guid)
        {
            return _clientRepository.GetByGuid(guid);
        }
        public Client GetClientById(int id)
        {
            return _clientRepository.GetById(id);
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

            return _locationRepository.GetByIdFilteredByClientIds(id, GetValidClientIds(clientIds));
        }

        public IEnumerable<Area> GetAreasByClientId(int id)
        {
            return _areaRepository.GetByClientId(id);
        }

        public Area GetAreaByIdAndClientId(IEnumerable<string> clientIds, int id)
        {
            return _areaRepository.GetByIdAndByClientId(id, GetValidClientIds(clientIds));
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
