using System;
using System.Collections.Generic;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Services
{
    public class LookupService : ILookupService
    {
        private IClientRepository _clientRepository;
        public LookupService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
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
    }
}
