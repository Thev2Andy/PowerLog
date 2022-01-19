using System;
using System.Reflection;
using PowerLog;

namespace LogTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.OnLog += OnLog;

            Log:
            Logger.Log("Enter log type [Null / Info / Warning / Error / Fatal]: ", LogType.Null, false, null);
            LogType MessageType;

            switch (Console.ReadLine().ToUpper())
            {
                case "NULL":
                    MessageType = LogType.Null;
                    break;

                case "INFO":
                    MessageType = LogType.Info;
                    break;

                case "WARNING":
                    MessageType = LogType.Warning;
                    break;

                case "ERROR":
                    MessageType = LogType.Error;
                    break;

                case "FATAL":
                    MessageType = LogType.Fatal;
                    break;

                default:
                    MessageType = LogType.Null;
                    break;
            }

            Logger.Log($"Picked {MessageType}.", LogType.Info, false, null);

            Logger.WriteInLogFile = false;
            Logger.Log(Environment.NewLine, LogType.Null, false, null);
            Logger.WriteInLogFile = true;

            Logger.Log("Enter message: ", LogType.Null, false, null);
            string LogText = Console.ReadLine();

            Logger.Log(LogText, MessageType, true, null);

            Logger.WriteInLogFile = false;
            Logger.Log(Environment.NewLine, LogType.Null, false, null);
            Logger.WriteInLogFile = true;

            Logger.Log(Environment.NewLine, LogType.Null, false, null);
            goto Log;
        }

        public static void OnLog(object sender, LogEventArgs logEventArgs) 
        {
            Console.Write($"{((logEventArgs.Timestamped) ? $"[{DateTime.Now.ToString("HH:mm:ss")}] " : "")}{((logEventArgs.OutputType != LogType.Null) ? $"{logEventArgs.OutputType.ToString()}: " : "")}{logEventArgs.LogMessage}");
            
            if (logEventArgs.OutputType == LogType.Fatal) {
                Logger.Log(new Exception("Example exception..").ToString(), LogType.Null, true, null);
            }
        }
    }
}
