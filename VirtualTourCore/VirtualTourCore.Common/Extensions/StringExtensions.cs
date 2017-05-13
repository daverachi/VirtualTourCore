using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualTourCore.Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// This shortens a string to the length of the int given
        /// </summary>
        /// <param name="_string"></param>
        /// <param name="charCount"></param>
        /// <returns></returns>
        public static string Trim(this string _string, int charCount)
        {
            if (charCount < _string.Length)
            {
                return _string.Substring(0, charCount) + "...";
            }
            return _string;
        }
    }
}
