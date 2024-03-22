using System;
using System.Collections.Generic;
using System.Diagnostics;
using PowerLog;

namespace PowerLog.Enrichers.Environment
{
    #region EnvironmentEnricher Class XML
    /// <summary>
    /// Captures and enriches logs with environment information.
    /// </summary>
    #endregion
    public class EnvironmentEnricher : IEnricher
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

        #region EnvironmentVariables Tuple List XML
        /// <summary>
        /// List of environment variable names to enrich the logs with.
        /// </summary>
        #endregion
        public List<(string EnrichmentName, string Variable)> EnvironmentVariables { get; set; }


        #region Enrich Method XML
        /// <inheritdoc/>
        #endregion
        public void Enrich(in Dictionary<string, Object> Enrichments)
        {
            Enrichments.TryAdd("Machine Name", System.Environment.MachineName);
            Enrichments.TryAdd("Environment User Name", System.Environment.UserName);
            Enrichments.TryAdd("CLR Version", System.Environment.Version);
            Enrichments.TryAdd("OS Version", System.Environment.OSVersion);


            string EnvironmentName = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (String.IsNullOrWhiteSpace(EnvironmentName)) {
                EnvironmentName = System.Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
            }

            if (String.IsNullOrWhiteSpace(EnvironmentName)) {
                EnvironmentName = "Production";
            }

            Enrichments.TryAdd($"Environment Name", EnvironmentName);


            for (int I = 0; I < EnvironmentVariables.Count; I++)
            {
                string Variable = System.Environment.GetEnvironmentVariable(EnvironmentVariables[I].Variable);
                if (!String.IsNullOrEmpty(Variable)) {
                    Enrichments.TryAdd(EnvironmentVariables[I].EnrichmentName, Variable);
                }
            }
        }



        #region EnvironmentEnricher Constructor XML
        /// <summary>
        /// The default <see cref="EnvironmentEnricher"/> constructor.
        /// </summary>
        /// <param name="Identifier">The enricher identifier.</param>
        /// <param name="Logger">The logger to append the enricher to.</param>
        /// <param name="EnvironmentVariables">List of environment variable names to enrich the logs with.</param>
        #endregion
        public EnvironmentEnricher(string Identifier, Log Logger, List<(string EnrichmentName, string Variable)> EnvironmentVariables = null)
        {
            this.Identifier = Identifier;
            this.Logger = Logger;
            this.EnvironmentVariables = EnvironmentVariables ?? new List<(string EnrichmentName, string Variable)>();

            this.IsEnabled = true;
        }
    }



    #region EnvironmentEnricherUtilities Class XML
    /// <summary>
    /// Contains extension methods.
    /// </summary>
    #endregion
    public static class EnvironmentEnricherUtilities
    {
        #region AppendEnvironment Function XML
        /// <summary>
        /// Appends a new environment enricher on the logger enricher stack.
        /// </summary>
        /// <param name="Logger">The logger to append the enricher to.</param>
        /// <param name="Identifier">The enricher identifier.</param>
        /// <param name="EnvironmentVariables">List of environment variable names to enrich the logs with.</param>
        /// <returns>The current logger, to allow for method chaining.</returns>
        #endregion
        public static Log AppendEnvironment(this Log Logger, string Identifier, List<(string EnrichmentName, string Variable)> EnvironmentVariables = null)
        {
            EnvironmentEnricher Enricher = new EnvironmentEnricher(Identifier, Logger, EnvironmentVariables);
            Logger.Push(Enricher);

            return Logger;
        }
    }
}