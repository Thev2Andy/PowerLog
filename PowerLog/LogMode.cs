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
        Save = 0,
        InvokeEvent = 1,
        NoSizeCheck = 2,
        Default = (Save | InvokeEvent),
    }
}
