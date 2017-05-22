using System;

namespace VirtualTourCore.Common.DataAccess
{
    public abstract class EntityBase : VTCoreEntity
    {
        public int? CreateUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdateUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
    }
}
