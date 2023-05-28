using System.IO;
using System;
using PowerLog;

namespace PowerLog.Sinks.Markdown
{
    #region MarkdownSink Class XML
    /// <summary>
    /// Writes logs to a markdown file.
    /// </summary>
    #endregion
    public class MarkdownSink : ISink
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


        #region LogPath LogIO XML
        /// <summary>
        /// Holds data regarding log file paths.
        /// </summary>
        #endregion
        public LogIO LogPath { get; internal set; }

        #region InitializationDate DateTime XML
        /// <summary>
        /// Holds the initialization date of this sink.
        /// </summary>
        #endregion
        public DateTime InitializationDate { get; }


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
            // Very specific log formatting, but no markdown collisions.
            Arguments ProcessedLog = Log.Parse();

            LogStream.Write($"| {ProcessedLog.Time.ToString(ProcessedLog.Template.DateFormat)} | {((ProcessedLog.Logger != null && !String.IsNullOrEmpty(ProcessedLog.Logger.Identifier)) ? $"{ProcessedLog.Logger.Identifier}" : String.Empty)} | {((ProcessedLog.Severity != Severity.Generic) ? $"{ProcessedLog.Severity.ToString()}" : String.Empty)} | {ProcessedLog.Content} | {((ProcessedLog.Sender != null) ? ProcessedLog.Sender : "N/A")} |{Environment.NewLine}");
        }

        #region Save Function XML
        /// <inheritdoc/>
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

            LogStream.Write($"# {Logger.Identifier} Output{Environment.NewLine}### Logger '{Logger.Identifier}' started at {InitializationDate.ToString("HH-mm-ss tt, dd MMMM yyyy")}.{Environment.NewLine}<br/>{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}## Log Table{Environment.NewLine}{Environment.NewLine}| Timestamp | Logger Identifier | Severity | Content | Sender |{Environment.NewLine}|---|---|---|---|---|{Environment.NewLine}");
        }

        #region Shutdown Function XML
        /// Closes the log stream and handles sink shutdown.
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



        #region MarkdownSink Constructor XML
        /// <summary>
        /// The default <see cref="MarkdownSink"/> constructor.
        /// </summary>
        /// <param name="Identifier">The sink identifier.</param>
        /// <param name="Logger">The logger to push the sink to.</param>
        /// <param name="LogPath">The log file path.</param>
        #endregion
        public MarkdownSink(string Identifier, Log Logger, LogIO LogPath = null) {
            this.Identifier = Identifier;
            this.Logger = Logger;

            this.InitializationDate = DateTime.Now;

            AppDomain.CurrentDomain.ProcessExit += HandleExit;

            this.LogPath = ((LogPath != null) ? LogPath : new LogIO(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Markdown Logs", $"{((Logger.Identifier.IndexOfAny(Path.GetInvalidPathChars()) == -1) ? Logger.Identifier : "Miscellaneous")}"),
                 $"{((Logger.Identifier.IndexOfAny(Path.GetInvalidFileNameChars()) == -1) ? Logger.Identifier : "Log")} Markdown - ({DateTime.Now.ToString("HH-mm-ss tt, dd MMMM yyyy")})", "md"));
        }
    }



    #region MarkdownSinkUtilities Class XML
    /// <summary>
    /// Contains extension methods.
    /// </summary>
    #endregion
    public static class MarkdownSinkUtilities
    {
        #region PushMarkdown Function XML
        /// <summary>
        /// Pushes a new markdown file sink on the logger sink stack.
        /// </summary>
        /// <param name="Logger">The logger to push the sink to.</param>
        /// <param name="Identifier">The sink identifier.</param>
        /// <param name="LogPath">The log file path.</param>
        /// <returns>The current logger, to allow for builder patterns.</returns>
        #endregion
        public static Log PushMarkdown(this Log Logger, string Identifier, LogIO LogPath = null)
        {
            MarkdownSink Sink = new MarkdownSink(Identifier, Logger, LogPath);
            Logger.Push(Sink);

            return Logger;
        }
    }
}