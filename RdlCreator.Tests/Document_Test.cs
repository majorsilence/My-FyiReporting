using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System;
using Majorsilence.Reporting.RdlCreator;
using NUnit.Framework;
using System.Text.RegularExpressions;
using Microsoft.Identity.Client;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using System.Linq;

namespace Majorsilence.Reporting.RdlCreator.Tests
{
    [TestFixture]
    public class Document_Test
    {
        [Test]
        public async Task SinglePagePdfDiskExport()
        {
            var document = GenerateData(1);
            var pdfBytes = await document.Create();

            using var fileStream = new FileStream("SinglePagePdf.pdf", FileMode.Create, FileAccess.Write);
            fileStream.Write(pdfBytes);
            await fileStream.DisposeAsync();

            using var pdfDocument = PdfDocument.Open("SinglePagePdf.pdf");
            var text = string.Join(" ", pdfDocument.GetPages().SelectMany(page => page.GetWords()).Select(word => word.Text));

            Assert.That(text, Is.Not.Null);
            Assert.That(pdfDocument.NumberOfPages, Is.EqualTo(1));
            Assert.That(text, Is.EqualTo("Test Header Text Area 1 Lorem ipsum. 0"));
        }

        [Test]
        public async Task MultiPagePdfTest()
        {
            var document = GenerateData(2);
            var pdfBytes = await document.Create();

            using var fileStream = new FileStream("MultiPagePdf.pdf", FileMode.Create, FileAccess.Write);
            fileStream.Write(pdfBytes);
            await fileStream.DisposeAsync();

            using var pdfDocument = PdfDocument.Open("MultiPagePdf.pdf");
            var text = string.Join(" ", pdfDocument.GetPages().SelectMany(page => page.GetWords()).Select(word => word.Text));

            Assert.That(text, Is.Not.Null);
            Assert.That(pdfDocument.NumberOfPages, Is.EqualTo(2));
            Assert.That(text, Is.EqualTo("Test Header Text Area 1 Lorem ipsum. 0 Test Header Text Area 1 Lorem ipsum. 1"));
        }

        private RdlCreator.Document GenerateData(int pageCount = 1)
        {
            var document = new Document
            {
                Description = "Sample report",
                Author = "John Doe",
                PageHeight = "11in",
                PageWidth = "8.5in",
                //Width = "7.5in",
                TopMargin = ".25in",
                LeftMargin = ".25in",
                RightMargin = ".25in",
                BottomMargin = ".25in"
            };

            for (int i = 0; i < pageCount; i++)
            {
                document.WithPage((option) =>
                {
                    option.WithHeight("10in")
                    .WithWidth("7.5in")
                    .WithText(new Text
                    {
                        Name = "Textbox1",
                        Top = ".1in",
                        Left = ".1in",
                        Width = "6in",
                        Height = ".25in",
                        Value = new Value { Text = "Text Area 1" },
                        Style = new Style { FontSize = "12pt", FontWeight = "Bold" }
                    })
                    .WithText(new Text
                    {
                        Name = "Textbox2",
                        Top = "1in",
                        Left = "1in",
                        Width = "6in",
                        Height = "4in",
                        Value = new Value { Text = "Lorem ipsum." },
                        Style = new Style
                        {
                            FontSize = "12pt",
                            BackgroundColor = "gray"
                        }
                    });

                    option.WithPageFooter(new PageFooter
                    {
                        Height = "14pt",
                        ReportItems = new ReportItemsFooter
                        {
                            Textbox = new Text
                            {
                                Name = "Footer",
                                Top = "1pt",
                                Left = "10pt",
                                Height = "12pt",
                                Width = "3in",
                                Value = new Value { Text = $"{i}" },
                                Style = new Style { FontSize = "10pt", FontWeight = "Normal" }
                            }
                        },
                        PrintOnFirstPage = "true",
                        PrintOnLastPage = "true"
                    });

                    option.WithPageHeader(
                        new PageHeader
                        {
                            Height = ".5in",
                            ReportItems = new ReportItemsHeader
                            {
                                Textbox = new Text
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
                        });
                });
            }

            return document;
        }
    }
}
