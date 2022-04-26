using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    #region LogTemplate Class XML
    /// <summary>
    /// Holds data related to the formatting of the logs.
    /// </summary>
    #endregion
    [Serializable] public class LogTemplate
    {
        #region DateFormat String XML
        /// <summary>
        /// The date formatting template.
        /// </summary>
        #endregion
        public string DateFormat { get; private set; }

        #region LogFormat String XML
        /// <summary>
        /// The log formatting template.
        /// Examples: <br/> <br/>
        /// Use `<c>|T|</c>` for the log timestamp. <br/>
        /// Use `<c>|N|</c>` for the logger name. <br/>
        /// Use `<c>|L|</c>` for the log level. <br/>
        /// Use `<c>|C|</c>` for the log content. <br/>
        /// Use `<c>|S|</c>` for the log sender. <br/>
        /// </summary>
        #endregion
        public string LogFormat { get; private set; }


        public LogTemplate(string LogFormat, string DateFormat = "HH:mm:ss") {
            this.DateFormat = DateFormat;
            this.LogFormat = LogFormat;
        }
    }
}
