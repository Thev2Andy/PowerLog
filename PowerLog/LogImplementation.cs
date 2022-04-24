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
        private static void OnClearConsole(object Sender, EventArgs E) {
            Console.Clear();
        }

        private static void OnLogConsole(object Sender, LogArgs LogArgs)
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
            Console.WriteLine(LogArgs.FormattedLog);

            Console.ForegroundColor = BackupColor;
        }
    }
}
