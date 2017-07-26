namespace VirtualTourCore.Core.Models.ModelMaps
{
    public class CustomizationMap : BaseEntityMap<Customization>
    {
        public CustomizationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Table & Column Mappings
            this.ToTable("Customization");
            this.Property(t => t.CustomCSS).HasColumnName("CustomCSS");
            this.Property(t => t.CustomJS).HasColumnName("CustomJS");
        }
    }
}
