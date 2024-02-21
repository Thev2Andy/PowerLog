﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PowerLog;

namespace PowerLog.Sinks.Markdown
{
    #region MarkdownSink Class XML
    /// <summary>
    /// Writes logs to a colored markdown file.
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
        private readonly Regex ColorOverrideRegex = new Regex(@"^\s*(\d{1,3})\s*([ ,\-])?\s*(\d{1,3})\s*([ ,\-])?\s*(\d{1,3})\s*$", RegexOptions.Compiled);
        private Severity[] SeverityLevels;
        private StreamWriter LogStream;

        private readonly Dictionary<Severity, Color> TerminalColorLUT = new Dictionary<Severity, Color>() {
            { Severity.Verbose, new Color(64, 64, 64) },
            { Severity.Trace, new Color(128, 128, 128) },
            { Severity.Debug, new Color(192, 192, 192) },
            { Severity.Information, new Color(255, 255, 255) },
            { Severity.Network, new Color(65, 135, 255) },
            { Severity.Notice, new Color(255, 240, 195) },
            { Severity.Caution, new Color(255, 230, 165) },
            { Severity.Warning, new Color(255, 215, 128) },
            { Severity.Alert, new Color(255, 170, 128) },
            { Severity.Error, new Color(255, 128, 128) },
            { Severity.Critical, new Color(255, 100, 100) },
            { Severity.Emergency, new Color(255, 85, 85) },
            { Severity.Fatal, new Color(192, 64, 64) },
            { Severity.Generic, new Color(255, 255, 255) },
        };

        private readonly List<Severity> HighlightedSeverities = new List<Severity>() {
            Severity.Critical,
            Severity.Emergency,
            Severity.Fatal,
        };


