﻿using System;
using PowerLog;

namespace LogTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.OnLog += OnLog;
            LogSession.Initialize();

        Log:
            Logger.Log("Enter log type [Null / Info / Warning / Error / Network / Fatal]: ", LogType.Null, false, true, true, null);
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

                case "NETWORK":
                    MessageType = LogType.Network;
                    break;

                case "FATAL":
                    MessageType = LogType.Fatal;
                    break;

                default:
                    MessageType = LogType.Null;
                    Logger.Log("Invalid choice. Log type set to Null.", LogType.Warning, false, true, true, null);
                    Logger.Log(Environment.NewLine, LogType.Null, false, true, true, null);
                    break;
            }

            Logger.Log($"Picked {MessageType}.", LogType.Info, false, true, true, null);

            Logger.Log(Environment.NewLine, LogType.Null, false, true, true, null);

            Logger.Log("Enter message: ", LogType.Null, false, true, true, null);
            string LogText = Console.ReadLine();

            Logger.Log(LogText, MessageType, true, true, true, null);

            Logger.Log(Environment.NewLine, LogType.Null, false, true, true, null);
            Logger.Log(Environment.NewLine, LogType.Null, false, true, true, null);
            goto Log;
        }

        public static void OnLog(object sender, LogEventArgs logEventArgs) 
        {
            Console.Write($"{((logEventArgs.Timestamped) ? $"[{DateTime.Now.ToString("HH:mm:ss")}] " : "")}" +
                $"{((logEventArgs.MessageType != LogType.Null) ? $"{logEventArgs.MessageType.ToString()}: " : "")}" +
                $"{logEventArgs.LogMessage}");
            
            if (logEventArgs.MessageType == LogType.Fatal) {
                Console.WriteLine();
                Logger.Log("Throwing exception..", LogType.Info, true, true, false, null);
                throw new NullReferenceException();
            }
        }
    }
}
