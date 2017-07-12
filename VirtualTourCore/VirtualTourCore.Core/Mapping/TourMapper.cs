using VirtualTourCore.Common.ValueObjects;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Mapping
{
    public static class TourMapper
    {
        public static TourDTO ToDataTransferObject(Tour tour)
        {
            TourDTO tourDTO = new TourDTO
            {
                Id = tour.Id,
                MapX = tour.MapX,
                MapY = tour.MapY,
                Name = tour.Name,
                DescriptionHtml = tour.DescriptionHtml,
                ImagePath = tour.AssetTourThumbnail != null ? tour.AssetTourThumbnail.FullPath() : string.Empty,
                KrPanoPath = tour.KrPanoTour != null ? tour.KrPanoTour.FullPath() : string.Empty
            };
            return tourDTO;
        }
    }
}
