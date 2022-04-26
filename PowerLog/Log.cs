﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    #region Log Class XML
    /// <summary>
    /// The class used for writing logs.
    /// </summary>
    #endregion
    public static class Log
    {
        // Public / Accessible variables.
        // Events.
        #region OnLog Event XML
        /// <summary>
        /// Invoked on <c>Log()</c> call.
        /// </summary>
        #endregion
        public static event EventHandler<LogArgs> OnLog;

        #region OnSave Event XML
        /// <summary>
        /// Invoked when the log is saved.
        /// </summary>
        #endregion
        public static event EventHandler OnSave;

        #region OnClear Event XML
        /// <summary>
        /// Invoked on <c>ClearLog()</c> call.
        /// </summary>
        #endregion
        public static event EventHandler OnClear;


        #region Write Function XML
        /// <summary>
        /// Calls a fully-customizable log, sent over to the <c>OnLog</c> event.
        /// </summary>
        /// <param name="LogContent">The content of the log.</param>
        /// <param name="LogLevel">The type / level of the log.</param>
        /// <param name="LoggingMode">The log options.</param>
        /// <param name="LogSender">The sender of the log.</param>
        #endregion
        public static void Write(string LogContent, LogType LogLevel, LogMode LoggingMode = LogMode.Default, object LogSender = null)
        {
            if (!LogSession.LoggingStripped)
            {
                if (LogSession.Initialized)
                {
                    LogArgs LogEventParams = new LogArgs()
                    {
                        FormattedLog = LogFormat.Format(LogContent, LogLevel, LoggingMode.HasFlag(LogMode.Timestamp), LogSender),

                        LogContent = LogContent,
                        LogLevel = LogLevel,
                        LogTime = DateTime.Now,
                        LoggingMode = LoggingMode,
                        LogSender = LogSender
                    };

                    if (LoggingMode.HasFlag(LogMode.Save))
                    {
                        LogSession.LogCache = $"{((LogSession.LogCache.Length > 0) ? LogSession.LogCache : String.Empty)}" +
                            $"{((!LogSession.LogCache.EndsWith(Environment.NewLine) && LogSession.LogCache.Length > 0) ? $"{Environment.NewLine}" : String.Empty)}" +
                            $"{LogEventParams.FormattedLog}";

                        if (LogSession.CheckSize() >= LogSession.LogSizeThreshold && (!LoggingMode.HasFlag(LogMode.NoSizeCheck))) {
                            Save(null, EventArgs.Empty);
                        }
                    }

                    if (LoggingMode.HasFlag(LogMode.InvokeEvent)) OnLog?.Invoke(LogSender, LogEventParams);
                    LogSession.LastLog = LogEventParams;
                }else {
                    throw new InvalidOperationException("LogSession not initialized. Did you forget to initialize the LogSession?");
                }
            }
        }

        #region LogArguments Function XML
        /// <summary>
        /// Calls a log by passing a <c>LogArgs</c> object.
        /// </summary>
        /// <param name="Arguments">The log arguments object to log.</param>
        #endregion
        public static void LogArguments(LogArgs Arguments) {
            Log.Write(Arguments.LogContent, Arguments.LogLevel, Arguments.LoggingMode, Arguments.LogSender);
        }

        #region Log Overloads
        #region Trace Function XML
        /// <summary>
        /// Calls a trace log, sent over to the <c>OnLog</c> event.
        /// </summary>
        /// <param name="LogContent">The message of the log.</param>
        /// <param name="LoggingMode">The log options.</param>
        /// <param name="LogSender">The sender of the log.</param>
        #endregion
        public static void Trace(string LogContent, LogMode LoggingMode = LogMode.Default, object LogSender = null) {
            Log.Write(LogContent, LogType.Trace, LoggingMode, LogSender);
        }

        #region Debug Function XML
        /// <summary>
        /// Calls a debug log, sent over to the <c>OnLog</c> event.
        /// </summary>
        /// <param name="LogContent">The message of the log.</param>
        /// <param name="LoggingMode">The log options.</param>
        /// <param name="LogSender">The sender of the log.</param>
        #endregion
        public static void Debug(string LogContent, LogMode LoggingMode = LogMode.Default, object LogSender = null) {
            Log.Write(LogContent, LogType.Debug, LoggingMode, LogSender);
        }

        #region Info Function XML
        /// <summary>
        /// Calls an info log, sent over to the <c>OnLog</c> event.
        /// </summary>
        /// <param name="LogContent">The message of the log.</param>
        /// <param name="LoggingMode">The log options.</param>
        /// <param name="LogSender">The sender of the log.</param>
        #endregion
        public static void Info(string LogContent, LogMode LoggingMode = LogMode.Default, object LogSender = null) {
            Log.Write(LogContent, LogType.Info, LoggingMode, LogSender);
        }

        #region Warning Function XML
        /// <summary>
        /// Calls a warning log, sent over to the <c>OnLog</c> event.
        /// </summary>
        /// <param name="LogContent">The message of the log.</param>
        /// <param name="LoggingMode">The log options.</param>
        /// <param name="LogSender">The sender of the log.</param>
        #endregion
        public static void Warning(string LogContent, LogMode LoggingMode = LogMode.Default, object LogSender = null) {
            Log.Write(LogContent, LogType.Warning, LoggingMode, LogSender);
        }

        #region Error Function XML
        /// <summary>
        /// Calls an error log, sent over to the <c>OnLog</c> event.
        /// </summary>
        /// <param name="LogContent">The message of the log.</param>
        /// <param name="LoggingMode">The log options.</param>
        /// <param name="LogSender">The sender of the log.</param>
        #endregion
        public static void Error(string LogContent, LogMode LoggingMode = LogMode.Default, object LogSender = null) {
            Log.Write(LogContent, LogType.Error, LoggingMode, LogSender);
        }

        #region Network Function XML
        /// <summary>
        /// Calls a network log, sent over to the <c>OnLog</c> event.
        /// </summary>
        /// <param name="LogContent">The message of the log.</param>
        /// <param name="LoggingMode">The log options.</param>
        /// <param name="LogSender">The sender of the log.</param>
        #endregion
        public static void Network(string LogContent, LogMode LoggingMode = LogMode.Default, object LogSender = null) {
            Log.Write(LogContent, LogType.Network, LoggingMode, LogSender);
        }

        #region Fatal Function XML
        /// <summary>
        /// Calls a fatal log, sent over to the <c>OnLog</c> event.
        /// </summary>
        /// <param name="LogContent">The message of the log.</param>
        /// <param name="LoggingMode">The log options.</param>
        /// <param name="LogSender">The sender of the log.</param>
        #endregion
        public static void Fatal(string LogContent, LogMode LoggingMode = LogMode.Default, object LogSender = null) {
            Log.Write(LogContent, LogType.Fatal, LoggingMode, LogSender);
        }

        #region Null Function XML
        /// <summary>
        /// Calls a null (no-header) log, sent over to the <c>OnLog</c> event.
        /// </summary>
        /// <param name="LogContent">The message of the log.</param>
        /// <param name="LoggingMode">The log options.</param>
        /// <param name="LogSender">The sender of the log.</param>
        #endregion
        public static void Null(string LogContent, LogMode LoggingMode = LogMode.Default, object LogSender = null) {
            Log.Write(LogContent, LogType.Null, LoggingMode, LogSender);
        }
        #endregion


        #region Save Function XML
        /// <summary>
        /// Saves the log in a file.
        /// </summary>
        #endregion
        public static void Save(object Sender = null, EventArgs Args = null)
        {
            if (!LogSession.LoggingStripped)
            {
                if (LogSession.Initialized)
                {
                    if (LogSession.WriteInLogFile)
                    {
                        string LogFilePath = LogSession.LogPath.GetLogPath();

                        if (!Directory.Exists(LogSession.LogPath.LogPath)) {
                            Directory.CreateDirectory(LogSession.LogPath.LogPath);
                        }

                        File.AppendAllText(LogFilePath, $"{LogSession.LogCache}{((LogSession.LogCache.EndsWith(Environment.NewLine) || LogSession.LogCache.Length <= 0) ? String.Empty : Environment.NewLine)}");
                        LogSession.LogCache = String.Empty;
                        OnSave?.Invoke(null, EventArgs.Empty);
                    }
                    else {
                        Log.Write("Log saving is currently disabled.", LogType.Error, LogMode.Default, null);
                    }
                }else {
                    throw new InvalidOperationException("LogSession not initialized. Did you forget to initialize the LogSession?");
                }
            }
        }

        #region Clear Function XML
        /// <summary>
        /// Invokes the <c>OnClear</c> event.
        /// </summary>
        #endregion
        public static void Clear(object Sender = null) {
            if (LogSession.Initialized) {
                OnClear?.Invoke(Sender, EventArgs.Empty);
            }else {
                throw new InvalidOperationException("LogSession not initialized. Did you forget to initialize the LogSession?");
            }
            
        }


        internal static void LogException(object Sender, UnhandledExceptionEventArgs ExceptionArgs) 
        {
            if (!LogSession.LoggingStripped)
            {
                if (LogSession.LogExceptions) {
                    Log.Write(String.Empty, LogType.Null, (LogMode.Save | LogMode.InvokeEvent), null); // Add a new line in the log file.
                    Log.Write($"Unhandled exception!{((ExceptionArgs.IsTerminating) ? " The application will now terminate." : String.Empty)}", ((ExceptionArgs.IsTerminating) ? LogType.Fatal : LogType.Error), LogMode.Default, ExceptionArgs.ExceptionObject);
                    Log.Write($"{((Exception)ExceptionArgs.ExceptionObject).GetType()}: {((Exception)ExceptionArgs.ExceptionObject).Message}{Environment.NewLine}{((Exception)ExceptionArgs.ExceptionObject).StackTrace}", LogType.Null);
                }
            }
        }
    }
}