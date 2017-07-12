namespace VirtualTourCore.Common.ValueObjects
{
    public class LocationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DescriptionHtml { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string ImagePath { get; set; }
    }
}