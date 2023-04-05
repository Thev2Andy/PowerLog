using System.IO;
using System;
using System.Collections.Generic;
using Spectre.Console;
using PowerLog;
using System.Text.RegularExpressions;

namespace PowerLog.Sinks.SpectreTerminal
{
    #region ConsoleSink Class XML
    /// <summary>
    /// Writes logs to the console.
    /// </summary>
    #endregion
    public class SpectreConsoleSink : ISink
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

        #region EnableColors Boolean XML
        /// <summary>
        /// If true, the console logs will be colored.
        /// </summary>
        #endregion
        public bool EnableColors { get; set; }


        // Private / Hidden variables..
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
            { Severity.NA, new Color(255, 255, 255) },
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
            if (Log.Severity >= Verbosity)
            {
                Severity[] SeverityLevels = Enum.GetValues(typeof(Severity)) as Severity[];

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

                        if (HighlightedSeverities.Contains(SeverityLevels[I])) {
                            InvertBackgroundColor = true;
                        }
                    }
                }

                Color FinalColor = new Color(((Byte)(ColorRedChannel / SeverityFlagCount)), ((Byte)(ColorGreenChannel / SeverityFlagCount)), ((Byte)(ColorBlueChannel / SeverityFlagCount)));
                bool MatchedOverride = false;
                bool MatchedHighlight = false;
                for (int I = 0; I < Log.Parameters.Count; I++)
                {
                    if (Log.Parameters[I].Identifier == "Color Override" && !MatchedOverride)
                    {
                        Regex ColorOverrideRegex = new Regex(@"^\s*(\d{1,3})\s*([ ,\-])?\s*(\d{1,3})\s*([ ,\-])?\s*(\d{1,3})\s*$");
                        if (ColorOverrideRegex.IsMatch(Log.Parameters[I].Value.ToString()))
                        {
                            try
                            {
                                string[] OverrideChannels = Log.Parameters[I].Value.ToString().Split(new Char[] { ',', ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
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

                    if (Log.Parameters[I].Identifier == "Highlight Override" && !MatchedHighlight)
                    {
                        try {
                            InvertBackgroundColor = Convert.ToBoolean(Log.Parameters[I].Value);
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

                AnsiConsole.Write(new Text((Log.FormattedLog + Environment.NewLine), new Style(((EnableColors) ? FinalColor : null), AnsiConsole.Background, ((InvertBackgroundColor) ? Decoration.Invert : Decoration.None))));
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
        public void Clear() {
            AnsiConsole.Clear();
        }


        #region SpectreConsoleSink Constructor XML
        /// <summary>
        /// The default <see cref="SpectreConsoleSink"/> constructor.
        /// </summary>
        /// <param name="Identifier">The sink identifier.</param>
        /// <param name="Logger">The logger to push the sink to.</param>
        /// <param name="EnableColors">Should this sink print to the spectre console using colors?</param>
        /// <param name="Verbosity">The sink verbosity.</param>
        #endregion
        public SpectreConsoleSink(string Identifier, Log Logger, bool EnableColors = true, Severity Verbosity = Severity.Verbose) {
            this.Identifier = Identifier;
            this.Logger = Logger;
            this.Verbosity = Verbosity;
            this.EnableColors = EnableColors;
        }
    }



    #region SpectreConsoleSinkUtilities Class XML
    /// <summary>
    /// Contains extension methods.
    /// </summary>
    #endregion
    public static class SpectreConsoleSinkUtilities
    {
        #region PushSpectreConsole Function XML
        /// <summary>
        /// Pushes a new spectre console sink on the logger sink stack.
        /// </summary>
        /// <param name="Logger">The logger to push the sink to.</param>
        /// <param name="Identifier">The sink identifier.</param>
        /// <param name="EnableColors">Should this sink print to the spectre console using colors?</param>
        /// <param name="Verbosity">The sink verbosity.</param>
        /// <returns>The current logger, to allow for builder patterns.</returns>
        #endregion
        public static Log PushSpectreConsole(this Log Logger, string Identifier, bool EnableColors = true, Severity Verbosity = Severity.Verbose)
        {
            SpectreConsoleSink Sink = new SpectreConsoleSink(Identifier, Logger, EnableColors, Verbosity);
            Logger.Push(Sink);

            return Logger;
        }
    }
}