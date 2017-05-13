using System.Data.Entity.ModelConfiguration;

namespace VirtualTourCore.Core.Models.ModelMaps
{
    public class ClientMap : BaseEntityMap<Client>
    {
        public ClientMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Table & Column Mappings
            this.ToTable("Client");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Guid).HasColumnName("Guid");
            this.Property(t => t.Link).HasColumnName("Link");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.DescriptionHtml).HasColumnName("DescriptionHtml");
            this.Property(t => t.DescriptionJson).HasColumnName("DescriptionJson");
            this.Property(t => t.Zipcode).HasColumnName("Zipcode");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.SupportEmail).HasColumnName("SupportEmail");
            this.Property(t => t.MarketingEmail).HasColumnName("MarketingEmail");
            this.Property(t => t.StreetAddress).HasColumnName("StreetAddress");
            //this.Property(t => t.ItemStatusID).HasColumnName("ItemStatusID");
            //this.Property(t => t.LayoutConfigurationID).HasColumnName("LayoutConfigurationID");
            this.Property(t => t.AssetLogoId).HasColumnName("AssetLogoId");
            this.Property(t => t.AssetProfileId).HasColumnName("AssetProfileId");
        }
    }
}
