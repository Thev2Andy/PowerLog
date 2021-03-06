using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PowerLog
{
    #region Log Class XML
    /// <summary>
    /// The class used for writing logs.
    /// </summary>
    #endregion
    [Serializable] public class Log
    {
        // Public / Accessible variables.
        #region Identifier String XML
        /// <summary>
        /// Contains the identifier / name of the current logger Object.
        /// </summary>
        #endregion
        public string Identifier { get; private set; }

        #region WriteLogInFile Boolean XML
        /// <summary>
        /// Toggle writing logs to a file.
        /// </summary>
        #endregion
        public bool WriteInLogFile = true;

        #region LogSizeThreshold UInt64 XML
        /// <summary>
        /// Determines the log cache size in bytes.
        /// </summary>
        #endregion
        public UInt64 LogSizeThreshold = 15000000;


        // Public / Accessible Events.
        #region OnLog Event XML
        /// <summary>
        /// Invoked on <c>Log()</c> call.
        /// </summary>
        #endregion
        public event Action<LogArgs> OnLog;

        #region OnSave Event XML
        /// <summary>
        /// Invoked when the log is saved.
        /// </summary>
        #endregion
        public event Action OnSave;

        #region OnClear Event XML
        /// <summary>
        /// Invoked on <c>Clear()</c> call.
        /// </summary>
        #endregion
        public event Action OnClear;

        #region OnDelete Event XML
        /// <summary>
        /// Invoked when this logger is deleted / destroyed.
        /// </summary>
        #endregion
        public event Action<Object> OnDelete;



        // Private / Internal variables..
        #region LogPath LogIO XML
        /// <summary>
        /// Holds data regarding log file paths.
        /// </summary>
        #endregion
        public LogIO LogPath { get; internal set; }

        #region LogCache String XML
        /// <summary>
        /// Contains the log cache to be written to the log file.
        /// </summary>
        #endregion
        public string LogCache { get; internal set; }

        #region LastLog LogArgs XML
        /// <summary>
        /// Contains the last log call data.
        /// </summary>
        #endregion
        public LogArgs LastLog { get; internal set; }




        #region Write Function XML
        /// <summary>
        /// Calls a fully-customizable log, sent over to the <c>OnLog</c> event.
        /// </summary>
        /// <param name="LogContent">The content of the this.</param>
        /// <param name="LogLevel">The type / level of the this.</param>
        /// <param name="LoggingData">The log options.</param>
        /// <param name="LogParameters">The additional log parameters.</param>
        /// <param name="LogSender">The sender of the this.</param>
        #endregion
        public void Write(string LogContent, LogType LogLevel, LogData LoggingData, List<LogParameter> LogParameters = null, Object LogSender = null)
        {
            LogArgs LogEventParams = new LogArgs()
            {
                LogContent = LogContent,
                LogLevel = LogLevel,
                LogTime = DateTime.Now,
                LoggingData = LoggingData,
                LogSender = LogSender,
                LogStacktrace = new StackTrace(),
                LogParameters = LogParameters,

                Logger = this
            };

            if (LoggingData.LogOptions.HasFlag(LogOptions.Save))
            {
                LogCache = $"{((LogCache.Length > 0) ? LogCache : String.Empty)}" +
                    $"{((LogCache.Length > 0) ? $"{Environment.NewLine}" : String.Empty)}" +
                    $"{LogEventParams.FormattedLog}";

                if (this.CheckLogSize() >= LogSizeThreshold && (!LoggingData.LogOptions.HasFlag(LogOptions.NoSizeCheck))) {
                    this.Save(null);
                }
            }

            if (LoggingData.LogOptions.HasFlag(LogOptions.InvokeEvent)) OnLog?.Invoke(LogEventParams);
            LastLog = LogEventParams;
        }

        #region LogArguments Function XML
        /// <summary>
        /// Calls a log by passing a <c>LogArgs</c> Object.
        /// </summary>
        /// <param name="Arguments">The log arguments Object to this.</param>
        #endregion
        public void LogArguments(LogArgs Arguments) {
            this.Write(Arguments.LogContent, Arguments.LogLevel, Arguments.LoggingData, Arguments.LogParameters, Arguments.LogSender);
        }

        #region Log Overloads
        #region Verbose Function XML
        /// <summary>
        /// Calls a verbose log, sent over to the <c>OnLog</c> event.
        /// </summary>
        /// <param name="LogContent">The message of the log.</param>
        /// <param name="LoggingData">The log options.</param>
        /// <param name="LogParameters">The additional log parameters.</param>
        /// <param name="LogSender">The sender of the this.</param>
        #endregion
        public void Verbose(string LogContent, LogData LoggingData, List<LogParameter> LogParameters = null, Object LogSender = null) {
            this.Write(LogContent, LogType.Verbose, LoggingData, LogParameters, LogSender);
        }

        #region Trace Function XML
        /// <summary>
        /// Calls a trace log, sent over to the <c>OnLog</c> event.
        /// </summary>
        /// <param name="LogContent">The message of the log.</param>
        /// <param name="LoggingData">The log options.</param>
        /// <param name="LogParameters">The additional log parameters.</param>
        /// <param name="LogSender">The sender of the this.</param>
        #endregion
        public void Trace(string LogContent, LogData LoggingData, List<LogParameter> LogParameters = null, Object LogSender = null) {
            this.Write(LogContent, LogType.Trace, LoggingData, LogParameters, LogSender);
        }

        #region Debug Function XML
        /// <summary>
        /// Calls a debug log, sent over to the <c>OnLog</c> event.
        /// </summary>
        /// <param name="LogContent">The message of the log.</param>
        /// <param name="LoggingData">The log options.</param>
        /// <param name="LogParameters">The additional log parameters.</param>
        /// <param name="LogSender">The sender of the this.</param>
        #endregion
        public void Debug(string LogContent, LogData LoggingData, List<LogParameter> LogParameters = null, Object LogSender = null) {
            this.Write(LogContent, LogType.Debug, LoggingData, LogParameters, LogSender);
        }

        #region Info Function XML
        /// <summary>
        /// Calls an info log, sent over to the <c>OnLog</c> event.
        /// </summary>
        /// <param name="LogContent">The message of the log.</param>
        /// <param name="LoggingData">The log options.</param>
        /// <param name="LogParameters">The additional log parameters.</param>
        /// <param name="LogSender">The sender of the this.</param>
        #endregion
        public void Info(string LogContent, LogData LoggingData, List<LogParameter> LogParameters = null, Object LogSender = null) {
            this.Write(LogContent, LogType.Info, LoggingData, LogParameters, LogSender);
        }

        #region Warning Function XML
        /// <summary>
        /// Calls a warning log, sent over to the <c>OnLog</c> event.
        /// </summary>
        /// <param name="LogContent">The message of the log.</param>
        /// <param name="LoggingData">The log options.</param>
        /// <param name="LogParameters">The additional log parameters.</param>
        /// <param name="LogSender">The sender of the this.</param>
        #endregion
        public void Warning(string LogContent, LogData LoggingData, List<LogParameter> LogParameters = null, Object LogSender = null) {
            this.Write(LogContent, LogType.Warning, LoggingData, LogParameters, LogSender);
        }

        #region Error Function XML
        /// <summary>
        /// Calls an error log, sent over to the <c>OnLog</c> event.
        /// </summary>
        /// <param name="LogContent">The message of the log.</param>
        /// <param name="LoggingData">The log options.</param>
        /// <param name="LogParameters">The additional log parameters.</param>
        /// <param name="LogSender">The sender of the this.</param>
        #endregion
        public void Error(string LogContent, LogData LoggingData, List<LogParameter> LogParameters = null, Object LogSender = null) {
            this.Write(LogContent, LogType.Error, LoggingData, LogParameters, LogSender);
        }

        #region Network Function XML
        /// <summary>
        /// Calls a network log, sent over to the <c>OnLog</c> event.
        /// </summary>
        /// <param name="LogContent">The message of the log.</param>
        /// <param name="LoggingData">The log options.</param>
        /// <param name="LogParameters">The additional log parameters.</param>
        /// <param name="LogSender">The sender of the this.</param>
        #endregion
        public void Network(string LogContent, LogData LoggingData, List<LogParameter> LogParameters = null, Object LogSender = null) {
            this.Write(LogContent, LogType.Network, LoggingData, LogParameters, LogSender);
        }

        #region Fatal Function XML
        /// <summary>
        /// Calls a fatal log, sent over to the <c>OnLog</c> event.
        /// </summary>
        /// <param name="LogContent">The message of the log.</param>
        /// <param name="LoggingData">The log options.</param>
        /// <param name="LogParameters">The additional log parameters.</param>
        /// <param name="LogSender">The sender of the this.</param>
        #endregion
        public void Fatal(string LogContent, LogData LoggingData, List<LogParameter> LogParameters = null, Object LogSender = null) {
            this.Write(LogContent, LogType.Fatal, LoggingData, LogParameters, LogSender);
        }

        #region NA Function XML
        /// <summary>
        /// Calls a NA (no-header) log, sent over to the <c>OnLog</c> event.
        /// </summary>
        /// <param name="LogContent">The message of the log.</param>
        /// <param name="LoggingData">The log options.</param>
        /// <param name="LogParameters">The additional log parameters.</param>
        /// <param name="LogSender">The sender of the this.</param>
        #endregion
        public void NA(string LogContent, LogData LoggingData, List<LogParameter> LogParameters = null, Object LogSender = null) {
            this.Write(LogContent, LogType.NA, LoggingData, LogParameters, LogSender);
        }
        #endregion


        #region Save Function XML
        /// <summary>
        /// Saves the log in a file.
        /// </summary>
        #endregion
        public void Save(Object Sender = null)
        {
            if (WriteInLogFile)
            {
                string LogFilePath = LogPath.Get();

                if (!Directory.Exists(LogPath.LogPath))
                {
                    Directory.CreateDirectory(LogPath.LogPath);
                }

                File.AppendAllText(LogFilePath, $"{LogCache}{((LogCache.EndsWith(Environment.NewLine) || LogCache.Length <= 0) ? String.Empty : Environment.NewLine)}");
                LogCache = String.Empty;
                OnSave?.Invoke();
            }

            else {
                this.Error("Log saving is currently disabled.", LogData.Default, null, this);
            }
        }

        #region Clear Function XML
        /// <summary>
        /// Invokes the <c>OnClear</c> event.
        /// </summary>
        #endregion
        public void Clear(Object Sender = null) {
            OnClear?.Invoke();
        }



        #region SwapIO Method XML
        /// <summary>
        /// Sets a new LogIO Object as log path data.
        /// </summary>
        /// <param name="NewIO">The 'LogIO' Object to swap to.</param>
        /// <param name="SwapMode">The 'LogIO' swap mode.</param>
        #endregion
        public void SwapIO(LogIO NewIO, IOSwapMode SwapMode)
        {
            if (LogPath != null)
            {
                if (!SwapMode.HasFlag(IOSwapMode.None))
                {
                    if (SwapMode.HasFlag(IOSwapMode.Override))
                    {
                        if (SwapMode.HasFlag(IOSwapMode.Migrate))
                        {
                            if (File.Exists(NewIO.Get())) File.Delete(NewIO.Get());

                            if (File.Exists(LogPath.Get())) {
                                File.Copy(LogPath.Get(), NewIO.Get());
                            }
                        }

                        else {
                            if (File.Exists(NewIO.Get())) File.Delete(NewIO.Get());
                        }
                    }

                    else
                    {
                        if (SwapMode.HasFlag(IOSwapMode.Migrate))
                        {
                            string OldLogContents = (File.Exists(LogPath.Get()) ? File.ReadAllText(LogPath.Get()) : String.Empty);
                            File.AppendAllText(NewIO.Get(),
                                $"{OldLogContents}{((OldLogContents.EndsWith(Environment.NewLine)) ? String.Empty : Environment.NewLine)}");
                        }
                    }
                }

                if (!SwapMode.HasFlag(IOSwapMode.Keep) || SwapMode.HasFlag(IOSwapMode.None)) File.Delete(LogPath.Get());
            }

            else {
                if (File.Exists(NewIO.Get()) && SwapMode.HasFlag(IOSwapMode.Override) && !SwapMode.HasFlag(IOSwapMode.None)) File.Delete(NewIO.Get());
            }

            LogPath = NewIO;
        }

        #region CheckLogSize Method XML
        /// <summary>
        /// Gets the log size in memory.
        /// </summary>
        /// <returns>The log size in memory, in bytes.</returns>
        #endregion
        public UInt64 CheckLogSize() {
            return (UInt64)(sizeof(char) * ((LogCache != null) ? LogCache.Length : 0f));
        }


        #region Log Constructor XML
        /// <summary>
        /// The default <c>Log</c> constructor.
        /// </summary>
        /// <param name="Identifier">The identifier / name of this logger.</param>
        #endregion
        public Log(string Identifier) {
            this.Identifier = Identifier;

            AppDomain.CurrentDomain.UnhandledException += EventSaveLog;
            AppDomain.CurrentDomain.ProcessExit += EventSaveLog;


            if (LogPath == null) SwapIO(new LogIO(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Logs", $"{((Identifier.IndexOfAny(Path.GetInvalidPathChars()) == -1) ? Identifier : "Miscellaneous")}"),
                 $"{((Identifier.IndexOfAny(Path.GetInvalidFileNameChars()) == -1) ? Identifier : "Log")} Output - ({DateTime.Now.ToString("HH-mm-ss tt, dd MMMM yyyy")})", "txt"),
                 (IOSwapMode.Keep));


            LogCache = String.Empty;
        }

        #region Log Finalizer XML
        /// <summary>
        /// The default <c>Log</c> finalizer.
        /// </summary>
        #endregion
        ~Log() {
            AppDomain.CurrentDomain.UnhandledException -= this.EventSaveLog;
            AppDomain.CurrentDomain.ProcessExit -= this.EventSaveLog;

            OnDelete?.Invoke(this);
            this.Save(this);
        }


        private void EventSaveLog(Object S, EventArgs E) {
            this.Save(S);
        }







        private void LogException(Object Sender, UnhandledExceptionEventArgs ExceptionArgs)
        {
            if (true /*LogExceptions*/)
            {
                this.NA(String.Empty, LogData.Default, null, this); // Add a new line in the log file.
                this.Write($"Unhandled exception!{((ExceptionArgs.IsTerminating) ? " The application will now terminate." : String.Empty)}", ((ExceptionArgs.IsTerminating) ? LogType.Fatal : LogType.Error), LogData.Default, null, ExceptionArgs.ExceptionObject);
                this.NA($"{((Exception)ExceptionArgs.ExceptionObject).GetType()}: {((Exception)ExceptionArgs.ExceptionObject).Message}{Environment.NewLine}{((Exception)ExceptionArgs.ExceptionObject).StackTrace}", LogData.Default);
            }
        }
    }
}
