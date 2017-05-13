using NLog;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace VirtualTourCore.Common.Logging
{
    public class Nlogger : INlogger
    {
        public void Error(string message = "", Exception exception = null, [CallerFilePath] string filePath = "", [CallerMemberNameAttribute] string membername = "", [CallerLineNumberAttribute] int lineNumber = 0)
        {
            CreateLog(false, 3, message, filePath, lineNumber, membername, exception);
        }

        public void Warn(string message = "", Exception exception = null, [CallerFilePath] string filePath = "", [CallerMemberNameAttribute] string membername = "", [CallerLineNumberAttribute] int lineNumber = 0)
        {
            CreateLog(false, 2, message, filePath, lineNumber, membername, exception);
        }

        public void Info(string message = "", [CallerFilePath] string filePath = "", [CallerMemberNameAttribute] string membername = "", [CallerLineNumberAttribute] int lineNumber = 0)
        {
            CreateLog(false, 1, message, filePath, lineNumber, membername);
        }

        public void CreateLog(bool isFrontend, int logLevelSet = 0, string message = "", string filePath = "", int lineNumber = 0, string membername = "", Exception exception = null)
        {
            NLog.Logger logger = NLog.LogManager.GetLogger("Logger");
            string loggerName = Path.GetFileName(filePath) + "-" + membername + "-" + lineNumber.ToString();
            LogEventInfo eventInfo = new LogEventInfo()
            {
                LoggerName = loggerName,
                Message = message,
                Exception = exception
            };
            eventInfo.Properties["LayerLevel"] = (isFrontend == false) ? ("BACKEND") : ("FRONTEND");
            eventInfo.Properties["Machine"] = Environment.MachineName;
            switch (logLevelSet)
            {
                case 3:
                    eventInfo.Level = LogLevel.Error;
                    break;
                case 2:
                    eventInfo.Level = LogLevel.Warn;
                    break;
                case 1:
                    eventInfo.Level = LogLevel.Info;
                    break;
            }
            logger.Log(eventInfo);
        }
    }
}
