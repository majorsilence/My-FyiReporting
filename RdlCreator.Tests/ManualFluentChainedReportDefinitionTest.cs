using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System;
using Majorsilence.Reporting.RdlCreator;
using NUnit.Framework;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using System.Linq;

namespace Majorsilence.Reporting.RdlCreator.Tests
{
    [TestFixture]
    public class ManualFluentChainedReportDefinitionTest
    {
        string connectionString = "Data Source=sqlitetestdb2.db;";
        string dataProvider = "Microsoft.Data.Sqlite";

        [Test]
        public async Task PdfStreamExport()
        {
            var create = new RdlCreator.Create();
            var report = GenerateTestData();
            var fyiReport = await create.GenerateRdl(report);
            using var ms = new Majorsilence.Reporting.Rdl.MemoryStreamGen();
            await fyiReport.RunGetData(null);
            await fyiReport.RunRender(ms, Majorsilence.Reporting.Rdl.OutputPresentationType.PDF);
            var pdfStream = ms.GetStream();

            pdfStream.Position = 0;
            using var pdfDocument = PdfDocument.Open(pdfStream);
            var text = string.Join(" ", pdfDocument.GetPages().SelectMany(page => page.GetWords()).Select(word => word.Text));

            Assert.That(text, Is.Not.Null);
            Assert.That(text, Is.EqualTo("Test Data Set Report CategoryID CategoryName Description Beverages Soft drinks, coffees, teas, beers, and ales Condiments Sweet and savory sauces, relishes, spreads, and seasonings Confections Desserts, candies, and sweet breads Dairy Products Cheeses Grains/Cereals Breads, crackers, pasta, and cereal Meat/Poultry Prepared meats Produce Dried fruit and bean curd Seafood Seaweed and fish 1 of 1"));
        }

        [Test]
        public async Task PdfDiskExport()
        {
            var create = new RdlCreator.Create();
            var report = GenerateTestData();
            var fyiReport = await create.GenerateRdl(report);
            using var ms = new Majorsilence.Reporting.Rdl.MemoryStreamGen();
            await fyiReport.RunGetData(null);
            await fyiReport.RunRender(ms, Majorsilence.Reporting.Rdl.OutputPresentationType.PDF);
            var pdfStream = ms.GetStream();
            pdfStream.Position = 0;

            using var fileStream = new FileStream("PdfChainedDiskExport.pdf", FileMode.Create, FileAccess.Write);
            pdfStream.CopyTo(fileStream);
            await fileStream.DisposeAsync();

            using var pdfDocument = PdfDocument.Open("PdfChainedDiskExport.pdf");
            var text = string.Join(" ", pdfDocument.GetPages().SelectMany(page => page.GetWords()).Select(word => word.Text));

            Assert.That(text, Is.Not.Null);
            Assert.That(text, Is.EqualTo("Test Data Set Report CategoryID CategoryName Description Beverages Soft drinks, coffees, teas, beers, and ales Condiments Sweet and savory sauces, relishes, spreads, and seasonings Confections Desserts, candies, and sweet breads Dairy Products Cheeses Grains/Cereals Breads, crackers, pasta, and cereal Meat/Poultry Prepared meats Produce Dried fruit and bean curd Seafood Seaweed and fish 1 of 1"));
        }

        [Test]
        public async Task NoDataSetPdfDiskExport()
        {
            var create = new RdlCreator.Create();
            var report = SmallTestData();
            var fyiReport = await create.GenerateRdl(report);
            using var ms = new Majorsilence.Reporting.Rdl.MemoryStreamGen();
            await fyiReport.RunGetData(null);
            await fyiReport.RunRender(ms, Majorsilence.Reporting.Rdl.OutputPresentationType.PDF);
            var pdfStream = ms.GetStream();
            pdfStream.Position = 0;

            using var fileStream = new FileStream("NoDataSetExport.pdf", FileMode.Create, FileAccess.Write);
            pdfStream.CopyTo(fileStream);
            await fileStream.DisposeAsync();

            using var pdfDocument = PdfDocument.Open("NoDataSetExport.pdf");
            var text = string.Join(" ", pdfDocument.GetPages().SelectMany(page => page.GetWords()).Select(word => word.Text));

            Assert.That(text, Is.Not.Null);
            Assert.That(text, Is.EqualTo("Test Header Text Area 1 Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. 1 of 1"));
        }


