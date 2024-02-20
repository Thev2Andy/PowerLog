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
        // Wildcard Regular Expressions..
        private static readonly Regex TimeWildcardRegex = new Regex(@"\|([^|]*)([T])([^|]*)\|", (RegexOptions.Multiline | RegexOptions.Compiled));
        private static readonly Regex IdentifierWildcardRegex = new Regex(@"\|([^|]*)([I])([^|]*)\|", (RegexOptions.Multiline | RegexOptions.Compiled));
        private static readonly Regex SeverityWildcardRegex = new Regex(@"\|([^|]*)([S])([^|]*)\|", (RegexOptions.Multiline | RegexOptions.Compiled));
        private static readonly Regex ContentWildcardRegex = new Regex(@"\|([^|]*)([C])([^|]*)\|", (RegexOptions.Multiline | RegexOptions.Compiled));
        private static readonly Regex ObjectWildcardRegex = new Regex(@"\|([^|]*)([O])([^|]*)\|", (RegexOptions.Multiline | RegexOptions.Compiled));


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
                Regex WildcardRegex = null;
                switch (Replacement.Key)
                {
                    case 'T':
                        WildcardRegex = TimeWildcardRegex;
                        break;

                    case 'I':
                        WildcardRegex = IdentifierWildcardRegex;
                        break;

                    case 'S':
                        WildcardRegex = SeverityWildcardRegex;
                        break;

                    case 'C':
                        WildcardRegex = ContentWildcardRegex;
                        break;

                    case 'O':
                        WildcardRegex = ObjectWildcardRegex;
                        break;


                    default:
                        throw new Exception($"Invalid wildcard key, couldn't find a replacement for `{Replacement.Key}`.");
                }

                Result = WildcardRegex.Replace(Result, new MatchEvaluator((Match Match) => {
                    bool IsSkippingSeverityHeader = (Replacement.Key == 'S' && Log.Severity.HasFlag(Severity.Generic));
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
            if (Log.Parameters != null)
            {
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


                foreach (KeyValuePair<string, Object> Parameter in PreprocessedLog.Parameters) {
                    PreprocessedLog.Content = PreprocessedLog.Content.Replace($"~{Parameter.Key}~", Parameter.Value.ToString());
                }

                return PreprocessedLog;
            }

            else {
                return Log;
            }
        }
    }
}
