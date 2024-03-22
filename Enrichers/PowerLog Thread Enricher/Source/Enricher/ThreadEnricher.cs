using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using PowerLog;

namespace PowerLog.Enrichers.Thread
{
    #region ThreadEnricher Class XML
    /// <summary>
    /// Captures and enriches logs with current thread information.
    /// </summary>
    #endregion
    public class ThreadEnricher : IEnricher
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
            Enrichments.TryAdd("Thread Name", System.Threading.Thread.CurrentThread.Name);
            Enrichments.TryAdd("Thread ID", System.Threading.Thread.CurrentThread.ManagedThreadId);
            Enrichments.TryAdd("Thread State", System.Threading.Thread.CurrentThread.ThreadState);
            Enrichments.TryAdd("Thread Priority", System.Threading.Thread.CurrentThread.Priority);
        }



        #region ThreadEnricher Constructor XML
        /// <summary>
        /// The default <see cref="ThreadEnricher"/> constructor.
        /// </summary>
        /// <param name="Identifier">The enricher identifier.</param>
        /// <param name="Logger">The logger to append the enricher to.</param>
        #endregion
        public ThreadEnricher(string Identifier, Log Logger)
        {
            this.Identifier = Identifier;
            this.Logger = Logger;

            this.IsEnabled = true;
        }
    }



    #region ThreadEnricherUtilities Class XML
    /// <summary>
    /// Contains extension methods.
    /// </summary>
    #endregion
    public static class ThreadEnricherUtilities
    {
        #region AppendThread Function XML
        /// <summary>
        /// Appends a new thread enricher on the logger enricher stack.
        /// </summary>
        /// <param name="Logger">The logger to append the enricher to.</param>
        /// <param name="Identifier">The enricher identifier.</param>
        /// <returns>The current logger, to allow for method chaining.</returns>
        #endregion
        public static Log AppendThread(this Log Logger, string Identifier)
        {
            ThreadEnricher Enricher = new ThreadEnricher(Identifier, Logger);
            Logger.Push(Enricher);

            return Logger;
        }
    }
}