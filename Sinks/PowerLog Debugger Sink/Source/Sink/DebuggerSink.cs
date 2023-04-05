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

        #region Verbosity Severity XML
        /// <summary>
        /// The verbosity of the sink.
        /// </summary>
        #endregion
        public Severity Verbosity { get; set; }

        #region Emit Function XML
        /// <inheritdoc/>
        #endregion
        public void Emit(Arguments Log)
        {
            if (Log.Severity >= Verbosity)
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
        /// <inheritdoc/>
        #endregion
        public DebuggerSink(string Identifier, Log Logger, Severity Verbosity = Severity.Verbose, bool WriteInReleaseMode = true)
        {
            this.Identifier = Identifier;
            this.Logger = Logger;
            this.WriteInReleaseMode = WriteInReleaseMode;
            this.Verbosity = Verbosity;
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
        /// <param name="Verbosity">The sink verbosity.</param>
        /// <param name="WriteInReleaseMode">Determines what class will the sink use. (<see cref="Trace"/> / <see cref="Debug"/>)</param>
        /// <returns>The current logger, to allow for builder patterns.</returns>
        #endregion
        public static Log PushDebugger(this Log Logger, string Identifier, Severity Verbosity = Severity.Verbose, bool WriteInReleaseMode = true)
        {
            DebuggerSink Sink = new DebuggerSink(Identifier, Logger, Verbosity, WriteInReleaseMode);
            Logger.Push(Sink);

            return Logger;
        }
    }
}