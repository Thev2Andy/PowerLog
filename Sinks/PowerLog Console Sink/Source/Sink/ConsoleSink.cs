using System.IO;
using System;
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


        #region Emit Function XML
        /// <inheritdoc/>
        #endregion
        public void Emit(Arguments Log)
        {
            if (Log.Severity >= Verbosity)
            {
                ConsoleColor BackupColor = Console.ForegroundColor;
                ConsoleColor TargetColor = Console.ForegroundColor;

                if (EnableColors)
                {
                    // Compares the internal severity values, then matches it to a console color.
                    // Before you ask, I don't think you can do this with a 'switch' statement.
                    if (Log.Severity >= Severity.Verbose && Log.Severity < Severity.Trace) TargetColor = ConsoleColor.DarkGray; // Verbose.
                    if (Log.Severity >= Severity.Trace && Log.Severity < Severity.Debug) TargetColor = ConsoleColor.Gray; // Trace.
                    if (Log.Severity >= Severity.Debug && Log.Severity < Severity.Information) TargetColor = ConsoleColor.Gray; // Debug.
                    if (Log.Severity >= Severity.Information && Log.Severity < Severity.Network) TargetColor = ConsoleColor.White; // Info.
                    if (Log.Severity >= Severity.Network && Log.Severity < Severity.Notice) TargetColor = ConsoleColor.Blue; // Network.
                    if (Log.Severity >= Severity.Notice && Log.Severity < Severity.Caution) TargetColor = ConsoleColor.White; // Notice.
                    if (Log.Severity >= Severity.Caution && Log.Severity < Severity.Warning) TargetColor = ConsoleColor.Yellow; // Caution.
                    if (Log.Severity >= Severity.Warning && Log.Severity < Severity.Alert) TargetColor = ConsoleColor.Yellow; // Warning.
                    if (Log.Severity >= Severity.Alert && Log.Severity < Severity.Error) TargetColor = ConsoleColor.DarkYellow; // Alert.
                    if (Log.Severity >= Severity.Error && Log.Severity < Severity.Critical) TargetColor = ConsoleColor.Red; // Error.
                    if (Log.Severity >= Severity.Critical && Log.Severity < Severity.Emergency) TargetColor = ConsoleColor.Red; // Critical.
                    if (Log.Severity >= Severity.Emergency && Log.Severity < Severity.Fatal) TargetColor = ConsoleColor.DarkRed; // Emergency.
                    if (Log.Severity >= Severity.Fatal && Log.Severity < Severity.NA) TargetColor = ConsoleColor.DarkRed; // Fatal.


                    if (Log.Severity == Severity.NA) TargetColor = BackupColor; // N/A.

                    /*switch (Log.Severity)
                    {
                        case Severity.Verbose:
                            TargetColor = ConsoleColor.DarkGray;
                            break;

                        case Severity.Trace:
                            TargetColor = ConsoleColor.Gray;
                            break;

                        case Severity.Debug:
                            TargetColor = ConsoleColor.Gray;
                            break;

                        case Severity.Info:
                            TargetColor = ConsoleColor.White;
                            break;

                        case Severity.Network:
                            TargetColor = ConsoleColor.Blue;
                            break;

                        case Severity.Notice:
                            TargetColor = ConsoleColor.White;
                            break;

                        case Severity.Warning:
                            TargetColor = ConsoleColor.Yellow;
                            break;

                        case Severity.Alert:
                            TargetColor = ConsoleColor.DarkYellow;
                            break;

                        case Severity.Error:
                            TargetColor = ConsoleColor.Red;
                            break;

                        case Severity.Critical:
                            TargetColor = ConsoleColor.Red;
                            break;

                        case Severity.Emergency:
                            TargetColor = ConsoleColor.DarkRed;
                            break;

                        case Severity.Fatal:
                            TargetColor = ConsoleColor.DarkRed;
                            break;

                        case Severity.NA:
                            TargetColor = BackupColor;
                            break;


                        default:
                            TargetColor = BackupColor;
                            break;
                    }*/
                }

                Console.ForegroundColor = TargetColor;
                Console.WriteLine(Log.FormattedLog);

                Console.ForegroundColor = BackupColor;
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
        /// <param name="EnableColors">Should this sink print to console using colors?</param>
        /// <param name="Verbosity">The sink verbosity.</param>
        #endregion
        public ConsoleSink(string Identifier, Log Logger, bool EnableColors = true, Severity Verbosity = Severity.Verbose) {
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
        /// <param name="EnableColors">Should this sink print to console using colors?</param>
        /// <param name="Verbosity">The sink verbosity.</param>
        /// <returns>The current logger, to allow for builder patterns.</returns>
        #endregion
        public static Log PushConsole(this Log Logger, string Identifier, bool EnableColors = true, Severity Verbosity = Severity.Verbose)
        {
            ConsoleSink Sink = new ConsoleSink(Identifier, Logger, EnableColors, Verbosity);
            Logger.Push(Sink);

            return Logger;
        }
    }
}