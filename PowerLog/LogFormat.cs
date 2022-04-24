using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    // Missing XML
    public static class LogFormat
    {
        // Private / Internal variables.
        // Missing XML.
        public static string DateTemplate { get; internal set; }

        // Missing XML.
        public static string LogTemplate { get; internal set; }



        // Missing XML.
        public static void SetTemplates(string NewLogTemplate, string NewDateTemplate) {
            DateTemplate = NewDateTemplate;
            LogTemplate = NewLogTemplate;
        }

        // TODO: Rewrite the formatting using Regex.
        // Missing XML.
        public static string Format(string LogContent, LogType LogLevel, bool Timestamped, object LogSender = null) {
            if (!string.IsNullOrEmpty(LogFormat.LogTemplate) && !string.IsNullOrEmpty(LogFormat.DateTemplate)) {
                // Regex Regex = new Regex("", RegexOptions.IgnoreCase, Regex.InfiniteMatchTimeout);

                return $"{((Timestamped) ? $"[{DateTime.Now.ToString(DateTemplate)}] " : String.Empty)}" +
                $"{((LogLevel != LogType.Null) ? $"{LogLevel.ToString()}: " : String.Empty)}" +
                $"{LogContent}";
            }else {
                throw new InvalidOperationException("Log formats are empty.");
            }
        }

    }
}
