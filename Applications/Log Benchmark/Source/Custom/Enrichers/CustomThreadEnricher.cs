using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using PowerLog;

namespace LogBenchmark
{
    #region CustomThreadEnricher Class XML
    /// <summary>
    /// Custom thread enricher for PowerLog, with the exact same enrichments as Serilog's thread enricher.<br/>Captures and enriches logs with current thread information.
    /// </summary>
    #endregion
    public class CustomThreadEnricher : IEnricher
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
            Enrichments.TryAdd("Thread Name", Thread.CurrentThread.Name);
            Enrichments.TryAdd("Thread ID", Thread.CurrentThread.ManagedThreadId);

            // Removed Properties:
            //      * Enrichments.TryAdd("Thread State", Thread.CurrentThread.ThreadState);
            //      * Enrichments.TryAdd("Thread Priority", Thread.CurrentThread.Priority);
        }



        #region CustomThreadEnricher Constructor XML
        /// <summary>
        /// The default <see cref="ThreadEnricher"/> constructor.
        /// </summary>
        /// <param name="Identifier">The enricher identifier.</param>
        /// <param name="Logger">The logger to append the enricher to.</param>
        #endregion
        public CustomThreadEnricher(string Identifier, Log Logger)
        {
            this.Identifier = Identifier;
            this.Logger = Logger;

            this.IsEnabled = true;
        }
    }



    #region CustomThreadEnricherUtilities Class XML
    /// <summary>
    /// Contains extension methods.
    /// </summary>
    #endregion
    public static class CustomThreadEnricherUtilities
    {
        #region AppendThread Function XML
        /// <summary>
        /// Appends a new thread enricher on the logger enricher stack.
        /// </summary>
        /// <param name="Logger">The logger to append the enricher to.</param>
        /// <param name="Identifier">The enricher identifier.</param>
        /// <returns>The current logger, to allow for method chaining.</returns>
        #endregion
        public static Log AppendCustomThread(this Log Logger, string Identifier)
        {
            CustomThreadEnricher Enricher = new CustomThreadEnricher(Identifier, Logger);
            Logger.Push(Enricher);

            return Logger;
        }
    }
}
