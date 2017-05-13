using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualTourCore.Core.Models.ModelMaps
{
    public class LocationMap : BaseEntityMap<Location>
    {
        public LocationMap()
        {
            this.HasKey(t => t.Id);
            // Table & Column Mappings
            this.ToTable("Location");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ClientId).HasColumnName("ClientId");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.DescriptionHtml).HasColumnName("DescriptionHtml");
            this.Property(t => t.DescriptionJson).HasColumnName("DescriptionJson");
            this.Property(t => t.StreetAddress).HasColumnName("StreetAddress");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.Zipcode).HasColumnName("Zipcode");
        }
    }
}
