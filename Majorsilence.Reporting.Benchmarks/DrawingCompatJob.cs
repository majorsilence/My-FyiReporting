using BenchmarkDotNet.Attributes;
using System;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Extensions;
using BenchmarkDotNet.Jobs;
using JetBrains.Annotations;


namespace Majorsilence.Reporting.Benchmarks
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
    public class DrawingCompatJob : JobConfigBaseAttribute
    {
        private const int DefaultValue = -1;

        public DrawingCompatJob(
            RunStrategy runStrategy,
            int launchCount = DefaultValue,
            int warmupCount = DefaultValue,
            int iterationCount = DefaultValue,
            int invocationCount = DefaultValue,
            string? id = null,
            bool baseline = false,
            string buildConfiguration = "Release-DrawingCompat"
        ) : base(CreateJob(id, launchCount, warmupCount, iterationCount, invocationCount, runStrategy, baseline,
            buildConfiguration))
        {
        }

        private static Job CreateJob(string? id, int launchCount, int warmupCount, int iterationCount,
            int invocationCount, RunStrategy? runStrategy,
            bool baseline, string buildConfiguration)
        {
            var job = new Job(id).WithCustomBuildConfiguration(buildConfiguration);
            int manualValuesCount = 0;

            if (launchCount != DefaultValue)
            {
                job.Run.LaunchCount = launchCount;
                manualValuesCount++;
            }

            if (warmupCount != DefaultValue)
            {
                job.Run.WarmupCount = warmupCount;
                manualValuesCount++;
            }

            if (iterationCount != DefaultValue)
            {
                job.Run.IterationCount = iterationCount;
                manualValuesCount++;
            }

            if (invocationCount != DefaultValue)
            {
                job.Run.InvocationCount = invocationCount;
                manualValuesCount++;

                int unrollFactor =
                    job.Run.ResolveValue(RunMode.UnrollFactorCharacteristic, EnvironmentResolver.Instance);
                if (invocationCount % unrollFactor != 0)
                {
                    job.Run.UnrollFactor = 1;
                    manualValuesCount++;
                }
            }

            if (runStrategy != null)
            {
                job.Run.RunStrategy = runStrategy.Value;
                manualValuesCount++;
            }

            if (baseline)
                job.Meta.Baseline = true;
            

            return job.Freeze();
        }
    }
}