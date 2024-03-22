using System;
using System.Diagnostics;
using PowerLog;

namespace PowerLog.Sinks.Logger
{
    #region LoggerSink Class XML
    /// <summary>
    /// Writes logs to another <see cref="Log"/> instance.
    /// </summary>
    #endregion
    public class LoggerSink : ISink
    {
        #region Identifier String XML
        /// <summary>
        /// The sink identifier / name.
        /// </summary>
        #endregion
        public string Identifier { get; }

        #region Logger Log XML
        /// <summary>
        /// The sink logger. (The one this sink is attached to.)
        /// </summary>
        #endregion
        public Log Logger { get; }

        #region TargetLogger Log XML
        /// <summary>
        /// The sink's target logger. (The one this sink pushes messages to.)
        /// </summary>
        #endregion
        public Log TargetLogger { get; }

        #region AllowedSeverities Severity XML
        /// <summary>
        /// The sink's allowed severity levels.
        /// </summary>
        #endregion
        public Severity AllowedSeverities { get; set; }

        #region StrictFiltering Boolean XML
        /// <summary>
        /// Determines whether a log needs to fully or partially match the allowed severities.
        /// </summary>
        #endregion
        public bool StrictFiltering { get; set; }

        #region IsEnabled Boolean XML
        /// <summary>
        /// Determines if the sink is enabled.
        /// </summary>
        #endregion
        public bool IsEnabled { get; set; }

        #region NormalizeTemplate Boolean XML
        /// <summary>
        /// If <see langword="true"/>, will override the template in logs sent by the current logger and instead use target logger's template.
        /// </summary>
        #endregion
        public bool NormalizeTemplate { get; set; }


        #region Emit Function XML
        /// <inheritdoc/>
        #endregion
        public void Emit(Arguments Log)
        {
            if (NormalizeTemplate) {
                Log = Log with { Template = TargetLogger.FormattingTemplate };
            }

            TargetLogger.Raw(Log);
        }

        #region Initialize Function XML
        /// <inheritdoc/>
        #endregion
        public void Initialize() { }

        #region Shutdown Function XML
        /// <inheritdoc/>
        #endregion
        public void Shutdown() { }

        #region Save Function XML
        /// <inheritdoc/>
        #endregion
        public void Save() { }

        #region Clear Function XML
        /// <inheritdoc/>
        #endregion
        public void Clear() { }


        #region LoggerSink Constructor XML
        /// <summary>
        /// The default <see cref="LoggerSink"/> constructor.
        /// </summary>
        /// <param name="Identifier">The sink identifier.</param>
        /// <param name="TargetLogger">The logger to emit messages to.</param>
        /// <param name="Logger">The logger to push the sink to.</param>
        /// <param name="AllowedSeverities">The sink's allowed severity levels.</param>
        /// <param name="NormalizeTemplate">If <see langword="true"/>, will override the template in logs sent by the current logger and instead use target logger's template.</param>
        #endregion
        public LoggerSink(string Identifier, Log TargetLogger, Log Logger, Severity AllowedSeverities = Verbosity.All, bool NormalizeTemplate = false)
        {
            this.Identifier = Identifier;
            this.Logger = Logger;
            this.TargetLogger = TargetLogger;
            this.AllowedSeverities = AllowedSeverities;
            this.StrictFiltering = true;
            this.IsEnabled = true;
            this.NormalizeTemplate = NormalizeTemplate;
        }
    }



    #region LoggerSinkUtilities Class XML
    /// <summary>
    /// Contains extension methods.
    /// </summary>
    #endregion
    public static class LoggerSinkUtilities
    {
        #region PushLogger Function XML
        /// <summary>
        /// Pushes a new logger sink on the logger sink stack.
        /// </summary>
        /// <param name="Logger">The logger to push the sink to.</param>
        /// <param name="Identifier">The sink identifier.</param>
        /// <param name="TargetLogger">The logger to emit messages to.</param>
        /// <param name="AllowedSeverities">The sink's allowed severity levels.</param>
        /// <param name="NormalizeTemplate">If <see langword="true"/>, will override the template in logs sent by the current logger and instead use target logger's template.</param>
        /// <returns>The current logger, to allow for method chaining.</returns>
        #endregion
        public static Log PushLogger(this Log Logger, string Identifier, Log TargetLogger, Severity AllowedSeverities = Verbosity.All, bool NormalizeTemplate = false)
        {
            LoggerSink Sink = new LoggerSink(Identifier, TargetLogger, Logger, AllowedSeverities, NormalizeTemplate);
            Logger.Push(Sink);

            return Logger;
        }
    }
}