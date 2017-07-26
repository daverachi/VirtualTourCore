namespace VirtualTourCore.Core.Models.ModelMaps
{
    public class AreaMap : BaseEntityMap<Area>
    {
        public AreaMap()
        {
            this.HasKey(t => t.Id);
            // Table & Column Mappings
            this.ToTable("Area");
            this.Property(t => t.ClientId).HasColumnName("ClientId");
            this.Property(t => t.LocationId).HasColumnName("locationId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.DescriptionHtml).HasColumnName("DescriptionHtml");
            this.Property(t => t.DescriptionJson).HasColumnName("DescriptionJson");
            this.Property(t => t.AssetAreaId).HasColumnName("AssetAreaId");

        }
    }
}
