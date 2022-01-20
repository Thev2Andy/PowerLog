using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    #region Logger Class XML
    /// <summary>
    /// The class used for writing logs.
    /// </summary>
    #endregion
    public static class Logger
    {
        #region OnLog Event XML
        /// <summary>
        /// Invoked on <c>Log()</c> call.
        /// </summary>
        #endregion
        public static event EventHandler<LogEventArgs> OnLog;

        #region OnClear Event XML
        /// <summary>
        /// Invoked on <c>ClearLog()</c> call.
        /// </summary>
        #endregion
        public static event EventHandler OnClear;


        #region Log Function XML
        /// <summary>
        /// Calls a log, sent over to the <c>OnLog</c> event.
        /// </summary>
        /// <param name="LogMessage">The message of the log.</param>
        /// <param name="MessageType">The type of the log.</param>
        /// <param name="SaveLogToFile">Save this specific log to the log file?</param>
        /// <param name="Timestamp">Timestamp this log in the file?</param>>
        /// <param name="InvokeLogEvent">Invoke the <c>OnLog</c> event?</param>
        /// <param name="LogSender">The sender of the log.</param>
        #endregion
        public static void Log(string LogMessage = "", LogType MessageType = LogType.Info, bool Timestamp = true, bool SaveLogToFile = true, bool InvokeLogEvent = true, object LogSender = null)
        {
            if (!LogSession.LoggingStripped)
            {
                if (LogSession.Initialized)
                {
                    LogEventArgs LogEventParams = new LogEventArgs
                    {
                        LogMessage = LogMessage,
                        MessageType = MessageType,
                        LogTime = DateTime.Now,
                        Timestamped = Timestamp
                    };

                    if (SaveLogToFile)
                    {
                        LogSession.LogCache = $"{((LogSession.LogCache.Length > 0) ? LogSession.LogCache : "")}" +
                            $"{((!LogSession.LogCache.EndsWith(Environment.NewLine) && LogSession.LogCache.Length > 0) ? $"{Environment.NewLine}" : "")}" +
                            $"{((Timestamp) ? $"[{DateTime.Now.ToString("HH:mm:ss")}] " : "")}" +
                            $"{((MessageType != LogType.Null) ? $"{MessageType.ToString()}: " : "")}" +
                            $"{LogMessage}";
                    }

                    if (InvokeLogEvent) OnLog?.Invoke(LogSender, LogEventParams);
                }
                else
                {
                    throw new InvalidOperationException("Logger not initialized. Did you forget to initialize the Logger?");
                }
            }
        }

        #region SaveLogs Function XML
        /// <summary>
        /// Saves the entire log in a file.
        /// </summary>
        #endregion
        public static void SaveLog(object Sender = null, EventArgs Args = null)
        {
            if (!LogSession.LoggingStripped)
            {
                if (LogSession.Initialized)
                {
                    if (LogSession.WriteInLogFile)
                    {
                        string LogFilePath = Path.Combine(Path.GetDirectoryName(LogSession.LogPath), "PowerLog Output.txt");
                        string LogFileContent = ((File.Exists(LogFilePath)) ? File.ReadAllText(LogFilePath) : "");
                        
                        string LogOutput = $"{((LogFileContent.Length > 0) ? LogFileContent : "")}" +
                            $"{((!LogFileContent.EndsWith(Environment.NewLine) && LogFileContent.Length > 0) ? $"{Environment.NewLine}" : "")}" +
                            $"{LogSession.LogCache}";


                        File.WriteAllText(LogFilePath, LogOutput);
                    }
                    else {
                        Log("Log saving is currently disabled.", LogType.Error, true, true, true, null);
                    }
                }
                else {
                    throw new InvalidOperationException("Logger not initialized. Did you forget to initialize the Logger?");
                }
            }
        }

        #region ClearLog Function XML
        /// <summary>
        /// Clears the log session-wide.
        /// </summary>
        /// <param name="DeleteFile">Delete the file aswell?</param>
        /// <param name="InvokeClearEvent">Invoke the <c>OnClear</c> event?</param>
        #endregion
        public static void ClearLog(bool DeleteFile = false, bool InvokeClearEvent = true)
        {
            try {
                if (DeleteFile) {
                    string LogFilePath = Path.Combine(Path.GetDirectoryName(LogSession.LogPath), "PowerLog Output.txt");
                    File.Delete(LogFilePath);
                }
            }catch (FileNotFoundException) {
                Log("No log file found.");
            }catch (Exception Ex) {
                Log($"{Ex.GetType()}: {Ex.Message}", LogType.Null);
            }

            if(InvokeClearEvent) OnClear?.Invoke(null, EventArgs.Empty);
            LogSession.LogCache = "";
        }


        internal static void LogException(object Sender, UnhandledExceptionEventArgs ExceptionArgs) 
        {
            if (!LogSession.LoggingStripped)
            {
                if (LogSession.LogExceptions) {
                    Log($"{((Exception)ExceptionArgs.ExceptionObject).GetType()}: {((Exception)ExceptionArgs.ExceptionObject).Message}{Environment.NewLine}{((Exception)ExceptionArgs.ExceptionObject).StackTrace}", LogType.Null);
                }
            }
        }
    }
}
