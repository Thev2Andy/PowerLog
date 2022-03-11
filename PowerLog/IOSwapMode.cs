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
    [Flags]
    public enum IOSwapMode
    {
        None = 0,
        Override = 1,
        Migrate = 2,
        KeepOldFile = 4
    }
}
