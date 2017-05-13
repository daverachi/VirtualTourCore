using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using VirtualTourCore.Common.DataAccess;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Models.ModelMaps;

namespace VirtualTourCore.Core.Models
{
    public class VirtualTourCoreContext : BaseDbContext, IVirtualTourCoreContext
    {
        static VirtualTourCoreContext()
        {
            Database.SetInitializer<VirtualTourCoreContext>(null);
        }

        public VirtualTourCoreContext()
            : base("Name=VirtualTourContext")
        {
        }

        public DbSet<RegistrationCode> RegistrationCodes { get; set; }
        public DbSet<SecurityUser> SecurityUsers { get; set; }
        public DbSet<SecurityUserClient> SecurityUserClients { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Location> Locations { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Prevent cascading delete
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new RegistrationCodeMap());
            modelBuilder.Configurations.Add(new SecurityUserMap());
            modelBuilder.Configurations.Add(new SecurityUserClientsMap());
            modelBuilder.Configurations.Add(new ClientMap());
            modelBuilder.Configurations.Add(new LocationMap());
        }

    }
}