        #region Emit Function XML
        /// <inheritdoc/>
        #endregion
        public void Emit(Arguments Log)
        {
            // Very specific log formatting, but no markdown collisions.
            Arguments ProcessedLog = Log.Parse();

            if (Log.Severity.Passes(AllowedSeverities, StrictFiltering))
            {
                bool InvertBackgroundColor = false;
                int SeverityFlagCount = 0;
                int ColorRedChannel = 0;
                int ColorGreenChannel = 0;
                int ColorBlueChannel = 0;

                for (int I = 0; I < SeverityLevels.Length; I++)
                {
                    if (Log.Severity.HasFlag(SeverityLevels[I]))
                    {
                        ColorRedChannel += TerminalColorLUT[SeverityLevels[I]].R;
                        ColorGreenChannel += TerminalColorLUT[SeverityLevels[I]].G;
                        ColorBlueChannel += TerminalColorLUT[SeverityLevels[I]].B;
                        SeverityFlagCount++;

                        if (HighlightedSeverities.Contains(SeverityLevels[I]) && !InvertBackgroundColor) {
                            InvertBackgroundColor = true;
                        }
                    }
                }

                Color FinalColor = new Color(((Byte)(ColorRedChannel / SeverityFlagCount)), ((Byte)(ColorGreenChannel / SeverityFlagCount)), ((Byte)(ColorBlueChannel / SeverityFlagCount)));
                bool MatchedOverride = false;
                bool MatchedHighlight = false;
                foreach (KeyValuePair<string, Object> Parameter in Log.Parameters)
                {
                    if (Parameter.Key == "Color Override" && !MatchedOverride)
                    {
                        if (ColorOverrideRegex.IsMatch(Parameter.Value.ToString()))
                        {
                            try
                            {
                                string[] OverrideChannels = Parameter.Value.ToString().Split(new Char[] { ',', ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
                                Byte OverrideRedChannel = Convert.ToByte(OverrideChannels[0]);
                                Byte OverrideGreenChannel = Convert.ToByte(OverrideChannels[1]);
                                Byte OverrideBlueChannel = Convert.ToByte(OverrideChannels[2]);

                                FinalColor = new Color(OverrideRedChannel, OverrideGreenChannel, OverrideBlueChannel);
                            }

                            catch (Exception) {
                                Logger.Error("An error occured while trying to override the color of the log.", null, null, this.Identifier);
                            }
                        }

                        MatchedOverride = true;
                    }

                    if (Parameter.Key == "Highlight Override" && !MatchedHighlight)
                    {
                        try {
                            InvertBackgroundColor = Convert.ToBoolean(Parameter.Value);
                        }

                        catch (Exception) {
                            Logger.Error("An error occured while trying to invert the color of the log.", null, null, this.Identifier);
                        }

                        MatchedHighlight = true;
                    }


                    if (MatchedOverride && MatchedHighlight) {
                        break;
                    }
                }

                string SeverityField = this.ComposeSeverity(ProcessedLog.Severity, FinalColor, InvertBackgroundColor);
                LogStream.Write($"| {ProcessedLog.Time.ToString(ProcessedLog.Template.DateFormat)} | {((ProcessedLog.Logger != null && !String.IsNullOrEmpty(ProcessedLog.Logger.Identifier)) ? $"{ProcessedLog.Logger.Identifier}" : String.Empty)} | {SeverityField} | {ProcessedLog.Content} | {((ProcessedLog.Sender != null) ? ProcessedLog.Sender : "N/A")} |{Environment.NewLine}");
            }
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

            LogStream.Write($"# {Logger.Identifier} Output{Environment.NewLine}* Sink started at {InitializationDate.ToString("HH:mm:ss, dddd, dd MMMM yyyy")}.{Environment.NewLine}{Environment.NewLine}# Output History{Environment.NewLine}| Timestamp | Logger | Severity | Content | Sender |{Environment.NewLine}| --------- | ------ | -------- | ------- | ------ |{Environment.NewLine}");
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


        private string ComposeSeverity(Severity Severity, Color Color, bool Highlight) {
            return $"<span style=\"{((Highlight) ? "background-" : String.Empty)}color: rgb({Color.R}, {Color.G}, {Color.B})\">{Severity}</span>";
        }

        private void HandleExit(Object S, EventArgs A) {
            Shutdown();
        }



        #region MarkdownSink Constructor XML
        /// <summary>
        /// The default <see cref="MarkdownSink"/> constructor.
        /// </summary>
        /// <param name="Identifier">The sink identifier.</param>
        /// <param name="Logger">The logger to push the sink to.</param>
        /// <param name="AllowedSeverities">The sink's allowed severity levels.</param>
        /// <param name="LogPath">The log file path.</param>
        #endregion
        public MarkdownSink(string Identifier, Log Logger, Severity AllowedSeverities = Verbosity.All, LogIO LogPath = null)
        {
            this.Identifier = Identifier;
            this.Logger = Logger;
            this.AllowedSeverities = AllowedSeverities;
            this.StrictFiltering = true;

            this.InitializationDate = DateTime.Now;
            SeverityLevels = Enum.GetValues<Severity>();

            AppDomain.CurrentDomain.ProcessExit += HandleExit;

            this.LogPath = ((LogPath != null) ? LogPath : new LogIO(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Markdown Logs", $"{((Logger.Identifier.IndexOfAny(Path.GetInvalidPathChars()) == -1) ? Logger.Identifier : "Miscellaneous")}"),
                 $"{((Logger.Identifier.IndexOfAny(Path.GetInvalidFileNameChars()) == -1) ? Logger.Identifier : "Log")} Markdown - ({DateTime.Now.ToString("HH-mm-ss tt, dd MMMM yyyy")})", "md"));
        }



        private struct Color
        {
            public Byte R;
            public Byte G;
            public Byte B;

            public Color(Byte R, Byte G, Byte B)
            {
                this.R = R;
                this.G = G;
                this.B = B;
            }
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
        /// <param name="AllowedSeverities">The sink's allowed severity levels.</param>
        /// <param name="LogPath">The log file path.</param>
        /// <returns>The current logger, to allow for builder patterns.</returns>
        #endregion
        public static Log PushMarkdown(this Log Logger, string Identifier, Severity AllowedSeverities = Verbosity.All, LogIO LogPath = null)
        {
            MarkdownSink Sink = new MarkdownSink(Identifier, Logger, AllowedSeverities, LogPath);
            Logger.Push(Sink);

            return Logger;
        }
    }
}