using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System;
using RdlCreator;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace RdlCreator.Tests
{
    [TestFixture]
    public class ManualReportDefinitionTest
    {
        string connectionString = "Data Source=sqlitetestdb2.db;";
        string dataProvider = "Microsoft.Data.Sqlite";

        [Test]
        public void TestMethod1()
        {
            var create = new RdlCreator.Create();
            var report = GenerateTestData();
            var fyiReport = create.GenerateRdl(report);
            var ms = new fyiReporting.RDL.MemoryStreamGen();
            fyiReport.RunGetData(null);
            fyiReport.RunRender(ms, fyiReporting.RDL.OutputPresentationType.CSV);
            var text = ms.GetText();
           
            Assert.That(text, Is.Not.Null);
            Assert.That(NormalizeEOL(text), Is.EqualTo(@"""Test Data Set Report""
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

        private RdlCreator.Report GenerateTestData()
        {

            // Create an instance of the Report class
            var report = new Report
            {
                Description = "Sample report",
                Author = "John Doe",
                PageHeight = "11in",
                PageWidth = "8.5in",
                Width = "7.5in",
                TopMargin = ".25in",
                LeftMargin = ".25in",
                RightMargin = ".25in",
                BottomMargin = ".25in",
                DataSources = new DataSources
                {
                    DataSource = new DataSource
                    {
                        Name = "DS1",
                        ConnectionProperties = new ConnectionProperties
                        {
                            DataProvider = dataProvider,
                            ConnectString = connectionString
                        }
                    }
                },
                DataSets = new DataSets
                {
                    DataSet = new DataSet
                    {
                        Name = "Data",
                        Query = new Query
                        {
                            DataSourceName = "DS1",
                            CommandText = "SELECT CategoryID, CategoryName, Description FROM Categories"
                        },
                        Fields = new Fields
                        {
                            Field = new List<Field>
                            {
                                new Field { Name = "CategoryID", DataField = "CategoryID", TypeName = "System.Int64" },
                                new Field { Name = "CategoryName", DataField = "CategoryName", TypeName = "System.String" },
                                new Field { Name = "Description", DataField = "Description", TypeName = "System.String" }
                            }
                        }
                    }
                },
                PageHeader = new PageHeader
                {
                    Height = ".5in",
                    ReportItems = new ReportItemsHeader
                    {
                        Textbox = new Textbox
                        {
                            Name = "Textbox1",
                            Top = ".1in",
                            Left = ".1in",
                            Width = "6in",
                            Height = ".25in",
                            Value = new Value { Text = "Test Data Set Report" },
                            Style = new Style { FontSize = "15pt", FontWeight = "Bold" }
                        }
                    },
                    PrintOnFirstPage = "true",
                    PrintOnLastPage = "true"
                },
                Body = new Body
                {
                    ReportItems = new ReportItemsBody
                    {
                        Table = new Table
                        {
                            TableName = "Table1",
                            DataSetName = "Data",
                            NoRows = "Query returned no rows!",
                            TableColumns = new TableColumns
                            {
                                TableColumn = new List<TableColumn>
                            {
                                new TableColumn { Width = "1.25in" },
                                new TableColumn { Width = "1.5in" },
                                new TableColumn { Width = "1.375in" }
                            }
                            },
                            Header = new Header
                            {
                                TableRows = new TableRows
                                {
                                    TableRow = new TableRow
                                    {
                                        Height = "12pt",
                                        TableCells = new TableCells()
                                        {
                                            TableCell = new List<TableCell>
                                            {
                                                new TableCell {  ReportItems= new TableCellReportItems(){ Textbox = new Textbox { Name = "Textbox2",
                                                    Value = new Value { Text = "CategoryID" },
                                                    Style = new Style { TextAlign = "Center", FontWeight = "Bold" } } } },
                                                new TableCell { ReportItems= new TableCellReportItems(){Textbox = new Textbox { Name = "Textbox3",
                                                    Value = new Value { Text = "CategoryName" },
                                                    Style = new Style { TextAlign = "Center", FontWeight = "Bold" } } } },
                                                new TableCell { ReportItems= new TableCellReportItems(){Textbox = new Textbox { Name = "Textbox4",
                                                    Value = new Value { Text = "Description" },
                                                    Style = new Style { TextAlign = "Center", FontWeight = "Bold" } } } }
                                            }
                                        }
                                    }
                                },
                                RepeatOnNewPage = "true"
                            },
                            Details = new Details
                            {
                                TableRows = new TableRows
                                {
                                    TableRow = new TableRow
                                    {
                                        Height = "12pt",
                                        TableCells = new TableCells()
                                        {
                                            TableCell = new List<TableCell>
                                            {
                                                new TableCell { ReportItems= new TableCellReportItems(){ Textbox = new Textbox { Name = "CategoryID",
                                                    Value = new Value { Text = "=Fields!CategoryID.Value" }, CanGrow = "false" } } },
                                                new TableCell { ReportItems = new TableCellReportItems(){ Textbox = new Textbox { Name = "CategoryName",
                                                    Value = new Value { Text = "=Fields!CategoryName.Value" }, CanGrow = "true" } } },
                                                new TableCell { ReportItems= new TableCellReportItems(){ Textbox = new Textbox { Name = "Description",
                                                    Value = new Value { Text = "=Fields!Description.Value" }, CanGrow = "true" } } }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    Height = "36pt"
                },
                PageFooter = new PageFooter
                {
                    Height = "14pt",
                    ReportItems = new ReportItemsFooter
                    {
                        Textbox = new Textbox
                        {
                            Name = "Textbox5",
                            Top = "1pt",
                            Left = "10pt",
                            Height = "12pt",
                            Width = "3in",
                            Value = new Value { Text = "=Globals!PageNumber + ' of ' + Globals!TotalPages" },
                            Style = new Style { FontSize = "10pt", FontWeight = "Normal" }
                        }
                    },
                    PrintOnFirstPage = "true",
                    PrintOnLastPage = "true"
                }
            };

            return report;
        }
    }
}
