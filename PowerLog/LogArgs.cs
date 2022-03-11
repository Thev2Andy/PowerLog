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
    public class LogArgs : EventArgs
    {
        public string LogMessage;
        public LogType LogLevel;
        public DateTime LogTime;
        public LogMode LoggingMode;
        public object LogSender;
    }
}
