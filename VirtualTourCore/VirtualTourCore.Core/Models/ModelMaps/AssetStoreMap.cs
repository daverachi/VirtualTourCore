using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualTourCore.Core.Models.ModelMaps
{
    public class AssetStoreMap : BaseEntityMap<AssetStore>
    {
        public AssetStoreMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("AssetStore");
            this.Property(t => t.ClientId).HasColumnName("ClientID");
            this.Property(t => t.Path).HasColumnName("Path");
            this.Property(t => t.Filename).HasColumnName("Filename");
            this.Property(t => t.Nickname).HasColumnName("Nickname");
            this.Property(t => t.FileType).HasColumnName("FileType");
        }
    }
}
