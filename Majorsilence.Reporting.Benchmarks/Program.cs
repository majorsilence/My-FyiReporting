// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Majorsilence.Reporting.Benchmarks;

//var summary = BenchmarkRunner.Run(typeof(Program).Assembly, 
//    new DebugBuildConfig()
//        .WithOptions(ConfigOptions.DisableOptimizationsValidator));

var summary = BenchmarkRunner.Run(typeof(Program).Assembly);


//var config = DefaultConfig.Instance
//    .AddJob(Job.Default.WithCustomBuildConfiguration("Release-DrawingCompat"))
//    .WithOptions(ConfigOptions.DisableOptimizationsValidator);

//var summary = BenchmarkSwitcher
//    .FromAssembly(typeof(Program).Assembly)
//    .Run(args, config);

Console.WriteLine(summary);
