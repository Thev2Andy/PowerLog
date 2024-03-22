using System;
using System.Collections.Generic;
using PowerLog;

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

        #region EnableColors Boolean XML
        /// <summary>
        /// Determines if console logs will be colored.
        /// </summary>
        #endregion
        public bool EnableColors { get; set; }


        // Private / Hidden variables..
        private Severity[] SeverityLevels;
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
            ConsoleColor OldForeground = Console.ForegroundColor;
            ConsoleColor OldBackground = Console.BackgroundColor;

            ConsoleColor TargetForegroundColor = OldForeground;
            ConsoleColor TargetBackgroundColor = OldBackground;

            if (EnableColors)
            {
                // Compares the internal severity values, then matches it to a console color.
                // Before you ask, I don't think you can do this with a 'switch' statement.
                // goofy job security type ahh code type
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



                bool InvertBackgroundColor = false;
                for (int I = 0; I < SeverityLevels.Length; I++)
                {
                    if (Log.Severity.HasFlag(SeverityLevels[I]) && HighlightedSeverities.Contains(SeverityLevels[I]))
                    {
                        InvertBackgroundColor = true;
                        break;
                    }
                }

                if (Log.Parameters != null)
                {
                    foreach (KeyValuePair<string, Object> Parameter in Log.Parameters)
                    {
                        if (Parameter.Key == "Highlight Override")
                        {
                            try {
                                InvertBackgroundColor = Convert.ToBoolean(Parameter.Value);
                            }

                            catch (Exception) {
                                // don't care didn't ask
                            }
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

            Console.WriteLine(Log.ComposedLog);

            Console.ForegroundColor = OldForeground;
            Console.BackgroundColor = OldBackground;
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
        /// <param name="EnableColors">Determines if console logs will be colored.</param>
        /// <param name="AllowedSeverities">The sink's allowed severity levels.</param>
        #endregion
        public ConsoleSink(string Identifier, Log Logger, bool EnableColors = true, Severity AllowedSeverities = Verbosity.All)
        {
            this.Identifier = Identifier;
            this.Logger = Logger;
            this.AllowedSeverities = AllowedSeverities;
            this.EnableColors = EnableColors;
            this.StrictFiltering = true;
            this.IsEnabled = true;

            SeverityLevels = Enum.GetValues<Severity>();
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
        /// <param name="EnableColors">Determines if console logs will be colored.</param>
        /// <param name="AllowedSeverities">The sink's allowed severity levels.</param>
        /// <returns>The current logger, to allow for method chaining.</returns>
        #endregion
        public static Log PushConsole(this Log Logger, string Identifier, bool EnableColors = true, Severity AllowedSeverities = Verbosity.All)
        {
            ConsoleSink Sink = new ConsoleSink(Identifier, Logger, EnableColors, AllowedSeverities);
            Logger.Push(Sink);

            return Logger;
        }
    }
}