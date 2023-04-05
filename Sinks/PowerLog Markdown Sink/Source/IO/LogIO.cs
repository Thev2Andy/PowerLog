using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog.Sinks.Markdown
{
    #region LogIO Class XML
    /// <summary>
    /// Holds data related to log file paths.
    /// </summary>
    #endregion
    [Serializable] public class LogIO
    {
        #region Path String XML
        /// <summary>
        /// The log file location.
        /// </summary>
        #endregion
        public string Path { get; private set; }

        #region FileName String XML
        /// <summary>
        /// The log file name.
        /// </summary>
        #endregion
        public string FileName { get; private set; }

        #region FileExtension String XML
        /// <summary>
        /// The log file extension.
        /// </summary>
        #endregion
        public string FileExtension { get; private set; }


        #region LogIO Constructor XML
        /// <summary>
        /// The default <see cref="LogIO"/> constructor.
        /// </summary>
        /// <param name="Path">The path of the log.</param>
        /// <param name="FileName">The filename of the log.</param>
        /// <param name="FileExtension">The file extension of the log.</param>
        #endregion
        public LogIO(string Path, string FileName, string FileExtension) {
            this.FileExtension = FileExtension;
            this.FileName = FileName;
            this.Path = Path;
        }

        #region Get Method XML
        /// <summary>
        /// Gets the complete log path.
        /// </summary>
        /// <returns>The complete log path.</returns>
        #endregion
        public string Get() {
            return System.IO.Path.Combine(Path, $"{FileName}.{FileExtension}");
        }
    }
}
