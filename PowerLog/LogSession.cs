using System;
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

        #region LogPath String XML
        /// <summary>
        /// The log file location.
        /// </summary>
        #endregion
        public static string LogPath;

        #region AutoCacheClear Boolean XML
        /// <summary>
        /// Allow automatic log cache clear?
        /// </summary>
        #endregion
        public static bool AutoCacheClear = true;

        #region CacheClearInterval Float XML
        /// <summary>
        /// The interval between cache clear calls.
        /// </summary>
        #endregion
        public static float CacheClearInterval = 7.5f;


        #region LoggingStripped Boolean XML
        /// <summary>
        /// Is the "-nolog" launch parameter active?
        /// </summary>
        #endregion
        public static bool LoggingStripped { get; internal set; }


        // Private variables.
        public static string LogCache { get; internal set; }
        public static bool Initialized { get; internal set; }

        #region Initialize Method XML
        /// <summary>
        /// Initializes the logger.
        /// </summary>
        #endregion
        public static void Initialize()
        {
            if (!Initialized)
            {
                AppDomain.CurrentDomain.ProcessExit += Logger.SaveLog;

                AppDomain.CurrentDomain.UnhandledException += Logger.LogException;
                AppDomain.CurrentDomain.UnhandledException += Logger.SaveLog;

                LogPath = Assembly.GetEntryAssembly().Location;

                AnalyzeLaunchParameters();

                Logger.ClearLog(true, false);
                Initialized = true;

                AutoCacheClearLoop();
            }
            else
            {
                throw new InvalidOperationException("Logger already initialized.");
            }
        }

        #region ClearCache Method XML
        /// <summary>
        /// Clear the log cache.
        /// </summary>
        #endregion
        public static void ClearCache() {
            Logger.SaveLog(null, EventArgs.Empty);
            Logger.ClearLog(false, false);
        }

        private static async void AutoCacheClearLoop()
        {
            await Task.Delay((int)Math.Round((CacheClearInterval * 1000)));
            if (AutoCacheClear) {
                ClearCache();
            }

            AutoCacheClearLoop();
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
