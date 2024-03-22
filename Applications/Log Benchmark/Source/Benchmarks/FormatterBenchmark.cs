using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using PowerLog;

namespace LogBenchmark
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    public class FormatterBenchmark
    {
        private static readonly Arguments Arguments = new Arguments
        {
            Content = "Hello, World! #~Parameter~ (Enrichment: ~@Enrichment~; Context: ~$Context~)",
            Severity = Severity.Debug,
            Time = DateTime.Now,
            Template = Template.Modern,
            Sender = "Benchmark",
            Parameters = new Dictionary<string, Object> { { "Parameter", 1 } },
            Enrichments = new Dictionary<string, Object> { { "Enrichment", 27 } },
            Context = new Dictionary<string, Object> { { "Context", 42 } },

            Logger = null,
        };


        [Benchmark]
        public void Compose_Parse()
        {
            Format.Compose(Arguments, Arguments.Template, true);
        }

        [Benchmark(Baseline = true)]
        public string CustomCompose_NoParse()
        {
            string Result = String.Empty;
            if (!String.IsNullOrEmpty(Arguments.Content)) {
                Result = $"{Arguments.Time.ToString(Arguments.Template.Date)} {((!Arguments.Severity.HasFlag(Severity.Generic)) ? $"[{Arguments.Severity}] " : String.Empty)}{Arguments.Content} ({Arguments.Sender ?? "N/A"})";
            }

            return Result;
        }

        [Benchmark]
        public void Compose_NoParse()
        {
            Format.Compose(Arguments, Arguments.Template, false);
        }

        [Benchmark]
        public void Parse()
        {
            Format.Parse(Arguments);
        }
    }
}
