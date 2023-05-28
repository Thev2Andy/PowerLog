using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    #region Format Class XML
    /// <summary>
    /// The class used for formatting logs.
    /// </summary>
    #endregion
    public static class Format
    {
        #region Formulate Function XML
        /// <summary>
        /// Formulates the log using the template by using regular expressions, then returns it as a string.
        /// </summary>
        /// <returns>The formulated log string.</returns>
        /// <param name="Log">The log to formulate.</param>
        /// <param name="Template">The log template.</param>
        #endregion
        public static string Formulate(this Arguments Log, Template Template)
        {
            Dictionary<char, string> Replacements = new Dictionary<char, string>
            {
                { 'T', Log.Time.ToString(Template.DateFormat) },
                { 'I', Log.Logger?.Identifier ?? "N/A"  },
                { 'S', Log.Severity.ToString() },
                { 'C', Log.Content },
                { 'O', Log.Sender?.ToString() ?? "N/A" },
            };

            
            string Result = Template.LogFormat;
            foreach (KeyValuePair<char, string> Replacement in Replacements)
            {
                Regex WildcardRegex = new Regex($@"\|([^|]*)([{Replacement.Key}])([^|]*)\|", RegexOptions.Multiline);
                Result = WildcardRegex.Replace(Result, new MatchEvaluator((Match Match) => {
                    bool IsSkippingSeverityHeader = (Replacement.Key == 'S' && Log.Severity == Severity.Generic);
                    return ((!IsSkippingSeverityHeader) ? ($"{Match.Groups[1].Value}" + $"{Replacement.Value}" + $"{Match.Groups[3].Value}") : String.Empty);
                }));
            }

            return Result;
        }

        #region Parse Function XML
        /// <summary>
        /// Parses a raw log argument object's parameters, then returns it.
        /// </summary>
        /// <returns>The processed log.</returns>
        /// <param name="Log">The log to parse the parameters of.</param>
        #endregion
        public static Arguments Parse(this Arguments Log)
        {
            if (Log.Parameters != null) {
                Arguments PreprocessedLog = new Arguments() {
                    Content = Log.Content,
                    Severity = Log.Severity,
                    Time = Log.Time,
                    Template = Log.Template,
                    Sender = Log.Sender,
                    Stacktrace = Log.Stacktrace,
                    Parameters = Log.Parameters,

                    Logger = Log.Logger
                };


                for (int I = 0; I < PreprocessedLog.Parameters.Count; I++) {
                    PreprocessedLog.Content = PreprocessedLog.Content.Replace($"~{PreprocessedLog.Parameters[I].Identifier}~", PreprocessedLog.Parameters[I].Value.ToString());
                }

                return PreprocessedLog;
            }

            else {
                return Log;
            }
        }
    }
}
