using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    // Missing XML.
    [Serializable] public class LogIO
    {
        #region LogPath String XML
        /// <summary>
        /// The log file location.
        /// </summary>
        #endregion
        public string LogPath { get; private set; }

        #region LogFileName String XML
        /// <summary>
        /// The log file name.
        /// </summary>
        #endregion
        public string LogFileName { get; private set; }

        #region LogFileName String XML
        /// <summary>
        /// The log file extension.
        /// </summary>
        #endregion
        public string LogFileExtension { get; private set; }

        public LogIO(string LogPath, string LogFileName, string LogFileExtension) {
            this.LogPath = LogPath;
            this.LogFileName = LogFileName;
            this.LogFileExtension = LogFileExtension;
        }

        #region GetLogPath Method XML
        /// <summary>
        /// Get the complete log path.
        /// </summary>
        /// <returns>The complete log path.</returns>
        #endregion
        public string GetLogPath() {
            return Path.Combine(LogPath, $"{LogFileName}.{LogFileExtension}");
        }
    }
}
