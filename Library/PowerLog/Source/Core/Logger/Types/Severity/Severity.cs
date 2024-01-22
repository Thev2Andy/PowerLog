using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    #region Severity Enum XML
    /// <summary>
    /// Log severity enumeration.
    /// </summary>
    #endregion
    [Flags] public enum Severity
    {
        #region Verbose Enum Entry XML
        /// <summary>
        /// 'Verbose' log level.
        /// </summary>
        #endregion
        Verbose = 1,

        #region Trace Enum Entry XML
        /// <summary>
        /// 'Trace' log level.
        /// </summary>
        #endregion
        Trace = 2,

        #region Debug Enum Entry XML
        /// <summary>
        /// 'Debug' log level.
        /// </summary>
        #endregion
        Debug = 4,

        #region Network Enum Entry XML
        /// <summary>
        /// 'Network' log level.
        /// </summary>
        #endregion
        Network = 8,

        #region Information Enum Entry XML
        /// <summary>
        /// 'Information' log level.
        /// </summary>
        #endregion
        Information = 16,

        #region Notice Enum Entry XML
        /// <summary>
        /// 'Notice' log level.
        /// </summary>
        #endregion
        Notice = 32,

        #region Caution Enum Entry XML
        /// <summary>
        /// 'Caution' log level.
        /// </summary>
        #endregion
        Caution = 64,

        #region Warning Enum Entry XML
        /// <summary>
        /// 'Warning' log level.
        /// </summary>
        #endregion
        Warning = 128,

        #region Alert Enum Entry XML
        /// <summary>
        /// 'Alert' log level.
        /// </summary>
        #endregion
        Alert = 256,

        #region Error Enum Entry XML
        /// <summary>
        /// 'Error' log level.
        /// </summary>
        #endregion
        Error = 512,

        #region Critical Enum Entry XML
        /// <summary>
        /// 'Critical' log level.
        /// </summary>
        #endregion
        Critical = 1024,

        #region Emergency Enum Entry XML
        /// <summary>
        /// 'Emergency' log level.
        /// </summary>
        #endregion
        Emergency = 2048,

        #region Fatal Enum Entry XML
        /// <summary>
        /// 'Fatal' log level.
        /// </summary>
        #endregion
        Fatal = 4096,

        #region Generic Enum Entry XML
        /// <summary>
        /// 'Generic' log level, will not have a header when formatted either alone or together with other severity levels.
        /// </summary>
        #endregion
        Generic = 8192,
    }
}
