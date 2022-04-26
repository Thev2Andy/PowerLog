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
        #region FormattedLog String XML
        /// <summary>
        /// The formatted log, using the saved log templates.
        /// </summary>
        #endregion
        public string FormattedLog {
            get {
                return LogFormat.Format(this, LoggingData.LogTemplate.LogFormat, LoggingData.LogTemplate.DateFormat);
            }
        }


        #region LogContent String XML
        /// <summary>
        /// The content of the log.
        /// </summary>
        #endregion
        public string LogContent;

        #region LogLevel LogType XML
        /// <summary>
        /// The level of the log.
        /// </summary>
        #endregion
        public LogType LogLevel;

        #region LogTime DateTime XML
        /// <summary>
        /// The time of the log.
        /// </summary>
        #endregion
        public DateTime LogTime;

        #region LoggingData LogData XML
        /// <summary>
        /// The logging data used for logging and formatting.
        /// </summary>
        #endregion
        public LogData LoggingData;

        #region LogSender Object XML
        /// <summary>
        /// The sender of the log.
        /// </summary>
        #endregion
        public object LogSender;

        #region Logger Log XML
        /// <summary>
        /// The logger that sent this log.
        /// </summary>
        #endregion
        public Log Logger;
    }
}
