using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PowerLog
{
    #region Arguments Struct XML
    /// <summary>
    /// The recorded log arguments.
    /// </summary>
    #endregion
    [Serializable] public record struct Arguments
    {
        #region ComposedLog String XML
        /// <summary>
        /// The composed log, composed using the saved log templates.
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
        public string Content { get; init; }

        #region Severity Severity XML
        /// <summary>
        /// The severity of the log.
        /// </summary>
        #endregion
        public Severity Severity { get; init; }

        #region Time DateTime XML
        /// <summary>
        /// The time of the log.
        /// </summary>
        #endregion
        public DateTime Time { get; init; }

        #region Template Template XML
        /// <summary>
        /// The saved logging template used for logging and formatting.
        /// </summary>
        #endregion
        public Template Template { get; init; }

        #region Sender Object XML
        /// <summary>
        /// The sender of the log.
        /// </summary>
        #endregion
        public Object Sender { get; init; }

        #region Parameters Dictionary XML
        /// <summary>
        /// Additional logging data.<br/>(Warning: The <see cref="Parameters"/> property is lazily initialized, so you should <see langword="null"/>-check it before using it.)
        /// </summary>
        #endregion
        public Dictionary<string, Object> Parameters { get; init; }

        #region Enrichments Dictionary XML
        /// <summary>
        /// Represents custom properties attached to logs by logger enrichers.<br/>(Warning: The <see cref="Enrichments"/> property is lazily initialized, so you should <see langword="null"/>-check it before using it.)
        /// </summary>
        #endregion
        public Dictionary<string, Object> Enrichments { get; init; }

        #region Context Dictionary XML
        /// <summary>
        /// Represents contextual properties attached to the logger and forwarded to emitted logs.<br/>(Warning: The <see cref="Context"/> property is lazily initialized, so you should <see langword="null"/>-check it before using it.)
        /// </summary>
        #endregion
        public Dictionary<string, Object> Context { get; init; }


        #region Logger Log XML
        /// <summary>
        /// The logger that sent this log.
        /// </summary>
        #endregion
        public Log Logger { get; init; }
    }
}
