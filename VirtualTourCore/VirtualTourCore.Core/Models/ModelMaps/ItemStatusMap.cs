using System.Data.Entity.ModelConfiguration;

namespace VirtualTourCore.Core.Models.ModelMaps
{
    public class ItemStatusMap : EntityTypeConfiguration<ItemStatus>
    {
        public ItemStatusMap()
        {
            this.HasKey(t => t.Id);
            // Table & Column Mappings
            this.ToTable("ItemStatus");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
