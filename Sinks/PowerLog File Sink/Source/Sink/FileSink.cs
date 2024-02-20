using System.IO;
using System;
using PowerLog;

namespace PowerLog.Sinks.IO
{
    #region FileSink Class XML
    /// <summary>
    /// Writes logs to a file.
    /// </summary>
    #endregion
    public class FileSink : ISink
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

        #region Verbosity Severity XML
        /// <summary>
        /// The verbosity of the sink.
        /// </summary>
        #endregion
        public Severity Verbosity { get; set; }


        #region LogPath LogIO XML
        /// <summary>
        /// Holds data regarding log file paths.
        /// </summary>
        #endregion
        public LogIO LogPath { get; internal set; }


        // Private / Hidden variables..
        #region LogStream StreamWriter XML
        /// <summary>
        /// The log stream.
        /// </summary>
        #endregion
        private StreamWriter LogStream;


        #region Emit Function XML
        /// <inheritdoc/>
        #endregion
        public void Emit(Arguments Log)
        {
            if (Log.Severity.Passes(Verbosity)) {
                LogStream.Write($"{Log.FormattedLog}{Environment.NewLine}");
            }
        }

        #region Save Function XML
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        #endregion
        public void Save() { }

        #region Initialize Function XML
        /// <inheritdoc/>
        #endregion
        public void Initialize()
        {
            if (!Directory.Exists(LogPath.Path)) Directory.CreateDirectory(LogPath.Path);
            LogStream = new StreamWriter(LogPath.Get(), true);
            LogStream.AutoFlush = true;
        }

        #region Shutdown Function XML
        /// <inheritdoc/>
        #endregion
        public void Shutdown() {
            LogStream.Close();
            LogStream.Dispose();
        }

        #region Clear Function XML
        /// <inheritdoc/>
        #endregion
        public void Clear() { }

        private void HandleExit(Object S, EventArgs A) {
            Shutdown();
        }



        #region FileSink Constructor XML
        /// <summary>
        /// The default <see cref="FileSink"/> constructor.
        /// </summary>
        /// <param name="Identifier">The sink identifier.</param>
        /// <param name="Logger">The logger to push the sink to.</param>
        /// <param name="Verbosity">The sink verbosity.</param>
        /// <param name="LogPath">The log file path.</param>
        #endregion
        public FileSink(string Identifier, Log Logger, Severity Verbosity = PowerLog.Verbosity.All, LogIO LogPath = null) {
            this.Identifier = Identifier;
            this.Logger = Logger;
            this.Verbosity = Verbosity;

            AppDomain.CurrentDomain.ProcessExit += HandleExit;

            this.LogPath = ((LogPath != null) ? LogPath : new LogIO(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Logs", $"{((Logger.Identifier.IndexOfAny(Path.GetInvalidPathChars()) == -1) ? Logger.Identifier : "Miscellaneous")}"),
                 $"{((Logger.Identifier.IndexOfAny(Path.GetInvalidFileNameChars()) == -1) ? Logger.Identifier : "Log")} Output - ({DateTime.Now.ToString("HH-mm-ss tt, dd MMMM yyyy")})", "txt"));
        }
    }



    #region FileSinkUtilities Class XML
    /// <summary>
    /// Contains extension methods.
    /// </summary>
    #endregion
    public static class FileSinkUtilities
    {
        #region PushFile Function XML
        /// <summary>
        /// Pushes a new file sink on the logger sink stack.
        /// </summary>
        /// <param name="Logger">The logger to push the sink to.</param>
        /// <param name="Identifier">The sink identifier.</param>
        /// <param name="Verbosity">The sink verbosity.</param>
        /// <param name="LogPath">The log file path.</param>
        /// <returns>The current logger, to allow for builder patterns.</returns>
        #endregion
        public static Log PushFile(this Log Logger, string Identifier, Severity Verbosity = Verbosity.All, LogIO LogPath = null)
        {
            FileSink Sink = new FileSink(Identifier, Logger, Verbosity, LogPath);
            Logger.Push(Sink);

            return Logger;
        }
    }
}