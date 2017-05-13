using System.Collections.Generic;

namespace VirtualTourCore.Common.DataAccess.Interfaces
{
    public interface ILookupRepositoryBase<TLookupEntity, TKnownValues>
    {
        TLookupEntity GetByKey(int key);
        TLookupEntity GetByCode(string code);
        TLookupEntity GetKnownValue(TKnownValues knownValue);
        IEnumerable<TLookupEntity> GetAll();
    }
    public interface ILookupRepositoryBase<TLookupEntity>
    {
        IEnumerable<TLookupEntity> GetAll();
    }
}
