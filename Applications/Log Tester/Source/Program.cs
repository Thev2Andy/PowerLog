using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using PowerLog.Sinks.Debugger;
using PowerLog.Sinks.SpectreTerminal;
using PowerLog.Sinks.Terminal;
using PowerLog.Sinks.Markdown;
using PowerLog.Sinks.IO;
using PowerLog;

namespace LogTester
{
    class Program
    {
        public static Log Log;

        static void Main(string[] Args)
        {
            Log = new Log("Log", Verbosity.All);
            Log.PushDebugger("Log DebuggerSink", Verbosity.All, true).
                PushSpectreConsole("Log SpectreConsoleSink", true, Verbosity.All).
                // PushConsole("Log ConsoleSink", true, Verbosity.All).
                PushFile("Log FileSink").
                PushMarkdown("Log MarkdownSink");


            Log.Verbose("Sinks added successfully.");

            Log.Verbose("Starting log severity demo..");
            Log.Write(String.Empty, Severity.Verbose);

            Log.Verbose("Verbose message..", Template.Default, null, "Startup");
            Log.Trace("Trace message..", Template.Default, null, "Startup");
            Log.Debug("Debug message..", Template.Default, null, "Startup");
            Log.Network("Network message..", Template.Default, null, "Startup");
            Log.Information("Information message..", Template.Default, null, "Startup");
            Log.Notice("Notice message..", Template.Default, null, "Startup");
            Log.Caution("Caution message", Template.Default, null, "Startup");
            Log.Warning("Warning message..", Template.Default, null, "Startup");
            Log.Alert("Alert message..", Template.Default, null, "Startup");
            Log.Error("Error message..", Template.Default, null, "Startup");
            Log.Critical("Critical message..", Template.Default, null, "Startup");
            Log.Emergency("Emergency message..", Template.Default, null, "Startup");
            Log.Fatal("Fatal message..", Template.Default, null, "Startup");
            Log.Generic("Generic (No-Header) message..", Template.Default, null, "Startup");
            Log.Write("Flags message.." , (Severity.Fatal | Severity.Notice | Severity.Generic));


            Array LogValues = Enum.GetValues(typeof(Severity));
            Random RNG = new Random();
            Log.Write("Dynamic log message..", (Severity)(LogValues.GetValue(RNG.Next(LogValues.Length))));

            Log.Generic(String.Empty);

            Log.Information("Param test.. (~PARAM~, ~PARAM2~)", Template.Default, new Dictionary<string, Object> { { "PARAM", "Hello" }, { "PARAM2", "World!" } }, null);
            Log.Information($"Application compiled at {File.GetLastWriteTime(Assembly.GetEntryAssembly().Location).ToString("HH-mm-ss tt, dd MMMM yyyy")}, in {typeof(Program).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>().Configuration} mode.", Template.Default, null, Process.GetCurrentProcess().ProcessName);

            Log.Information($"Here's a fancy overridden colored log in Spectre.Console, however the normal console will use the default color. (Color: `~Color Override~`, Highlight: `~Highlight Override~`)", null, new Dictionary<string, Object> { { "Color Override", "84, 0, 255" }, { "Highlight Override", true } });
            Log.Information("F |C|");
            Log.Information("MD Sink Test", new Template("| |T| | |I| | |S| | |C| | |O| |"));

            Log.Verbose("Log severity demo ended.");
            Log.Generic(String.Empty);


            while (true)
            {
                Console.Write("Enter message: ");
                Log.Write(Console.ReadLine(), ((Severity)(LogValues.GetValue(RNG.Next(LogValues.Length)))));
                Console.WriteLine();
            }
        }
    }
}
