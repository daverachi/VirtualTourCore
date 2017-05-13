namespace VirtualTourCore.Common.DataAccess
{
    public abstract class LookupBase : EntityBase
    {
        public int Key { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
    }
}
