using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    #region LogSession Class XML
    /// <summary>
    /// Holds information regarding the current logging session.
    /// </summary>
    #endregion
    public static class LogSession
    {
        #region WriteLogInFile Boolean XML
        /// <summary>
        /// Toggle writing logs to a file.
        /// </summary>
        #endregion
        public static bool WriteInLogFile = true;

        #region LogExceptions Boolean XML
        /// <summary>
        /// Enable auto-logging of unhandled exceptions?
        /// </summary>
        #endregion
        public static bool LogExceptions = true;

        #region LogSizeThreshold UInt64 XML
        /// <summary>
        /// Determines the log cache size in bytes.
        /// </summary>
        #endregion
        public static UInt64 LogSizeThreshold = 15000000;


        // Private / Internal variables.
        #region LogPath LogIO XML
        /// <summary>
        /// Holds data regarding log file paths.
        /// </summary>
        #endregion
        public static LogIO LogPath { get; internal set; }

        #region LogCache String XML
        /// <summary>
        /// Contains the log cache to be written to the log file.
        /// </summary>
        #endregion
        public static string LogCache { get; internal set; }

        #region LastLog LogArgs XML
        /// <summary>
        /// Contains the last log call data.
        /// </summary>
        #endregion
        public static LogArgs LastLog { get; internal set; }

        #region Initialized Boolean XML
        /// <summary>
        /// Is the logging session initialized?
        /// </summary>
        #endregion
        public static bool Initialized { get; internal set; }

        #region LoggingStripped Boolean XML
        /// <summary>
        /// If active (application launched with "-nolog" parameter), all functionality of PowerLog is disabled.
        /// </summary>
        #endregion
        public static bool LoggingStripped { get; internal set; }



        #region Initialize Method XML
        /// <summary>
        /// Initializes the log session.
        /// </summary>
        /// <param name="AllowLaunchParameters">Enable checking of launch parameters?</param>
        #endregion
        public static void Initialize(bool AllowLaunchParameters = true)
        {
            if (!Initialized)
            {
                AppDomain.CurrentDomain.ProcessExit += Log.SaveLog;

                AppDomain.CurrentDomain.UnhandledException += Log.LogException;
                AppDomain.CurrentDomain.UnhandledException += Log.SaveLog;

                if(LogPath == null) SwapLogIO(new LogIO(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Logs"), 
                    $"Log Output - ({DateTime.Now.ToString("HH-mm-ss tt, dd MMMM yyyy")})", "txt"), 
                    (IOSwapMode.KeepOldFile));

                LogCache = String.Empty;

                if (AllowLaunchParameters) AnalyzeLaunchParameters();

                Initialized = true;
            }
            else
            {
                throw new InvalidOperationException("LogSession already initialized.");
            }
        }

        #region SwapLogIO Method XML
        /// <summary>
        /// Sets a new LogIO object as log path data.
        /// </summary>
        /// <param name="NewIO">The 'LogIO' object to swap to.</param>
        /// <param name="SwapMode">The 'LogIO' swap mode.</param>
        #endregion
        public static void SwapLogIO(LogIO NewIO, IOSwapMode SwapMode) {
            if (LogPath != null) {
                if (!SwapMode.HasFlag(IOSwapMode.None)) {
                    if(SwapMode.HasFlag(IOSwapMode.Override)) {
                        if (SwapMode.HasFlag(IOSwapMode.Migrate)) {
                            if (File.Exists(NewIO.GetLogPath())) File.Delete(NewIO.GetLogPath());

                            if (File.Exists(LogPath.GetLogPath())) {
                                File.Copy(LogPath.GetLogPath(), NewIO.GetLogPath());
                            }
                        }else {
                            if (File.Exists(NewIO.GetLogPath())) File.Delete(NewIO.GetLogPath());
                        }
                    }else {
                        if (SwapMode.HasFlag(IOSwapMode.Migrate)) {
                            string OldLogContents = (File.Exists(LogPath.GetLogPath()) ? File.ReadAllText(LogPath.GetLogPath()) : String.Empty);
                            File.AppendAllText(NewIO.GetLogPath(), 
                                $"{OldLogContents}{((OldLogContents.EndsWith(Environment.NewLine)) ? String.Empty : Environment.NewLine)}");
                        }
                    }
                }

                if (!SwapMode.HasFlag(IOSwapMode.KeepOldFile) || SwapMode.HasFlag(IOSwapMode.None)) File.Delete(LogPath.GetLogPath());
            }else {
                if(File.Exists(NewIO.GetLogPath()) && SwapMode.HasFlag(IOSwapMode.Override) && !SwapMode.HasFlag(IOSwapMode.None)) File.Delete(NewIO.GetLogPath());
            }

            LogPath = NewIO;
        }

        #region CheckLogSize Method XML
        /// <summary>
        /// Gets the log size in memory.
        /// </summary>
        /// <returns>The log size in memory, in bytes.</returns>
        #endregion
        public static UInt64 CheckLogSize() {
            return (UInt64)(sizeof(char) * ((LogCache != null) ? LogCache.Length : 0f));
        }

        private static void AnalyzeLaunchParameters()
        {
            string[] LaunchParams = Environment.GetCommandLineArgs();

            for (int i = 0; i < LaunchParams.Length; i++)
            {
                if (LaunchParams[i] == "-nolog") LoggingStripped = true;
            }
        }
    }
}
