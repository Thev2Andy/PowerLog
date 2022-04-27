using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    #region IOSwapMode Enum XML
    /// <summary>
    /// IO swap mode enumeration.
    /// </summary>
    #endregion
    [Serializable] [Flags] public enum IOSwapMode
    {
        #region None EnumEntry XML
        /// <summary>
        /// Do nothing, only swap the LogIO.
        /// </summary>
        #endregion
        None = 0,

        #region Override EnumEntry XML
        /// <summary>
        /// Overwrite the file at the new path, if it exists.
        /// </summary>
        #endregion
        Override = 1,

        #region Migrate EnumEntry XML
        /// <summary>
        /// Migrate the contents of the old log to the new log file.
        /// </summary>
        #endregion
        Migrate = 2,

        #region Keep EnumEntry XML
        /// <summary>
        /// Keep the old log file?
        /// </summary>
        #endregion
        Keep = 4
    }
}
