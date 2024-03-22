using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using PowerLog.Sinks.Debugger;
using PowerLog.Sinks.SpectreTerminal;
using PowerLog.Sinks.Terminal;
using PowerLog.Sinks.Markdown;
using PowerLog.Sinks.IO;
using PowerLog.Enrichers.Stacktrace;
using PowerLog.Enrichers.Environment;
using PowerLog.Enrichers.Process;
using PowerLog.Enrichers.Thread;
using PowerLog.Filters.Predicate;
using PowerLog.Sinks.Asynchronous;
using PowerLog.Sinks.Logger;
using PowerLog;

namespace LogTester
{
    class Program
    {
        public static Log Log;
        public static Log Proxy;

        static void Main(string[] Args)
        {
            Log = new Log("Log", Verbosity.All);
            Log.PushDebugger("Log Debugger Sink", Verbosity.All).
                PushSpectreConsole("Log Spectre Console Sink", true, Verbosity.All).
                // PushConsole("Log ConsoleSink", true, Verbosity.All).
                PushMarkdown("Log Markdown Sink").
                PushFile("Log File Sink");
                // PushAsynchronous("Log Asynchronous Sink", Verbosity.All);

            Proxy = new Log("Proxy", Verbosity.All);
            Proxy.PushLogger("Proxy Logger Sink", Log, Verbosity.All, false);

            // List<AsynchronousSink> AsyncSinks = Log.Find<AsynchronousSink>();
            // AsynchronousSink AsyncSink = ((AsyncSinks.Count > 0) ? AsyncSinks[0] : null);
            /*if (AsyncSink != null)
            {
                DebuggerSink DebuggerSink = new DebuggerSink("Log Debugger Sink", Log, Verbosity.All);
                SpectreConsoleSink SpectreConsoleSink = new SpectreConsoleSink("Log Spectre Console Sink", Log, true, Verbosity.All);
                ConsoleSink ConsoleSink = new ConsoleSink("Log Console Sink", Log, true, Verbosity.All);
                MarkdownSink MarkdownSink = new MarkdownSink("Log Markdown Sink", Log, Verbosity.All);
                FileSink FileSink = new FileSink("Log File Sink", Log, Verbosity.All);

                AsyncSink.Push(DebuggerSink);
                AsyncSink.Push(SpectreConsoleSink);
                // AsyncSink.Push(ConsoleSink);
                AsyncSink.Push(MarkdownSink);
                AsyncSink.Push(FileSink);
            }*/

            Log.FormattingTemplate = Template.Minimal;
            Template PreviousTemplate = Log.FormattingTemplate;

            Thread.CurrentThread.Name = "Bob";

            Log.AppendStacktrace("Stacktrace Enricher").
                AppendProcess("Process Enricher").
                AppendThread("Thread Enricher").
                AppendEnvironment("Environment Enricher", new List<(string, string)> { ("Path", "PATH"), ("AppData", "APPDATA"), ("Temp", "TEMP") });

            // Log.Find<StacktraceEnricher>()[0].IsEnabled = false;

            Log.FilterByPredicate("Predicate Filter", (Log) => { return Log.Enrichments.ContainsKey("Thread ID"); });
            Log.Find<PredicateFilter>()[0].IsEnabled = false;


            Log.Verbose("Sinks added successfully.");

            Log.Verbose("Starting log severity demo..");
            Log.Write(String.Empty, Severity.Verbose);

            Log.Verbose("Verbose message..", null, "Startup");
            Log.Trace("Trace message..", null, "Startup");
            Log.Debug("Debug message..", null, "Startup");
            Log.Network("Network message..", null, "Startup");
            Log.Information("Information message..", null, "Startup");
            Log.Notice("Notice message..", null, "Startup");
            Log.Caution("Caution message", null, "Startup");
            Log.Warning("Warning message..", null, "Startup");
            Log.Alert("Alert message..", null, "Startup");
            Log.Error("Error message..", null, "Startup");
            Log.Critical("Critical message..", null, "Startup");
            Log.Emergency("Emergency message..", null, "Startup");
            Log.Fatal("Fatal message..", null, "Startup");
            Log.Generic("Generic (No-Header) message..", null, "Startup");
            Log.Write("Flags message.." , (Severity.Network | Severity.Error));


            Severity[] LogValues = Enum.GetValues<Severity>();
            Random RNG = new Random();
            Log.Write("Dynamic log message..", LogValues[RNG.Next(LogValues.Length)]);

            Log.Generic(String.Empty);

            // Log.FormattingTemplate = Log.FormattingTemplate with { Flags = Log.FormattingTemplate.Flags & ~Template.Options.Parse };
            Log.Generic($"Random Number: ~Random Number~", new Dictionary<string, Object> { { "Random Number", RNG.Next(0, 101) } }, RNG);
            Log.FormattingTemplate = PreviousTemplate;

            Log.Generic(String.Empty);

            // Log.Information("Param test.. (~PARAM~, ~PARAM2~)", new Dictionary<string, Object> { { "PARAM", "Hello" }, { "PARAM2", "World!" } }, null);
            // Log.Information($"Application compiled at {File.GetLastWriteTime(Assembly.GetEntryAssembly().Location).ToString("HH:mm:ss, dd MMMM yyyy")}, in {typeof(Program).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>().Configuration} mode.", null, Process.GetCurrentProcess().ProcessName);

            Log.Context.Add("Color Override", "84, 0, 255");
            Log.Context.Add("Highlight Override", true);
            Log.Information($"Colored log via contextual properties.. (Color: `~$Color Override~`, Highlight: `~$Highlight Override~`)");
            Log.Context.Clear();

            Log.Information("F |C|");

            Log.FormattingTemplate = new Template("| |T| | |I| | |S| | |C| | |O| |", "HH:mm:ss", Template.Options.Analysis);
            Log.Information("Markdown table syntax test.");
            Log.FormattingTemplate = PreviousTemplate;

            Proxy.Information("Information message.. (Sent through a logger sink to the main logger.)");

            Log.Verbose("Log severity demo ended.");
            Log.Generic(String.Empty);


            while (true)
            {
                Console.Write("Enter message: ");
                string Message = Console.ReadLine();

                Log.Context.Add("ID", RNG.Next(0, 101));
                Log.Write(Message, ((Severity)(LogValues.GetValue(RNG.Next(LogValues.Length)))));
                Log.Context.Remove("ID");

                Console.WriteLine();
            }
        }
    }
}
