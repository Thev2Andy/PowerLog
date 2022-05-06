using System;
using PowerLog;

namespace LogTester
{
    class Program
    {
        public static Log Log;

        static void Main(string[] args)
        {
            Log = new Log("LOG");

            Log.OnLog += OnLog;
            Log.LogSizeThreshold = 128;

            Log.Trace("Trace message..", LogData.Default, null, "Startup");
            Log.Debug("Debug message..", LogData.Default, null, "Startup");
            Log.Info("Info message..", LogData.Default, null, "Startup");
            Log.Warning("Warning message..", LogData.Default, null, "Startup");
            Log.Error("Error message..", LogData.Default, null, "Startup");
            Log.Network("Network message..", LogData.Default, null, "Startup");
            Log.Fatal("Fatal message..", LogData.Default, null, "Startup");
            Log.NA("NA (No-Header) message..", LogData.Default, null, "Startup");

            int Tries = 0;

            Array LogValues = Enum.GetValues(typeof(LogType));
            Random RNG = new Random();
            Log.Write("Dynamic log message..", (LogType)(LogValues.GetValue(RNG.Next(LogValues.Length))), LogData.Default);

            Log.NA("", LogData.EmptyLine);
            // Log.NA(Environment.NewLine, LogData.Default);

            Log.Info("Param test.. (~PARAM~)", LogData.Default, new System.Collections.Generic.List<LogParameter>() { new LogParameter("PARAM", "It works!") }, null);

            Log:
            Console.Write("Enter message: ");
            Log.Write(Console.ReadLine(), (LogType)(LogValues.GetValue(RNG.Next(LogValues.Length))), LogData.Default);

            if (Tries == 4)
            {
                // Log.Clear();
                // LogSession.SwapLogIO(new LogIO(LogSession.LogPath.LogPath, "joe mama", LogSession.LogPath.LogFileExtension), (IOSwapMode.Migrate));
                // throw new OutOfMemoryException();
                // throw new AccessViolationException();
                // throw new StackOverflowException();
            }

            Tries++;

            // Console.WriteLine($"Current log size: {(LogSession.CheckLogSize() / 1000000f)} MB / {(LogSession.LogSizeThreshold / 1000000f)} MB.");
            // Console.WriteLine($"Last log: {LogSession.LastLog}");

            // Console.ReadKey();

            goto Log;
        }

        public static void OnLog(LogArgs logEventArgs) 
        {
            ConsoleColor BackupColor = Console.ForegroundColor;
            ConsoleColor TargetColor;

            switch (logEventArgs.LogLevel)
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

                case LogType.NA:
                    TargetColor = BackupColor;
                    break;


                default:
                    TargetColor = BackupColor;
                    break;
            }

            Console.ForegroundColor = TargetColor;
            Console.WriteLine(logEventArgs.FormattedLog);

            Console.ForegroundColor = BackupColor;
        }
    }
}
