using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualTourCore.Common.Helper
{
    public class SearchResults<T>
    {
        public IEnumerable<T> Results { get; set; }
        public int RowsAffected { get; set; }
    }
}
