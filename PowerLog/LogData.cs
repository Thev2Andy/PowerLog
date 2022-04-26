using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    #region LogData Class XML
    /// <summary>
    /// Contains the log template and logging mode.
    /// </summary>
    #endregion
    [Serializable] public class LogData
    {
        #region LogTemplate LogTemplate XML
        /// <summary>
        /// The format of the log and date.
        /// </summary>
        #endregion
        public LogTemplate LogTemplate { get; private set; }

        #region LogMode LogMode XML
        /// <summary>
        /// The mode of the log.
        /// </summary>
        #endregion
        public LogMode LogMode { get; private set; }

        // Missing XML.
        public static LogData Default {
            get {
                return new LogData(LogMode.Default, new LogTemplate("|[T] ||N ||L: ||C|| (S)|", "HH:mm:ss"));
            }
        }
        
        // Missing XML.
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
