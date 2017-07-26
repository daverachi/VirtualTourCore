using System.Data.Entity.ModelConfiguration;

namespace VirtualTourCore.Core.Models.ModelMaps
{
    public class RegistrationCodeMap : EntityTypeConfiguration<RegistrationCode>
    {
        public RegistrationCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("RegistrationCode");
            this.Property(t => t.Guid).HasColumnName("GUID");
            this.Property(t => t.AvailableUsages).HasColumnName("AvailableUsages");
            this.Property(t => t.ClientId).HasColumnName("ClientId");
        }
    }
}
