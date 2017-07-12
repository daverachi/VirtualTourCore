using VirtualTourCore.Common.ValueObjects;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Mapping
{
    public static class LocationMapper
    {
        public static LocationDTO ToDataTransferObject(Location location)
        {
            LocationDTO locationDTO = new LocationDTO {
                Id = location.Id,
                Name = location.Name,
                DescriptionHtml = location.DescriptionHtml,
                StreetAddress = location.StreetAddress,
                City = location.City,
                State = location.State,
                Zipcode = location.Zipcode,
                ImagePath = location.AssetLocation != null ? location.AssetLocation.FullPath() : string.Empty
            };
            return locationDTO;
        }
    }
}
