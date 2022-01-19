using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    /// <summary>
    /// The class used for writing logs.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Invoked on <c>Log()</c> call.
        /// </summary>
        public static event EventHandler<LogEventArgs> OnLog;

        /// <summary>
        /// Toggle writing logs to a file, stored next to the caller assembly.
        /// </summary>
        public static bool WriteInLogFile = true;



        // Private variables.
        private static bool Initialized;


        /// <summary>
        /// Calls a log, sent over to the <cref>OnLog</cref> event.
        /// </summary>
        /// <param name="LogMessage">The message of the log.</param>
        /// <param name="MessageType">The type of the log.</param>
        /// <param name="Timestamp">Timestamp this log in the file?</param>>
        /// <param name="LogSender">The sender of the log.</param>
        public static void Log(string LogMessage, LogType MessageType, bool Timestamp, object LogSender)
        {
            LogEventArgs LogEventParams = new LogEventArgs();
            LogEventParams.LogMessage = LogMessage;
            LogEventParams.MessageType = MessageType;
            LogEventParams.LogTime = DateTime.Now;
            LogEventParams.Timestamped = Timestamp;

            string LogFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), "PowerLog Output.txt");
            string LogFileContent = ((WriteInLogFile && File.Exists(LogFilePath) && Initialized) ? File.ReadAllText(LogFilePath) : "");
            if (WriteInLogFile) {
                File.WriteAllText(LogFilePath,
                    $"{((LogFileContent.Length > 0) ? LogFileContent : "")}" +
                    $"{((!LogFileContent.EndsWith(Environment.NewLine) && LogFileContent.Length > 0) ? $"{Environment.NewLine}" : "")}" +
                    $"{((Timestamp) ? $"[{DateTime.Now.ToString("HH:mm:ss")}] " : "")}" +
                    $"{((MessageType != LogType.Null) ? $"{MessageType.ToString()}: " : "")}" +
                    $"{LogMessage}");
            }

            Initialized = true;

            OnLog?.Invoke(LogSender, LogEventParams);
        }
    }
}
