namespace VirtualTourCore.Core.Models.ModelMaps
{
    public class SecurityUserClientsMap : BaseEntityMap<SecurityUserClient>
    {
        public SecurityUserClientsMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Table & Column Mappings
            this.ToTable("SecurityUserClients");
            this.Property(t => t.SecurityUserId).HasColumnName("SecurityUserId");
            this.Property(t => t.ClientId).HasColumnName("ClientId");
        }
    }
}
