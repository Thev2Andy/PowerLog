using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    #region LogIO Class XML
    /// <summary>
    /// Holds data related to log file paths.
    /// </summary>
    #endregion
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

        #region LogIO Constructor XML
        /// <summary>
        /// The default <c>LogIO</c> constructor.
        /// </summary>
        /// <param name="LogPath">The path of the log.</param>
        /// <param name="LogFileName">The filename of the log.</param>
        /// <param name="LogFileExtension">The file extension of the log.</param>
        #endregion
        public LogIO(string LogPath, string LogFileName, string LogFileExtension) {
            this.LogPath = LogPath;
            this.LogFileName = LogFileName;
            this.LogFileExtension = LogFileExtension;
        }

        #region Get Method XML
        /// <summary>
        /// Gets the complete log path.
        /// </summary>
        /// <returns>The complete log path.</returns>
        #endregion
        public string Get() {
            return Path.Combine(LogPath, $"{LogFileName}.{LogFileExtension}");
        }
    }
}
