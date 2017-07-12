using VirtualTourCore.Common.ValueObjects;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Mapping
{
    public static class ClientMapper
    {
        public static ClientDTO ToDataTransferObject(Client client)
        {
            ClientDTO clientDTO = new ClientDTO
            {
                Name = client.Name,
                Link = client.Link,
                DescriptionHtml = client.DescriptionHtml,
                StreetAddress = client.StreetAddress,
                City = client.City,
                State = client.State,
                Zipcode = client.Zipcode,
                Phone = client.Phone,
                SupportEmail = client.SupportEmail,
                MarketingEmail = client.MarketingEmail,
                LogoPath = client.AssetLogo != null ? client.AssetLogo.FullPath() : string.Empty,
                ProfilePath = client.AssetProfile != null ? client.AssetProfile.FullPath() : string.Empty,
            };
            return clientDTO;
        }
    }
}
