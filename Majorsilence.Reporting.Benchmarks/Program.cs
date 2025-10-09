// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Majorsilence.Reporting.Benchmarks;
using System.Diagnostics;

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


// Run a simple multithreaded test

var jsonBp = new JsonDataProviderBenchmark();
Console.WriteLine("|Threads|Duration|Source|Calls/Sec|Total Calls|Errors|");
Console.WriteLine("|-------|--------|------|---------:|----------:|-----:|");
ConcurrentCallCount(jsonBp, threadCount: 1, durationInSeconds: 30);
ConcurrentCallCount(jsonBp, threadCount: 20, durationInSeconds: 30);
ConcurrentCallCount(jsonBp, threadCount: 20, durationInSeconds: 30);
ConcurrentCallCount(jsonBp, threadCount: 30, durationInSeconds: 30);
ConcurrentCallCount(jsonBp, threadCount: 40, durationInSeconds: 30);
ConcurrentCallCount(jsonBp, threadCount: 50, durationInSeconds: 30);


void ConcurrentCallCount(JsonDataProviderBenchmark inst, int threadCount, int durationInSeconds)
{
    Stopwatch stopwatch = new Stopwatch();

// Start the stopwatch
    stopwatch.Start();

    int functionCallCount = 0;
    int errors = 0;

// Create an array to hold the threads
    Thread[] threads = new Thread[threadCount];

// Create and start the threads
    for (int i = 0; i < threadCount; i++)
    {
        threads[i] = new Thread(async () =>
        {
            while (stopwatch.Elapsed.TotalSeconds < durationInSeconds)
            {
                try
                {
                    await inst.NestedJson();

                    Interlocked.Increment(ref functionCallCount);
                }
                catch (Exception ex)
                {
                    Interlocked.Increment(ref errors);
                    Console.WriteLine(ex);
                }
            }
        });

        threads[i].Start();
    }

    foreach (var thread in threads)
    {
        thread.Join();
    }

    stopwatch.Stop();
    double callRate = functionCallCount / stopwatch.Elapsed.TotalSeconds;

    Console.WriteLine(
        $"|{threadCount}|{durationInSeconds}|{inst.GetType().Name}|{callRate:N0}|{functionCallCount:N0}|{errors:N0}|");
}