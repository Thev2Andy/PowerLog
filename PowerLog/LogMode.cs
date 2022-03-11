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
    [Flags]
    public enum LogMode
    {
        Timestamp = 0,
        Save = 1,
        InvokeEvent = 2,
        NoSizeCheck = 3,
        Default = (Timestamp | Save | InvokeEvent)
    }
}
