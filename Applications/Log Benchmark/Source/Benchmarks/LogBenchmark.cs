using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using PowerLog;
using Serilog;

namespace LogBenchmark
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class LogBenchmark
    {
        private const string StackedPowerLogContent = "Hello, world! #~Number~ (Constant: ~@Enriched Constant~; Context: ~$Contextual Constant~)";
        private const string PowerLogContent = "Hello, world! #~Number~";
        private const Severity CombinedSeverity = Severity.Network | Severity.Error;
        private static readonly Dictionary<string, Object> Parameters = new Dictionary<string, Object>() { { "Number", Number } };
        private static readonly PowerLog.Log PowerLog_Logger = new PowerLog.Log("Benchmark Logger", Verbosity.All.Except(Severity.Verbose));
        private static readonly PowerLog.Log PowerLog_Logger_Stacked = new PowerLog.Log("Stacked Benchmark Logger", Verbosity.All.Except(Severity.Verbose));
        private const string Sender = "Benchmark";

        private static readonly int Number = 1;

        private const string SerilogContent = "Hello, world! #{Number}";
        private static readonly ILogger Serilog_Logger = new LoggerConfiguration().CreateLogger();
        private static readonly ILogger Serilog_Logger_Stacked = new LoggerConfiguration().Enrich.WithThreadId().Enrich.WithThreadName().CreateLogger();


        [Benchmark]
        public void PowerLog_SingleLevelAbstracted()
        {
            PowerLog_Logger.Information(PowerLogContent, Parameters, Sender);
        }

        [Benchmark]
        public void PowerLog_SingleLevelAbstracted_Excluded()
        {
            PowerLog_Logger.Verbose(PowerLogContent, Parameters, Sender);
        }

        [Benchmark]
        public void PowerLog_SingleLevelWrite()
        {
            PowerLog_Logger.Write(PowerLogContent, Severity.Information, Parameters, Sender);
        }

        [Benchmark]
        public void PowerLog_CombinedLevel()
        {
            PowerLog_Logger.Write(PowerLogContent, CombinedSeverity, Parameters, Sender);
        }

        [Benchmark]
        public void PowerLog_Stacked_SingleLevelAbstracted()
        {
            PowerLog_Logger_Stacked.Information(StackedPowerLogContent, Parameters, Sender);
        }

        [Benchmark]
        public void PowerLog_Stacked_SingleLevelAbstracted_Excluded()
        {
            PowerLog_Logger_Stacked.Information(StackedPowerLogContent, Parameters, Sender);
        }

        [Benchmark]
        public void Serilog_Stacked_Abstracted()
        {
            Serilog_Logger_Stacked.Information(SerilogContent, Number);
        }

        [Benchmark]
        public void Serilog_Stacked_Abstracted_Excluded()
        {
            Serilog_Logger_Stacked.Information(SerilogContent, Number);
        }

        [Benchmark]
        public void Serilog_Abstracted_Excluded()
        {
            Serilog_Logger.Verbose(SerilogContent, Number);
        }

        [Benchmark(Baseline = true)]
        public void Serilog_Abstracted()
        {
            Serilog_Logger.Information(SerilogContent, Number);
        }

        [Benchmark]
        public void Serilog_Write()
        {
            Serilog_Logger.Write(Serilog.Events.LogEventLevel.Information, SerilogContent, Number);
        }



        public LogBenchmark()
        {
            PowerLog_Logger.FormattingTemplate = Template.Modern;
            PowerLog_Logger_Stacked.FormattingTemplate = Template.Modern;

            PowerLog_Logger_Stacked.Context.TryAdd("Contextual Constant", 1);
            PowerLog_Logger_Stacked.AppendCustomThread("Custom Thread Enricher");

            Serilog_Logger_Stacked.BindProperty("Contextual Constant", 1, false, out _);
        }
    }
}
