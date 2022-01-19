using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    /// <summary>
    /// Log Event Arguments holder.
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        public string LogMessage;
        public LogType MessageType;
        public DateTime LogTime;
        public bool Timestamped;
        public object LogSender;
    }
}
