using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VirtualTourCore.Common.Logging
{
    public interface INlogger
    {
        void Error(string message = "", Exception exception = null, [CallerFilePath] string filePath = "", [CallerMemberNameAttribute] string membername = "", [CallerLineNumberAttribute] int lineNumber = 0);
        void Warn(string message = "", Exception exception = null, [CallerFilePath] string filePath = "", [CallerMemberNameAttribute] string membername = "", [CallerLineNumberAttribute] int lineNumber = 0);
        void Info(string message = "", [CallerFilePath] string filePath = "", [CallerMemberNameAttribute] string membername = "", [CallerLineNumberAttribute] int lineNumber = 0);
        void CreateLog(bool isFrontend, int logLevelSet = 0, string message = "", string filePath = "", int lineNumber = 0, string membername = "", Exception exception = null);
    }
}
