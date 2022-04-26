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
        Trace,
        Debug,
        Info,
        Warning,
        Error,
        Network,
        Fatal,
        NA,
    }
}