        private RdlCreator.Report GenerateTestData()
        {

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
                BottomMargin = ".25in"
            }
            .WithDataSources(
                 new DataSources
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
                 }
                )
            .WithDataSets(
                new DataSets
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
                }
                )
            .WithPageHeader(
                new PageHeader
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
                })
            .WithPageFooter(new PageFooter
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
            });

            // Add a body to the report

            report
            .WithBody("36pt")
            .WithTable()
            .WithTableName("Table1")
            .WithTableDataSetName("Data")
            .WithTableNoRows("Query returned no rows!")
            .WithTableHeader(new TableRow
            {
                Height = "12pt",
                TableCells = new TableCells()
                {
                    TableCell = new List<TableCell>
                    {
                        new TableCell {  ReportItems= new TableCellReportItems(){ ReportItem = new Textbox { Name = "Textbox2",
                            Value = new Value { Text = "CategoryID" },
                            Style = new Style { TextAlign = "Center", FontWeight = "Bold" } } } },
                        new TableCell { ReportItems= new TableCellReportItems(){ReportItem = new Textbox { Name = "Textbox3",
                            Value = new Value { Text = "CategoryName" },
                            Style = new Style { TextAlign = "Center", FontWeight = "Bold" } } } },
                        new TableCell { ReportItems= new TableCellReportItems(){ReportItem = new Textbox { Name = "Textbox4",
                            Value = new Value { Text = "Description" },
                            Style = new Style { TextAlign = "Center", FontWeight = "Bold" } } } }
                    }
                }
            }, repeatOnNewPage: "true")
            .WithTableColumns(new TableColumns
            {
                TableColumn = new List<TableColumn>
                            {
                                new TableColumn { Width = "1.25in" },
                                new TableColumn { Width = "1.5in" },
                                new TableColumn { Width = "1.375in" }
                            }
            })
            .WithTableDetails(new TableRow
            {
                Height = "12pt",
                TableCells = new TableCells()
                {
                    TableCell = new List<TableCell>
                    {
                        new TableCell {
                            ReportItems= new TableCellReportItems()
                            {
                                    ReportItem = new CustomReportItems()
                                    {
                                        Name = "QrCode",
                                        Type = "QR Code",
                                        Width = "35.91mm",
                                        Height = "22pt",
                                        CustomProperties = new CustomProperties
                                        {
                                            CustomProperty = new CustomProperty()
                                            {
                                            Name = "QrCode",
                                            Value = "=Fields!CategoryID.Value"
                                            }
                                        },
                                    CanGrow="true",
                                    Style = new Style
                                    {
                                        BorderStyle= new BorderStyle
                                        {
                                            Default="None",
                                            Bottom="Solid"
                                        },
                                        BorderColor=new BorderColor
                                        {
                                            Bottom = "Gray"
                                        },
                                        BorderWidth= new BorderWidth
                                        {
                                            Bottom="1pt"
                                        }
                                    }
                                }
                            }
                        },
                        new TableCell
                        {
                            ReportItems = new TableCellReportItems()
                            {
                                ReportItem = new Textbox {
                                    Name = "CategoryName",
                                    Value = new Value
                                        {
                                        Text = "=Fields!CategoryName.Value"
                                    },
                                    CanGrow = "true",
                                    Style = new Style
                                    {
                                        BorderStyle= new BorderStyle
                                        {
                                            Default="None",
                                            Bottom="Solid"
                                        },
                                        BorderColor=new BorderColor
                                        {
                                            Bottom = "Gray"
                                        },
                                        BorderWidth= new BorderWidth
                                        {
                                            Bottom="1pt"
                                        }

                                    }
                                }
                            }
                        },
                        new TableCell
                        {
                            ReportItems= new TableCellReportItems()
                            {
                                ReportItem = new Textbox
                                {
                                    Name = "Description",
                                    Value = new Value
                                    {
                                        Text = "=Fields!Description.Value"
                                    },
                                    CanGrow = "true",
                                    Style = new Style
                                    {
                                        BorderStyle= new BorderStyle
                                        {
                                            Default="None",
                                            Bottom="Solid"
                                        },
                                        BorderColor=new BorderColor
                                        {
                                            Bottom = "Gray"
                                        },
                                        BorderWidth= new BorderWidth
                                        {
                                            Bottom="1pt"
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            });

            return report;
        }

        private RdlCreator.Report SmallTestData()
        {

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
                BottomMargin = ".25in"
            }
            .WithPageHeader(
                new PageHeader
                {
                    Height = ".5in",
                    ReportItems = new ReportItemsHeader
                    {
                        Textbox = new Textbox
                        {
                            Name = "Header",
                            Top = ".1in",
                            Left = ".1in",
                            Width = "6in",
                            Height = ".25in",
                            Value = new Value { Text = "Test Header" },
                            Style = new Style { FontSize = "15pt", FontWeight = "Bold" }
                        }
                    },
                    PrintOnFirstPage = "true",
                    PrintOnLastPage = "true"
                })
            .WithPageFooter(new PageFooter
            {
                Height = "14pt",
                ReportItems = new ReportItemsFooter
                {
                    Textbox = new Textbox
                    {
                        Name = "Footer",
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
            })
            .WithBody("36pt")
            .WithReportText(new Textbox
            {
                Name = "Textbox1",
                Top = ".1in",
                Left = ".1in",
                Width = "6in",
                Height = ".25in",
                Value = new Value { Text = "Text Area 1" },
                Style = new Style { FontSize = "12pt", FontWeight = "Bold" }
            })
            .WithReportText(new Textbox
            {
                Name = "Textbox2",
                Top = "1in",
                Left = "1in",
                Width = "6in",
                Height = "4in",
                Value = new Value { Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum." },
                Style = new Style
                {
                    FontSize = "12pt",
                    BackgroundColor = "gray"
                }
            });

            return report;
        }

    }
}
