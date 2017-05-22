using VirtualTourCore.Common.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using VirtualTourCore.Common.Logging;

namespace VirtualTourCore.Common.DataAccess
{
    public class LookupRepositoryBase<TLookupEntity, TKnownValuesEnum> : BaseRepository<TLookupEntity>, ILookupRepositoryBase<TLookupEntity, TKnownValuesEnum>
       where TLookupEntity : LookupBase
    {
        public LookupRepositoryBase(IUnitOfWork UnitOfWork, INlogger log)
            : base(UnitOfWork, log)
        {
        }

        public TLookupEntity GetByCode(string code)
        {
            return DbSet.SingleOrDefault(e => e.Code == code);
        }

        public TLookupEntity GetByKey(int Key)
        {
            return DbSet.SingleOrDefault(e => e.Key == Key);
        }

        public TLookupEntity GetKnownValue(TKnownValuesEnum knownValue)
        {
            return GetByCode(knownValue.ToString());
        }

        public IEnumerable<TLookupEntity> GetAll()
        {
            return DbSet.OrderBy(e => e.DisplayOrder);
        }
    }
    public class LookupRepositoryBase<TLookupEntity> : BaseRepository<TLookupEntity>, ILookupRepositoryBase<TLookupEntity>
            where TLookupEntity : EntityBase
    {
        protected DbSet<TLookupEntity> _set;

        public LookupRepositoryBase(IUnitOfWork uow, INlogger log)
            : base(uow, log)
        {
            _set = DbSet;
        }

        public IEnumerable<TLookupEntity> GetAll()
        {
            return _set;
        }
    }
}
