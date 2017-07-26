using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Interfaces
{
    public interface ILookupService
    {
        IEnumerable<Client> GetClientByGuids(IEnumerable<Guid> guids);
        Client GetClientByGuid(Guid guid);
        Client GetClientById(int id);
        IEnumerable<Client> GetClients();
        IEnumerable<Location> GetLocationsByClientId(int clientId);
        Location GetLocationByIdAndClientId(IEnumerable<string> clientIds, int id);
        IEnumerable<Area> GetAreasByClientId(int id);
        Area GetAreaByIdAndClientId(IEnumerable<string> clientIds, int id);
        IEnumerable<Area> GetAreasByLocationId(int id);
        IEnumerable<Tour> GetToursByAreaId(int areaId);
        Tour GetTourByIdAndClientId(IEnumerable<string> clientIds, int id);
        bool ValidRegistrationCode(string code);
        bool ValidEmail(string code);
        bool ValidUsername(string code);
        IEnumerable<SelectListItem> BuildSelectList(int? currentId);
        Customization PopulateCustomizationBasedOnEntityValues(int clientId, int locationId, int areaId, int tourId, int? customizationId);
    }
}
