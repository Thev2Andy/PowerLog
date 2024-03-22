using System;
using System.Collections.Generic;
using System.Diagnostics;
using PowerLog;

namespace PowerLog.Enrichers.Process
{
    #region ProcessEnricher Class XML
    /// <summary>
    /// Captures and enriches logs with process information.
    /// </summary>
    #endregion
    public class ProcessEnricher : IEnricher
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

        #region AutomaticRefresh Boolean XML
        /// <summary>
        /// Determines if the process object gets automatically refreshed.
        /// </summary>
        #endregion
        public bool AutomaticRefresh { get; set; }


        // Private / Hidden variables..
        private System.Diagnostics.Process CurrentProcess;


        #region Enrich Method XML
        /// <inheritdoc/>
        #endregion
        public void Enrich(in Dictionary<string, Object> Enrichments)
        {
            if (AutomaticRefresh) { 
                this.Refresh();
            }

            Enrichments.TryAdd("Process ID", CurrentProcess.Id);
            Enrichments.TryAdd("Process Name", CurrentProcess.ProcessName);
            Enrichments.TryAdd("Process Start Time", CurrentProcess.StartTime);
        }

        #region Refresh Method XML
        /// <summary>
        /// Refreshes the process object.
        /// </summary>
        #endregion
        public void Refresh() {
            CurrentProcess.Refresh();
        }



        #region ProcessEnricher Constructor XML
        /// <summary>
        /// The default <see cref="ProcessEnricher"/> constructor.
        /// </summary>
        /// <param name="Identifier">The enricher identifier.</param>
        /// <param name="Logger">The logger to append the enricher to.</param>
        /// <param name="AutomaticRefresh">Determines if the process object gets automatically refreshed.</param>
        #endregion
        public ProcessEnricher(string Identifier, Log Logger, bool AutomaticRefresh = false)
        {
            this.Identifier = Identifier;
            this.Logger = Logger;
            this.AutomaticRefresh = AutomaticRefresh;

            this.CurrentProcess = System.Diagnostics.Process.GetCurrentProcess();
            this.IsEnabled = true;
        }
    }



    #region ProcessEnricherUtilities Class XML
    /// <summary>
    /// Contains extension methods.
    /// </summary>
    #endregion
    public static class ProcessEnricherUtilities
    {
        #region AppendProcess Function XML
        /// <summary>
        /// Appends a new process enricher on the logger enricher stack.
        /// </summary>
        /// <param name="Logger">The logger to append the enricher to.</param>
        /// <param name="Identifier">The enricher identifier.</param>
        /// <param name="AutomaticRefresh">Determines if the process object gets automatically refreshed.</param>
        /// <returns>The current logger, to allow for method chaining.</returns>
        #endregion
        public static Log AppendProcess(this Log Logger, string Identifier, bool AutomaticRefresh = false)
        {
            ProcessEnricher Enricher = new ProcessEnricher(Identifier, Logger, AutomaticRefresh);
            Logger.Push(Enricher);

            return Logger;
        }
    }
}