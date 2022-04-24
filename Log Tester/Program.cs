using System;
using PowerLog;

namespace LogTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.OnLog += OnLog;
            // Log.OnSave += OnSave;
            LogSession.Initialize();
            LogSession.LogSizeThreshold = 128;

            Log.Trace("Trace message..", LogMode.Default, "Startup");
            Log.Debug("Debug message..", LogMode.Default, "Startup");
            Log.Info("Info message..", LogMode.Default, "Startup");
            Log.Warning("Warning message..", LogMode.Default, "Startup");
            Log.Error("Error message..", LogMode.Default, "Startup");
            Log.Network("Network message..", LogMode.Default, "Startup");
            Log.Fatal("Fatal message..", LogMode.Default, "Startup");
            Log.Null("Null (No-Header) message..", LogMode.Default, "Startup");

            int Tries = 0;

            Array LogValues = Enum.GetValues(typeof(LogType));
            Random RNG = new Random();
            Log.Write("Dynamic log message..", (LogType)(LogValues.GetValue(RNG.Next(LogValues.Length))));

            Log.Null("", (LogMode.Save | LogMode.InvokeEvent));
            // Log.Null("", LogMode.Default);

            Log:
            Console.Write("Enter message: ");
            Log.Write(Console.ReadLine(), (LogType)(LogValues.GetValue(RNG.Next(LogValues.Length))), LogMode.Default, null);

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

        private static void OnSave(object Sender, EventArgs E)
        {
            Log.Write("The log just got saved.", LogType.Info, (LogMode.Default | LogMode.NoSizeCheck));
        }

        public static void OnLog(object sender, LogArgs logEventArgs) 
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

                case LogType.Null:
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
