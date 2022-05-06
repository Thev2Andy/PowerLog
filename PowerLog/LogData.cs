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
        // Public / Accessible variables..
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
        public LogOptions LogOptions { get; private set; }

        
        // Predefined profiles..
        #region Default LogData XML
        /// <summary>
        /// The default <c>LogData</c> profile, with it's default templates.
        /// </summary>
        #endregion
        public static LogData Default {
            get {
                return new LogData(LogOptions.Default, new LogTemplate("|[T] |||N |L: ||C|| (S)|", "HH:mm:ss"));
            }
        }

        #region EmptyLine LogData XML
        /// <summary>
        /// An empty <c>LogData</c> profile, specifically for empty lines.
        /// </summary>
        #endregion
        public static LogData EmptyLine {
            get {
                return new LogData(LogOptions.Default, new LogTemplate("", ""));
            }
        }



        #region LogData Constructor XML
        /// <summary>
        /// The default <c>LogData</c> constructor.
        /// </summary>
        /// <param name="LogOptions">The logging options to use.</param>
        /// <param name="LogTemplate">The log formats.</param>
        #endregion
        public LogData(LogOptions LogOptions, LogTemplate LogTemplate) {
            this.LogTemplate = LogTemplate;
            this.LogOptions = LogOptions;
        }
    }
}
