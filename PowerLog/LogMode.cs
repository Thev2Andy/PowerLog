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
    [Flags] public enum LogMode
    {
        Timestamp = 1,
        Save = 2,
        InvokeEvent = 4,
        NoSizeCheck = 8,
        Default = (Timestamp | Save | InvokeEvent)
    }
}
