// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Majorsilence.Reporting.Benchmarks;

var summary = BenchmarkRunner.Run(typeof(Program).Assembly, 
    new DebugBuildConfig()
        .WithOptions(ConfigOptions.DisableOptimizationsValidator));

Console.WriteLine(summary);
