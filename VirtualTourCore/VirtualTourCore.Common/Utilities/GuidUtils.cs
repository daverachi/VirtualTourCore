using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualTourCore.Common.Utilities
{
    public static class GuidUtils
    {
        public static string CreateShortGuid(int length)
        {
            if (length > 32) { length = 32; }// Maximum length of Guid
            return Guid.NewGuid().ToString("N").Substring(0, length);
        }
        public static string AppendGuidToFilename(string filename)
        {
            string[] parsedFilename = filename.Split('.');
            var extension = parsedFilename.Last();
            var name = parsedFilename.Where(x => x != extension)
                .Aggregate(new StringBuilder(), (sb, x) => sb.Append(x), sb => sb.ToString());
            return string.Format("{0}-{1}.{2}", name, CreateShortGuid(6), extension);
        }
    }
}
