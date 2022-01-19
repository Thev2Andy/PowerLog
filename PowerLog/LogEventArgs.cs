using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    public class LogEventArgs : EventArgs
    {
        public string LogMessage;
        public LogType OutputType;
        public DateTime LogTime;
        public bool Timestamped;
    }
}
