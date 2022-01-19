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

        /// <summary>
        /// Enable auto-logging of unhandled exceptions?
        /// </summary>
        public static bool LogExceptions = true;


        // Private variables.
        public static string EntireLog { get; private set; }
        public static bool Initialized { get; private set; }


        /// <summary>
        /// Calls a log, sent over to the <c>OnLog</c> event.
        /// </summary>
        /// <param name="LogMessage">The message of the log.</param>
        /// <param name="MessageType">The type of the log.</param>
        /// <param name="SaveLogToFile">Save this specific log to the log file?</param>
        /// <param name="Timestamp">Timestamp this log in the file?</param>>
        /// <param name="TriggerLogEvent">Trigger the <c>OnLog</c> event?</param>
        /// <param name="LogSender">The sender of the log.</param>
        public static void Log(string LogMessage, LogType MessageType, bool SaveLogToFile, bool Timestamp, bool TriggerLogEvent, object LogSender)
        {
            if (Initialized) {
                LogEventArgs LogEventParams = new LogEventArgs();
                LogEventParams.LogMessage = LogMessage;
                LogEventParams.MessageType = MessageType;
                LogEventParams.LogTime = DateTime.Now;
                LogEventParams.Timestamped = Timestamp;

                if (SaveLogToFile)
                {
                    EntireLog = $"{((EntireLog.Length > 0) ? EntireLog : "")}" +
                        $"{((!EntireLog.EndsWith(Environment.NewLine) && EntireLog.Length > 0) ? $"{Environment.NewLine}" : "")}" +
                        $"{((Timestamp) ? $"[{DateTime.Now.ToString("HH:mm:ss")}] " : "")}" +
                        $"{((MessageType != LogType.Null) ? $"{MessageType.ToString()}: " : "")}" +
                        $"{LogMessage}";
                }

                if(TriggerLogEvent) OnLog?.Invoke(LogSender, LogEventParams);
            }else {
                throw new InvalidOperationException("Logger not initialized. Did you forget to initialize the Logger?");
            }
        }

        /// <summary>
        /// Saves the entire log in a file.
        /// </summary>
        public static void SaveLogToFile(object Sender, EventArgs Args) 
        {
            if (Initialized) {
                if (WriteInLogFile)
                {
                    string LogFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "PowerLog Output.txt");
                    File.WriteAllText(LogFilePath, EntireLog);
                }
                else
                {
                    Log("Log saving is currently disabled.", LogType.Error, true, true, true, null);
                }
            }else {
                throw new InvalidOperationException("Logger not initialized. Did you forget to initialize the Logger?");
            }
        }

        public static void LogException(object Sender, UnhandledExceptionEventArgs ExceptionArgs) 
        {
            if (LogExceptions) {
                Exception Ex = (Exception)ExceptionArgs.ExceptionObject;

                Log($"{Ex.GetType()}: {Ex.Message}{Environment.NewLine}{Ex.StackTrace}", LogType.Null, true, true, true, null);
                WriteInLogFile = true;
            }
        }

        /// <summary>
        /// Initializes the logger.
        /// </summary>
        public static void Initialize()
        {
            if(!Initialized) {
                AppDomain.CurrentDomain.ProcessExit += SaveLogToFile;

                AppDomain.CurrentDomain.UnhandledException += LogException;
                AppDomain.CurrentDomain.UnhandledException += SaveLogToFile;

                Initialized = true;
                EntireLog = "";
            }else {
                throw new InvalidOperationException("Logger already initialized.");
            }
        }
    }
}
