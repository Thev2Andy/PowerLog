using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    #region LogType Enum XML
    /// <summary>
    /// Log type enumeration.
    /// </summary>
    #endregion
    public enum LogType
    {
        #region Trace EnumEntry XML
        /// <summary>
        /// 'Trace' log level.
        /// </summary>
        #endregion
        Trace,

        #region Debug EnumEntry XML
        /// <summary>
        /// 'Debug' log level.
        /// </summary>
        #endregion
        Debug,

        #region Info EnumEntry XML
        /// <summary>
        /// 'Info' log level.
        /// </summary>
        #endregion
        Info,

        #region Warning EnumEntry XML
        /// <summary>
        /// 'Warning' log level.
        /// </summary>
        #endregion
        Warning,

        #region Error EnumEntry XML
        /// <summary>
        /// 'Error' log level.
        /// </summary>
        #endregion
        Error,

        #region Network EnumEntry XML
        /// <summary>
        /// 'Network' log level.
        /// </summary>
        #endregion
        Network,

        #region Fatal EnumEntry XML
        /// <summary>
        /// 'Fatal' log level.
        /// </summary>
        #endregion
        Fatal,

        #region NA EnumEntry XML
        /// <summary>
        /// 'NA' log level, will not have a header when formatted.
        /// </summary>
        #endregion
        NA,
    }
}
