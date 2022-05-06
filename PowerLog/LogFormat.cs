using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    #region LogFormat Class XML
    /// <summary>
    /// The class used for formatting logs.
    /// </summary>
    #endregion
    [Serializable] public static class LogFormat
    {
        #region Postproces Function XML
        /// <summary>
        /// Formats a log string using Regex, then returns it.
        /// </summary>
        /// <returns>The formatted log string.</returns>
        /// <param name="Log">The log to postprocess.</param>
        /// <param name="LogTemplate">The log template.</param>
        /// <param name="DateTemplate">The log date template.</param>
        #endregion
        public static string Postprocess(LogArgs Log, string LogTemplate, string DateTemplate)
        {
            Regex TimeRegex = new Regex(@"\|([^|]*)T([^|]*)\|", RegexOptions.Multiline);
            string Result = TimeRegex.Replace(LogTemplate, ("${1}" + $"{Log.LogTime.ToString(DateTemplate)}" + "${2}"));


            Regex LogRegex = new Regex(@"\|([^|]*)N([^|]*)\|", RegexOptions.Multiline);
            Result = LogRegex.Replace(Result, ((Log.Logger != null && !String.IsNullOrEmpty(Log.Logger.Identifier)) ? ("${1}" + $"{Log.Logger.Identifier}" + "${2}") : String.Empty));

            Regex LevelRegex = new Regex(@"\|([^|]*)L([^|]*)\|", RegexOptions.Multiline);
            Result = LevelRegex.Replace(Result, ((Log.LogLevel != LogType.NA) ? ("${1}" + $"{Log.LogLevel.ToString()}" + "${2}") : String.Empty));

            Regex ContentRegex = new Regex(@"\|([^|]*)C([^|]*)\|", RegexOptions.Multiline);
            Result = ContentRegex.Replace(Result, ("${1}" + $"{Log.LogContent}" + "${2}"));

            Regex SenderRegex = new Regex(@"\|([^|]*)S([^|]*)\|", RegexOptions.Multiline);
            Result = SenderRegex.Replace(Result, ("${1}" + $"{((Log.LogSender != null) ? Log.LogSender : "N/A")}" + "${2}"));

            return Result;
        }

        #region Postproces Function XML
        /// <summary>
        /// Preprocesses a raw log object, then returns it.
        /// </summary>
        /// <returns>The preprocessed log.</returns>
        /// <param name="Log">The log to preprocess.</param>
        #endregion
        public static LogArgs Preprocess(LogArgs Log)
        {
            if (Log.LogParameters != null) {
                LogArgs PreprocessedLog = Log;
                for (int i = 0; i < PreprocessedLog.LogParameters.Count; i++)
                {
                    PreprocessedLog.LogContent = Log.LogContent.Replace($"~{PreprocessedLog.LogParameters[i].Identifier}~", PreprocessedLog.LogParameters[i].Value.ToString());
                }

                return PreprocessedLog;
            }

            else {
                return Log;
            }
        }
    }
}
