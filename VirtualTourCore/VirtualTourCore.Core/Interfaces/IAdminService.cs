using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Interfaces
{
    public interface IAdminService
    {
        int? CreateClient(Client client, HttpPostedFileBase logo, HttpPostedFileBase profile);
        int? UpdateClient(Client client, HttpPostedFileBase logo, HttpPostedFileBase profile);
        int? CreateLocation(Location location);
        int? UpdateLocation(Location location);
        bool DeleteClient(Client client);
        bool DeleteLocation(Location location);
        void CreateArea(Area area);
        void UpdateArea(Area area);
        void DeleteArea(Area area);
        void CreateTour(Tour tour);
        void UpdateTour(Tour tour);
        void DeleteTour(Tour tour);
    }
}
