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
        string DeleteClient(Client client);
        string DeleteLocation(Location location);
        void CreateArea(Area area, HttpPostedFileBase areaMap);
        void UpdateArea(Area area, HttpPostedFileBase areaMap);
        string DeleteArea(Area area);
        void CreateTour(Tour tour, HttpPostedFileBase tourThumb, HttpPostedFileBase KrPanoZip);
        void UpdateTour(Tour tour, HttpPostedFileBase tourThumb, HttpPostedFileBase KrPanoZip);
        string DeleteTour(Tour tour);
        void UpdateTourLocations(List<Tour> tours, int userId);
        string IssueInvitation(int id, string email);
    }
}
