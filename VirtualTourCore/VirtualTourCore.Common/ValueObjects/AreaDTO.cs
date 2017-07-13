using System.Collections.Generic;

namespace VirtualTourCore.Common.ValueObjects
{
    public class AreaDTO
    {
        public AreaDTO()
        {
            Tours = new List<TourDTO>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string DescriptionHtml { get; set; }
        public string ImagePath { get; set; }
        public List<TourDTO> Tours { get; set; }
    }
}
