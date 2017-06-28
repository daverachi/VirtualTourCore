using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.DescriptionHtml).HasColumnName("DescriptionHtml");
            this.Property(t => t.DescriptionJson).HasColumnName("DescriptionJson");
        }
    }
}
