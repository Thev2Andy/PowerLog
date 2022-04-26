using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    // Missing XML.
    [Serializable] public class LogData
    {
        public LogTemplate LogTemplate { get; private set; }
        public LogMode LogMode { get; private set; }

        public static LogData Default {
            get {
                return new LogData(LogMode.Default, new LogTemplate("|[T] ||N ||L: ||C|| (S)|", "HH:mm:ss"));
            }
        }

        public static LogData EmptyLine {
            get {
                return new LogData(LogMode.Default, new LogTemplate("", ""));
            }
        }


        public LogData(LogMode LogMode, LogTemplate LogTemplate) {
            this.LogTemplate = LogTemplate;
            this.LogMode = LogMode;
        }
    }
}
