using System;
using System.Collections.Generic;
using BenchmarkDotNet.Running;
using PowerLog;

namespace LogBenchmark
{
    public class Program
    {
        public static void Main(string[] Args)
        {
            BenchmarkRunner.Run<LogBenchmark>();
            // BenchmarkRunner.Run<FormatterBenchmark>();
        }
    }
}
