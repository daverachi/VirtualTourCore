namespace VirtualTourCore.Core.Models.ModelMaps
{
    public class TourMap : BaseEntityMap<Tour>
    {
        public TourMap()
        {
            this.HasKey(t => t.Id);
            // Table & Column Mappings
            this.ToTable("Tour");
            this.Property(t => t.ClientId).HasColumnName("ClientId");
            this.Property(t => t.AreaId).HasColumnName("AreaId");
            this.Property(t => t.MapX).HasColumnName("MapX");
            this.Property(t => t.MapY).HasColumnName("MapY");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.DescriptionHtml).HasColumnName("DescriptionHtml");
            this.Property(t => t.DescriptionJson).HasColumnName("DescriptionJson");
            this.Property(t => t.AssetTourThumbnailId).HasColumnName("AssetTourThumbnailId");
            this.Property(t => t.CustomizationId).HasColumnName("CustomizationId");
            this.Property(t => t.ItemStatusId).HasColumnName("ItemStatusId");
        }
    }
}
