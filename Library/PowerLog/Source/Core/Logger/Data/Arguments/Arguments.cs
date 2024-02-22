using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PowerLog
{
    #region Arguments Class XML
    /// <summary>
    /// The recorded log arguments.
    /// </summary>
    #endregion
    [Serializable] public class Arguments
    {
        #region ComposedLog String XML
        /// <summary>
        /// The composed log, using the saved log templates.
        /// </summary>
        #endregion
        public string ComposedLog {
            get {
                return this.Compose(Template, true);
            }
        }


        #region Content String XML
        /// <summary>
        /// The content of the log.
        /// </summary>
        #endregion
        public string Content;

        #region Severity Severity XML
        /// <summary>
        /// The severity of the log.
        /// </summary>
        #endregion
        public Severity Severity;

        #region Time DateTime XML
        /// <summary>
        /// The time of the log.
        /// </summary>
        #endregion
        public DateTime Time;

        #region Template Template XML
        /// <summary>
        /// The logging template used for logging and formatting.
        /// </summary>
        #endregion
        public Template Template;

        #region Stacktrace StackTrace XML
        /// <summary>
        /// The stacktrace of the log.
        /// </summary>
        #endregion
        public StackTrace Stacktrace;

        #region Sender Object XML
        /// <summary>
        /// The sender of the log.
        /// </summary>
        #endregion
        public Object Sender;

        #region Parameters Dictionary XML
        /// <summary>
        /// Additional logging data.
        /// </summary>
        #endregion
        public Dictionary<string, Object> Parameters;


        #region Logger Log XML
        /// <summary>
        /// The logger that sent this log.
        /// </summary>
        #endregion
        public Log Logger;
    }
}
