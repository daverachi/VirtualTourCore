using System.Collections.Generic;

namespace VirtualTourCore.Common.ValueObjects
{
    public class ClientDTO
    {
        public ClientDTO()
        {
            Locations = new List<LocationDTO>();
        }
        public string Name { get; set; }
        public string Link { get; set; }
        public string DescriptionHtml { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Phone { get; set; }
        public string SupportEmail { get; set; }
        public string MarketingEmail { get; set; }
        public string LogoPath { get; set; }
        public string ProfilePath { get; set; }
        public List<LocationDTO> Locations { get; set; }
    }
}