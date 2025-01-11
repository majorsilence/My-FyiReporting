using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System;
using fyiReporting.RdlCreator;
using NUnit.Framework;
using System.Text.RegularExpressions;
using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace fyiReporting.RdlCreator.Tests
{
    [TestFixture]
    public class DataProviderTest
    {
        string connectionString = "Data Source=sqlitetestdb2.db;";
        string dataProvider = "Microsoft.Data.Sqlite";

        [SetUp]
        public void Setup()
        {
            var files = new[]
            {
                "TestMethodExcelLegacy.xls",
                "TestMethodExcel.xlsx",
                "TestMethodExcelDataOnly.xlsx",
                "TestMethodPdf.pdf"
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
        public async Task TestMethodCsv()
        {
            var create = new RdlCreator.Create();

            var fyiReport = await create.GenerateRdl(dataProvider,
                connectionString,
                "SELECT CategoryID, CategoryName, Description FROM Categories",
                pageHeaderText: "DataProviderTest TestMethod1");
            var ms = new fyiReporting.RDL.MemoryStreamGen();
            await fyiReport.RunGetData(null);
            await fyiReport.RunRender(ms, fyiReporting.RDL.OutputPresentationType.CSV);
            var text = ms.GetText();

            Assert.That(text, Is.Not.Null);
            Assert.That(NormalizeEOL(text), Is.EqualTo(@"""DataProviderTest TestMethod1""
""CategoryID"",""CategoryName"",""Description""
1,""Beverages"",""Soft drinks, coffees, teas, beers, and ales""
2,""Condiments"",""Sweet and savory sauces, relishes, spreads, and seasonings""
3,""Confections"",""Desserts, candies, and sweet breads""
4,""Dairy Products"",""Cheeses""
5,""Grains/Cereals"",""Breads, crackers, pasta, and cereal""
6,""Meat/Poultry"",""Prepared meats""
7,""Produce"",""Dried fruit and bean curd""
8,""Seafood"",""Seaweed and fish""
""1 of 1""
"));
        }

        [Test]
        public async Task TestMethodExcelLegacy()
        {
            var create = new RdlCreator.Create();

            var fyiReport = await create.GenerateRdl(dataProvider,
                connectionString,
                "SELECT CategoryID, CategoryName, Description FROM Categories",
                pageHeaderText: "DataProviderTest TestMethod1");

            string filepath = System.IO.Path.Combine(Environment.CurrentDirectory, "TestMethodExcelLegacy.xls");
            var ofs = new fyiReporting.RDL.OneFileStreamGen(filepath, true);
            await fyiReport.RunGetData(null);
            await fyiReport.RunRender(ofs, fyiReporting.RDL.OutputPresentationType.ExcelTableOnly);

            Assert.Multiple(() =>
            {
                Assert.That(System.IO.File.Exists(filepath), Is.True);
                Assert.That(new FileInfo(filepath).Length, Is.GreaterThan(0));
            });
        }

        [Test]
        public async Task TestMethodExcel2007()
        {
            var create = new RdlCreator.Create();

            var fyiReport = await create.GenerateRdl(dataProvider,
                connectionString,
                "SELECT CategoryID, CategoryName, Description FROM Categories",
                pageHeaderText: "DataProviderTest TestMethod1");

            string filepath = System.IO.Path.Combine(Environment.CurrentDirectory, "TestMethodExcel.xlsx");
            var ofs = new fyiReporting.RDL.OneFileStreamGen(filepath, true);
            await fyiReport.RunGetData(null);
            await fyiReport.RunRender(ofs, fyiReporting.RDL.OutputPresentationType.Excel2007);

            Assert.That(System.IO.File.Exists(filepath), Is.True);
        }

        [Test]
        public async Task TestMethodExcel2007DataOnly()
        {
            var create = new RdlCreator.Create();

            var fyiReport = await create.GenerateRdl(dataProvider,
                connectionString,
                "SELECT CategoryID, CategoryName, Description FROM Categories",
                pageHeaderText: "DataProviderTest TestMethod1");

            string filepath = System.IO.Path.Combine(Environment.CurrentDirectory, "TestMethodExcelDataOnly.xlsx");
            var ofs = new fyiReporting.RDL.OneFileStreamGen(filepath, true);
            await fyiReport.RunGetData(null);
            await fyiReport.RunRender(ofs, fyiReporting.RDL.OutputPresentationType.Excel2007DataOnly);

            Assert.That(System.IO.File.Exists(filepath), Is.True);
        }

        [Test]
        public async Task TestMethodPdf()
        {
            var create = new RdlCreator.Create();

            var fyiReport = await create.GenerateRdl(dataProvider,
                connectionString,
                "SELECT CategoryID, CategoryName, Description FROM Categories",
                pageHeaderText: "DataProviderTest TestMethod1");

            string filepath = System.IO.Path.Combine(Environment.CurrentDirectory, "TestMethodPdf.pdf");
            var ofs = new fyiReporting.RDL.OneFileStreamGen(filepath, true);
            await fyiReport.RunGetData(null);
            await fyiReport.RunRender(ofs, fyiReporting.RDL.OutputPresentationType.PDF);

            Assert.Multiple(() =>
            {
                Assert.That(System.IO.File.Exists(filepath), Is.True);
                Assert.That(new FileInfo(filepath).Length, Is.GreaterThan(0));
            });
        }

        [Test]
        public async Task TestReportFromDataTable()
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("CategoryID", typeof(int));
            dt.Columns.Add("CategoryName", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Rows.Add(1, "Beverages", "Soft drinks, coffees, teas, beers, and ales");
            dt.Rows.Add(2, "Condiments", "Sweet and savory sauces, relishes, spreads, and seasonings");
            dt.Rows.Add(3, "Confections", "Desserts, candies, and sweet breads");
            dt.Rows.Add(4, "Dairy Products", "Cheeses");
            dt.Rows.Add(5, "Grains/Cereals", "Breads, crackers, pasta, and cereal");
            dt.Rows.Add(6, "Meat/Poultry", "Prepared meats");
            dt.Rows.Add(7, "Produce", "Dried fruit and bean curd");
            dt.Rows.Add(8, "Seafood", "Seaweed and fish");

            var create = new RdlCreator.Create();
            var fyiReport = await create.GenerateRdl(dt,
                pageHeaderText: "DataProviderTest TestMethod1");
            var ms = new fyiReporting.RDL.MemoryStreamGen();
            await fyiReport.RunGetData(null);
            await fyiReport.RunRender(ms, fyiReporting.RDL.OutputPresentationType.CSV);
            var text = ms.GetText();

            Assert.That(text, Is.Not.Null);
            Assert.That(NormalizeEOL(text), Is.EqualTo(@"""DataProviderTest TestMethod1""
""CategoryID"",""CategoryName"",""Description""
1,""Beverages"",""Soft drinks, coffees, teas, beers, and ales""
2,""Condiments"",""Sweet and savory sauces, relishes, spreads, and seasonings""
3,""Confections"",""Desserts, candies, and sweet breads""
4,""Dairy Products"",""Cheeses""
5,""Grains/Cereals"",""Breads, crackers, pasta, and cereal""
6,""Meat/Poultry"",""Prepared meats""
7,""Produce"",""Dried fruit and bean curd""
8,""Seafood"",""Seaweed and fish""
""1 of 1""
"));
        }

        class Category
        {
            public int CategoryID { get; set; }
            public string CategoryName { get; set; }
            public string Description { get; set; }
        }

        [Test]
        public async Task TestReportFromEnumerable()
        {
            var data = new List<Category>
            {
                new Category { CategoryID = 1, CategoryName = "Beverages", Description = "Soft drinks, coffees, teas, beers, and ales" },
                new Category { CategoryID = 2, CategoryName = "Condiments", Description = "Sweet and savory sauces, relishes, spreads, and seasonings" },
                new Category { CategoryID = 3, CategoryName = "Confections", Description = "Desserts, candies, and sweet breads" },
                new Category { CategoryID = 4, CategoryName = "Dairy Products", Description = "Cheeses" },
                new Category { CategoryID = 5, CategoryName = "Grains/Cereals", Description = "Breads, crackers, pasta, and cereal" },
                new Category { CategoryID = 6, CategoryName = "Meat/Poultry", Description = "Prepared meats" },
                new Category { CategoryID = 7, CategoryName = "Produce", Description = "Dried fruit and bean curd" },
                new Category { CategoryID = 8, CategoryName = "Seafood", Description = "Seaweed and fish" }
            };

            var create = new RdlCreator.Create();
            var fyiReport = await create.GenerateRdl(data,
                pageHeaderText: "DataProviderTest TestMethod1");
            var ms = new fyiReporting.RDL.MemoryStreamGen();
            await fyiReport.RunGetData(null);
            await fyiReport.RunRender(ms, fyiReporting.RDL.OutputPresentationType.CSV);
            var text = ms.GetText();

            Assert.That(text, Is.Not.Null);
            Assert.That(NormalizeEOL(text), Is.EqualTo(@"""DataProviderTest TestMethod1""
""CategoryID"",""CategoryName"",""Description""
1,""Beverages"",""Soft drinks, coffees, teas, beers, and ales""
2,""Condiments"",""Sweet and savory sauces, relishes, spreads, and seasonings""
3,""Confections"",""Desserts, candies, and sweet breads""
4,""Dairy Products"",""Cheeses""
5,""Grains/Cereals"",""Breads, crackers, pasta, and cereal""
6,""Meat/Poultry"",""Prepared meats""
7,""Produce"",""Dried fruit and bean curd""
8,""Seafood"",""Seaweed and fish""
""1 of 1""
"));
        }

        private string NormalizeEOL(string input)
        {
            return Regex.Replace(input, @"\r\n|\n\r|\n|\r", "\r\n");
        }
    }
}
