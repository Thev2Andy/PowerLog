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


        #region Compose Function XML
        /// <summary>
        /// Parses and composes a log using the template by using regular expressions, then returns it as a string.
        /// </summary>
        /// <returns>The composed log string.</returns>
        /// <param name="Log">The log to parse and compose.</param>
        /// <param name="Template">Formatting template used in composing the log.</param>
        /// <param name="Evaluate">Whether or not parameters are evaluated, parsed and replaced.</param>
        #endregion
        public static string Compose(this Arguments Log, Template Template, bool Evaluate)
        {
            if (String.IsNullOrEmpty(Log.Content)) {
                return String.Empty;
            }

            if (Evaluate && Template.Flags.HasFlag(Template.Options.Parse)) {
                Log = Log.Parse();
            }

            Dictionary<char, string> Replacements = new Dictionary<char, string>
            {
                { 'T', Log.Time.ToString(Template.Date) },
                { 'I', Log.Logger?.Identifier ?? "N/A"  },
                { 'S', Log.Severity.ToString() },
                { 'C', Log.Content ?? String.Empty },
                { 'O', Log.Sender?.ToString() ?? "N/A" },
            };

            
            string Result = Template.Format;
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
                    bool IsDiscarding = false;

                    if (Replacement.Key == 'I' && Log.Template.Flags.HasFlag(Template.Options.ConditionalLogger) && Log.Logger == null) {
                        IsDiscarding = true;
                    }

                    if (Replacement.Key == 'S' && Log.Template.Flags.HasFlag(Template.Options.ConditionalSeverity) && Log.Severity.HasFlag(Severity.Generic)) {
                        IsDiscarding = true;
                    }

                    if (Replacement.Key == 'O' && Log.Template.Flags.HasFlag(Template.Options.ConditionalObject) && Log.Sender == null) {
                        IsDiscarding = true;
                    }

                    return ((!IsDiscarding) ? ($"{Match.Groups[1].Value}" + $"{Replacement.Value}" + $"{Match.Groups[3].Value}") : String.Empty);
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
            string ParsedContent = Log.Content;

            if (Log.Parameters != null)
            {
                foreach (KeyValuePair<string, Object> Parameter in Log.Parameters) {
                    ParsedContent = ParsedContent.Replace($"~{Parameter.Key}~", ((Parameter.Value != null) ? Parameter.Value.ToString() : String.Empty));
                }
            }

            if (Log.Context != null)
            {
                foreach (KeyValuePair<string, Object> ContextualProperty in Log.Context) {
                    ParsedContent = ParsedContent.Replace($"~${ContextualProperty.Key}~", ((ContextualProperty.Value != null) ? ContextualProperty.Value.ToString() : String.Empty));
                }
            }

            if (Log.Enrichments != null)
            {
                foreach (KeyValuePair<string, Object> Enrichment in Log.Enrichments) {
                    ParsedContent = ParsedContent.Replace($"~@{Enrichment.Key}~", ((Enrichment.Value != null) ? Enrichment.Value.ToString() : String.Empty));
                }
            }

            Arguments ParsedLog = new Arguments()
            {
                Content = ParsedContent,
                Severity = Log.Severity,
                Time = Log.Time,
                Template = Log.Template,
                Sender = Log.Sender,
                Parameters = Log.Parameters,

                Logger = Log.Logger
            };

            return ParsedLog;
        }
    }
}
