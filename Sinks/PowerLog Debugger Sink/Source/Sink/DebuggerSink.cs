using System.IO;
using System;
using PowerLog;
using System.Diagnostics;

namespace PowerLog.Sinks.Debugger
{
    #region DebuggerSink Class XML
    /// <summary>
    /// Writes logs to the currently attached debugger.
    /// </summary>
    #endregion
    public class DebuggerSink : ISink
    {
        #region Identifier String XML
        /// <summary>
        /// The sink identifier / name.
        /// </summary>
        #endregion
        public string Identifier { get; }

        #region Logger Log XML
        /// <summary>
        /// The sink logger.
        /// </summary>
        #endregion
        public Log Logger { get; }

        #region WriteInReleaseMode Boolean XML
        /// <summary>
        /// Determines what class will the sink use. (<see cref="Trace"/> / <see cref="Debug"/>)
        /// </summary>
        #endregion
        public bool WriteInReleaseMode { get; private set; }

        #region AllowedSeverities Severity XML
        /// <summary>
        /// The sink's allowed severity levels.
        /// </summary>
        #endregion
        public Severity AllowedSeverities { get; set; }

        #region StrictFiltering Boolean XML
        /// <summary>
        /// Verbosity test behaviour, determines if a given log needs to fully or partially match the allowed severities.
        /// </summary>
        #endregion
        public bool StrictFiltering { get; set; }

        #region Emit Function XML
        /// <inheritdoc/>
        #endregion
        public void Emit(Arguments Log)
        {
            if (Log.Severity.Passes(AllowedSeverities, StrictFiltering))
            {
                if (WriteInReleaseMode) {
                    Trace.WriteLine(Log.FormattedLog);
                }

                else {
                    Debug.WriteLine(Log.FormattedLog);
                }
            }
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


        #region DebuggerSink Constructor XML
        /// <summary>
        /// The default <see cref="DebuggerSink"/> constructor.
        /// </summary>
        /// <param name="Identifier">The sink identifier.</param>
        /// <param name="Logger">The logger to push the sink to.</param>
        /// <param name="AllowedSeverities">The sink's allowed severity levels.</param>
        /// <param name="WriteInReleaseMode">Determines what class will the sink use. (<see cref="Trace"/> / <see cref="Debug"/>)</param>
        #endregion
        public DebuggerSink(string Identifier, Log Logger, Severity AllowedSeverities = Verbosity.All, bool WriteInReleaseMode = true)
        {
            this.Identifier = Identifier;
            this.Logger = Logger;
            this.WriteInReleaseMode = WriteInReleaseMode;
            this.AllowedSeverities = AllowedSeverities;
            this.StrictFiltering = true;
        }
    }



    #region DebuggerSinkUtilities Class XML
    /// <summary>
    /// Contains extension methods.
    /// </summary>
    #endregion
    public static class DebuggerSinkUtilities
    {
        #region PushDebugger Function XML
        /// <summary>
        /// Pushes a new debugger sink on the logger sink stack.
        /// </summary>
        /// <param name="Logger">The logger to push the sink to.</param>
        /// <param name="Identifier">The sink identifier.</param>
        /// <param name="AllowedSeverities">The sink's allowed severity levels.</param>
        /// <param name="WriteInReleaseMode">Determines what class will the sink use. (<see cref="Trace"/> / <see cref="Debug"/>)</param>
        /// <returns>The current logger, to allow for builder patterns.</returns>
        #endregion
        public static Log PushDebugger(this Log Logger, string Identifier, Severity AllowedSeverities = Verbosity.All, bool WriteInReleaseMode = true)
        {
            DebuggerSink Sink = new DebuggerSink(Identifier, Logger, AllowedSeverities, WriteInReleaseMode);
            Logger.Push(Sink);

            return Logger;
        }
    }
}