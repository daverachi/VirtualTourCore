using System.Data.Entity.ModelConfiguration;
using VirtualTourCore.Common.DataAccess;

namespace VirtualTourCore.Core.Models.ModelMaps
{
    public class BaseEntityMap<T> : EntityTypeConfiguration<T> where T : EntityBase
    {
        public BaseEntityMap()
        {            
            this.Property(t => t.CreateUserId).HasColumnName("CreateUserId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateUserId).HasColumnName("UpdateUserId");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
        }
    }
}
