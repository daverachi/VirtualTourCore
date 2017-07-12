using VirtualTourCore.Common.ValueObjects;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Mapping
{
    public static class AreaMapper
    {
        public static AreaDTO ToDataTransferObject(Area area)
        {
            AreaDTO areaDTO = new AreaDTO
            {
                Id = area.Id,
                Name = area.Name,
                DescriptionHtml = area.DescriptionHtml,
                ImagePath = area.AssetArea != null ? area.AssetArea.FullPath() : string.Empty
            };
            return areaDTO;
        }
    }
}
