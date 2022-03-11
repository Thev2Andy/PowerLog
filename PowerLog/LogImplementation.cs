using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    #region LogImplementation Class XML
    /// <summary>
    /// A class containing a base implementation for quickly getting started.
    /// </summary>
    #endregion
    public static class LogImplementation
    {
        public static void InitializeSession(bool UseDefaultLogFunctions = true)
        {
            // Most of the time, default launch parameters are fine.
            LogSession.Initialize(true);

            // Set the log size threshold to 5 MB.
            LogSession.LogSizeThreshold = 5000000;

            // Subscribe to events using default functions.
            if (UseDefaultLogFunctions)
            {
                Log.OnLog += OnLog;
                Log.OnClear += OnClear;
            }
        }

        private static void OnClear(object Sender, EventArgs E) {
            Console.Clear();
        }

        private static void OnLog(object Sender, LogArgs LogArgs)
        {
            ConsoleColor BackupColor = Console.ForegroundColor;
            ConsoleColor TargetColor;

            switch (LogArgs.LogLevel)
            {
                case LogType.Trace:
                    TargetColor = ConsoleColor.Gray;
                    break;

                case LogType.Debug:
                    TargetColor = ConsoleColor.DarkGray;
                    break;

                case LogType.Info:
                    TargetColor = ConsoleColor.White;
                    break;

                case LogType.Warning:
                    TargetColor = ConsoleColor.Yellow;
                    break;

                case LogType.Error:
                    TargetColor = ConsoleColor.Red;
                    break;

                case LogType.Network:
                    TargetColor = ConsoleColor.Blue;
                    break;

                case LogType.Fatal:
                    TargetColor = ConsoleColor.DarkRed;
                    break;

                case LogType.Null:
                    TargetColor = BackupColor;
                    break;


                default:
                    TargetColor = BackupColor;
                    break;
            }

            Console.ForegroundColor = TargetColor;
            Console.WriteLine($"{((LogArgs.LoggingMode.HasFlag(LogMode.Timestamp)) ? $"[{DateTime.Now.ToString("HH:mm:ss")}] " : String.Empty)}" +
                $"{((LogArgs.LogLevel != LogType.Null) ? $"{LogArgs.LogLevel.ToString()}: " : String.Empty)}" +
                $"{LogArgs.LogMessage}");

            Console.ForegroundColor = BackupColor;
        }
    }
}
