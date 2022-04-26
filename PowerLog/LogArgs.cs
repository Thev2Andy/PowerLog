using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    #region LogArgs Class XML
    /// <summary>
    /// Log arguments holder.
    /// </summary>
    #endregion
    [Serializable] public class LogArgs : EventArgs
    {
        public string FormattedLog {
            get {
                return LogFormat.Format(this, LoggingData.LogTemplate.LogFormat, LoggingData.LogTemplate.DateFormat);
            }
        }

        public string LogContent;
        public LogType LogLevel;
        public DateTime LogTime;
        public LogData LoggingData;
        public object LogSender;

        public Log Logger;
    }
}
