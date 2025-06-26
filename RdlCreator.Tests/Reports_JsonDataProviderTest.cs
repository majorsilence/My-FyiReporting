using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System;
using Majorsilence.Reporting.RdlCreator;
using NUnit.Framework;
using System.Text.RegularExpressions;
using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace Majorsilence.Reporting.RdlCreator.Tests
{
    [TestFixture]
    public class Reports_JsonDataProviderTest
    {
        string connectionString = "file=TestData.json";
        string dataProvider = "Json";

        [SetUp]
        public void Setup()
        {
            var files = new[]
            {
                "TestJsonMethodPdf.pdf"
            };

            foreach (var file in files)
            {
                var filepath = System.IO.Path.Combine(Environment.CurrentDirectory, file);
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
            }
        }

        [Test]
        public async Task Test1()
        {
            var create = new RdlCreator.Create();

            var fyiReport = await create.GenerateRdl(dataProvider,
                connectionString,
                "columns=EmployeID,LastName,FirstName,Title",
                pageHeaderText: "DataProviderTest TestMethod1");
            var ms = new Majorsilence.Reporting.Rdl.MemoryStreamGen();
            await fyiReport.RunGetData(null);
            await fyiReport.RunRender(ms, Majorsilence.Reporting.Rdl.OutputPresentationType.CSV);
            var text = ms.GetText();

            Assert.That(text, Is.Not.Null);
            Assert.That(NormalizeEOL(text), Is.EqualTo(NormalizeEOL(@"""DataProviderTest TestMethod1""
""EmployeID"",""LastName"",""FirstName"",""Title""
""1"",""Davolio"",""Nancy"",""Sales Representative""
""2"",""Fuller"",""Andrew"",""Vice President, Sales""
""3"",""Leverling"",""Janet"",""Sales Representative""
""4"",""Peacock"",""Margaret"",""Sales Representative""
""5"",""Buchanan"",""Steven"",""Sales Manager""
""6"",""Suyama"",""Michael"",""Sales Representative""
""8"",""Callahan"",""Laura"",""Inside Sales Coordinator""
""9"",""Dodsworth"",""Anne"",""Sales Representative""
""1 of 1""
")));
        }

        

        private string NormalizeEOL(string input)
        {
            return Regex.Replace(input, @"\r\n|\n\r|\n|\r", "\r\n");
        }
    }
}
