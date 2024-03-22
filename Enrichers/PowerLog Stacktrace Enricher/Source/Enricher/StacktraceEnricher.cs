using System;
using System.Collections.Generic;
using System.Diagnostics;
using PowerLog;

namespace PowerLog.Enrichers.Stacktrace
{
    #region StacktraceEnricher Class XML
    /// <summary>
    /// Captures and enriches logs with stacktrace information.
    /// </summary>
    #endregion
    public class StacktraceEnricher : IEnricher
    {
        #region Identifier String XML
        /// <summary>
        /// The enricher identifier / name.
        /// </summary>
        #endregion
        public string Identifier { get; }

        #region Logger Log XML
        /// <summary>
        /// The enricher logger.
        /// </summary>
        #endregion
        public Log Logger { get; }

        #region IsEnabled Boolean XML
        /// <summary>
        /// Determines if the enricher is enabled.
        /// </summary>
        #endregion
        public bool IsEnabled { get; set; }


        #region Enrich Method XML
        /// <inheritdoc/>
        #endregion
        public void Enrich(in Dictionary<string, Object> Enrichments)
        {
            Enrichments.TryAdd("Stacktrace", new StackTrace(2, true));
        }



        #region StacktraceEnricher Constructor XML
        /// <summary>
        /// The default <see cref="StacktraceEnricher"/> constructor.
        /// </summary>
        /// <param name="Identifier">The enricher identifier.</param>
        /// <param name="Logger">The logger to append the enricher to.</param>
        #endregion
        public StacktraceEnricher(string Identifier, Log Logger)
        {
            this.Identifier = Identifier;
            this.Logger = Logger;

            this.IsEnabled = true;
        }
    }



    #region StacktraceEnricherUtilities Class XML
    /// <summary>
    /// Contains extension methods.
    /// </summary>
    #endregion
    public static class StacktraceEnricherUtilities
    {
        #region AppendStacktrace Function XML
        /// <summary>
        /// Appends a new stacktrace enricher on the logger enricher stack.
        /// </summary>
        /// <param name="Logger">The logger to append the enricher to.</param>
        /// <param name="Identifier">The enricher identifier.</param>
        /// <returns>The current logger, to allow for method chaining.</returns>
        #endregion
        public static Log AppendStacktrace(this Log Logger, string Identifier)
        {
            StacktraceEnricher Enricher = new StacktraceEnricher(Identifier, Logger);
            Logger.Push(Enricher);

            return Logger;
        }
    }
}