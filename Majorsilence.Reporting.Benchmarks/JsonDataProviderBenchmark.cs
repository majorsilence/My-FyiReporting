using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System;
using Majorsilence.Reporting.RdlCreator;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Majorsilence.Reporting.Benchmarks
{
    [AsciiDocExporter]
    [HtmlExporter]
    [MemoryDiagnoser]
    [RPlotExporter]
    [SimpleJob(RuntimeMoniker.Net80, baseline: true)]
    public class JsonDataProviderBenchmark
    {
        string dataProvider = "Json";
        private string connectionString = "file=NestedJsonData.json";
        
        [Benchmark]
        public async Task NestedJson()
        {
            var create = new RdlCreator.Create();

            var fyiReport = await create.GenerateRdl(dataProvider,
                connectionString,
                "columns=EmployeeID,LastName,FirstName,ContactInfo_Phone,ContactInfo_Email",
                pageHeaderText: "DataProviderTest TestMethod1");
            using var ms = new Majorsilence.Reporting.Rdl.MemoryStreamGen();
            await fyiReport.RunGetData(null);
            await fyiReport.RunRender(ms, Majorsilence.Reporting.Rdl.OutputPresentationType.CSV);
            var text = ms.GetText();
        }
    }
}