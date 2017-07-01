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
        int? CreateLocation(Location location, HttpPostedFileBase locationImage);
        int? UpdateLocation(Location location, HttpPostedFileBase locationImage);
        bool DeleteClient(Client client);
        bool DeleteLocation(Location location);
        void CreateArea(Area area, HttpPostedFileBase areaMap);
        void UpdateArea(Area area, HttpPostedFileBase areaMap);
        void DeleteArea(Area area);
        void CreateTour(Tour tour, HttpPostedFileBase tourThumb);
        void UpdateTour(Tour tour, HttpPostedFileBase tourThumb);
        void DeleteTour(Tour tour);
    }
}
