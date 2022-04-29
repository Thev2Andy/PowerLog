using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    #region LogMode Enum XML
    /// <summary>
    /// Log mode enumeration.
    /// </summary>
    #endregion
    [Serializable] [Flags] public enum LogMode
    {
        #region Save EnumEntry XML
        /// <summary>
        /// Save the log to the log file.
        /// </summary>
        #endregion
        Save = 0,

        #region InvokeEvent EnumEntry XML
        /// <summary>
        /// Invokes the log event, can be used for anonymous logs.
        /// </summary>
        #endregion
        InvokeEvent = 1,

        #region NoSizeCheck EnumEntry XML
        /// <summary>
        /// Prevents the log cache size check.
        /// </summary>
        #endregion
        NoSizeCheck = 2,

        #region Default EnumEntry XML
        /// <summary>
        /// The default log mode.
        /// </summary>
        #endregion
        Default = (Save | InvokeEvent),
    }
}
