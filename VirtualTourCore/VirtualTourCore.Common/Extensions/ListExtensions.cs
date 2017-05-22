using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualTourCore.Common.Extensions
{
    public static class ListExtensions
    {
        public static string ConvertIntListToCSV(this List<int> items)
        {
            return string.Join(", ", items.Select(x => x.ToString()).ToArray());
        }
        
    }
}
