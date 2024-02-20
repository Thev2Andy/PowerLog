﻿using System.IO;
using System;
using PowerLog;
using System.Collections.Generic;
using System.Drawing;

namespace PowerLog.Sinks.Terminal
{
    #region ConsoleSink Class XML
    /// <summary>
    /// Writes logs to the console.
    /// </summary>
    #endregion
    public class ConsoleSink : ISink
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
            if (Log.Severity.Passes(Verbosity))
            {
                ConsoleColor OldForeground = Console.ForegroundColor;
                ConsoleColor OldBackground = Console.BackgroundColor;

                ConsoleColor TargetForegroundColor = OldForeground;
                ConsoleColor TargetBackgroundColor = OldBackground;

                if (EnableColors)
                {
                    // Compares the internal severity values, then matches it to a console color.
                    // Before you ask, I don't think you can do this with a 'switch' statement.
                    if (Log.Severity >= Severity.Verbose && Log.Severity < Severity.Trace) TargetForegroundColor = ConsoleColor.DarkGray; // Verbose.
                    if (Log.Severity >= Severity.Trace && Log.Severity < Severity.Debug) TargetForegroundColor = ConsoleColor.Gray; // Trace.
                    if (Log.Severity >= Severity.Debug && Log.Severity < Severity.Network) TargetForegroundColor = ConsoleColor.Gray; // Debug.
                    if (Log.Severity >= Severity.Network && Log.Severity < Severity.Information) TargetForegroundColor = ConsoleColor.Blue; // Network.
                    if (Log.Severity >= Severity.Information && Log.Severity < Severity.Notice) TargetForegroundColor = ConsoleColor.White; // Information.
                    if (Log.Severity >= Severity.Notice && Log.Severity < Severity.Caution) TargetForegroundColor = ConsoleColor.White; // Notice.
                    if (Log.Severity >= Severity.Caution && Log.Severity < Severity.Warning) TargetForegroundColor = ConsoleColor.Yellow; // Caution.
                    if (Log.Severity >= Severity.Warning && Log.Severity < Severity.Alert) TargetForegroundColor = ConsoleColor.Yellow; // Warning.
                    if (Log.Severity >= Severity.Alert && Log.Severity < Severity.Error) TargetForegroundColor = ConsoleColor.DarkYellow; // Alert.
                    if (Log.Severity >= Severity.Error && Log.Severity < Severity.Critical) TargetForegroundColor = ConsoleColor.Red; // Error.
                    if (Log.Severity >= Severity.Critical && Log.Severity < Severity.Emergency) TargetForegroundColor = ConsoleColor.Red; // Critical.
                    if (Log.Severity >= Severity.Emergency && Log.Severity < Severity.Fatal) TargetForegroundColor = ConsoleColor.DarkRed; // Emergency.
                    if (Log.Severity >= Severity.Fatal && Log.Severity < Severity.Generic) TargetForegroundColor = ConsoleColor.DarkRed; // Fatal.
                    if (Log.Severity >= Severity.Generic) TargetForegroundColor = OldForeground; // Generic.



                    Severity[] SeverityLevels = Enum.GetValues(typeof(Severity)) as Severity[];
                    bool InvertBackgroundColor = false;
                    for (int I = 0; I < SeverityLevels.Length; I++)
                    {
                        if (Log.Severity.HasFlag(SeverityLevels[I]) && HighlightedSeverities.Contains(SeverityLevels[I]))
                        {
                            InvertBackgroundColor = true;
                            break;
                        }
                    }

                    foreach (KeyValuePair<string, Object> Parameter in Log.Parameters)
                    {
                        if (Parameter.Key == "Highlight Override")
                        {
                            try {
                                InvertBackgroundColor = Convert.ToBoolean(Parameter.Value);
                            }

                            catch (Exception) {
                                Logger.Error("An error occured while trying to invert the color of the log.", null, null, this.Identifier);
                            }
                        }
                    }

                    if (InvertBackgroundColor)
                    {
                        ConsoleColor BackupColor = TargetForegroundColor;
                        TargetForegroundColor = TargetBackgroundColor;
                        TargetBackgroundColor = BackupColor;
                    }
                }


                Console.ForegroundColor = TargetForegroundColor;
                Console.BackgroundColor = TargetBackgroundColor;

                Console.WriteLine(Log.FormattedLog);

                Console.ForegroundColor = OldForeground;
                Console.BackgroundColor = OldBackground;
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
            Console.Clear();
        }



        #region ConsoleSink Constructor XML
        /// <summary>
        /// The default <see cref="ConsoleSink"/> constructor.
        /// </summary>
        /// <param name="Identifier">The sink identifier.</param>
        /// <param name="Logger">The logger to push the sink to.</param>
        /// <param name="EnableColors">Should this sink print to the console using colors?</param>
        /// <param name="Verbosity">The sink verbosity.</param>
        #endregion
        public ConsoleSink(string Identifier, Log Logger, bool EnableColors = true, Severity Verbosity = PowerLog.Verbosity.All) {
            this.Identifier = Identifier;
            this.Logger = Logger;
            this.Verbosity = Verbosity;
            this.EnableColors = EnableColors;
        }
    }



    #region ConsoleSinkUtilities Class XML
    /// <summary>
    /// Contains extension methods.
    /// </summary>
    #endregion
    public static class ConsoleSinkUtilities
    {
        #region PushConsole Function XML
        /// <summary>
        /// Pushes a new console sink on the logger sink stack.
        /// </summary>
        /// <param name="Logger">The logger to push the sink to.</param>
        /// <param name="Identifier">The sink identifier.</param>
        /// <param name="EnableColors">Should this sink print to the console using colors?</param>
        /// <param name="Verbosity">The sink verbosity.</param>
        /// <returns>The current logger, to allow for builder patterns.</returns>
        #endregion
        public static Log PushConsole(this Log Logger, string Identifier, bool EnableColors = true, Severity Verbosity = Verbosity.All)
        {
            ConsoleSink Sink = new ConsoleSink(Identifier, Logger, EnableColors, Verbosity);
            Logger.Push(Sink);

            return Logger;
        }
    }
}