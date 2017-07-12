namespace VirtualTourCore.Common.ValueObjects
{
    public class TourDTO
    {
        public int Id { get; set; }
        public double? MapX { get; set; }
        public double? MapY { get; set; }
        public string Name { get; set; }
        public string DescriptionHtml { get; set; }
        public string KrPanoPath { get; set; }
        public string ImagePath { get; set; }
    }
}
