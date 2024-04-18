using fyiReporting.RDL;
using iTextSharp.text;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RdlCreator
{
    public class Create
    {
        public fyiReporting.RDL.Report GenerateRdl(Report report)
        {
            // Create a new instance of the Report class
            var serializer = new XmlSerializer(typeof(Report));
            string xml;
            using (var writer = new Utf8StringWriter())
            {
                serializer.Serialize(writer, report);
                xml = writer.ToString();
            }

            var rdlp = new RDLParser(xml);
            var fyiReport = rdlp.Parse();
            return fyiReport;
        }

        public fyiReporting.RDL.Report GenerateRdl(DataTable data,
            string description = "",
            string author = "",
            string pageHeight = "11in",
            string pageWidth = "8.5in",
            string width = "7.5in",
            string topMargin = ".25in",
            string leftMargin = ".25in",
            string rightMargin = ".25in",
            string bottomMargin = ".25in",
            string pageHeaderText = "")
        {
            // Create a new instance of the Report class
            throw new NotImplementedException();
        }

        public fyiReporting.RDL.Report GenerateRdl<T>(IEnumerable<T> data,
            string description = "",
            string author = "",
            string pageHeight = "11in",
            string pageWidth = "8.5in",
            string width = "7.5in",
            string topMargin = ".25in",
            string leftMargin = ".25in",
            string rightMargin = ".25in",
            string bottomMargin = ".25in",
            string pageHeaderText = "")
        {
            // Create a new instance of the Report class
            throw new NotImplementedException();
        }

        public fyiReporting.RDL.Report GenerateRdl(string dataProvider,
            string connectionString,
            string commandText,
            CommandType commandType = CommandType.Text,
            string description = "",
            string author = "",
            string pageHeight = "11in",
            string pageWidth = "8.5in",
            string width = "7.5in",
            string topMargin = ".25in",
            string leftMargin = ".25in",
            string rightMargin = ".25in",
            string bottomMargin = ".25in",
            string pageHeaderText = "")
        {

            var headerTableCells = new List<TableCell>();
            var bodyTableCells = new List<TableCell>();
            var fields = new List<Field>();

            using (var cn = RdlEngineConfig.GetConnection(dataProvider, connectionString))
            {
                cn.ConnectionString = connectionString;
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = commandText;
                    cmd.CommandType = commandType;
                    using (var dr = cmd.ExecuteReader(CommandBehavior.SchemaOnly))
                    {
                        for(int i = 0; i < dr.FieldCount; i++)
                        {
                            string colName = dr.GetName(i);
                            TypeCode colType = Type.GetTypeCode(dr.GetFieldType(i));
                            headerTableCells.Add(new TableCell
                            {
                                ReportItems = new TableCellReportItems()
                                {
                                    Textbox = new Textbox
                                    {
                                        Name = $"TextboxH{colName}",
                                        Value = new Value { Text = colName },
                                        Style = new Style { TextAlign = "Center", FontWeight = "Bold" }
                                    }
                                }
                            });

                            bodyTableCells.Add(new TableCell
                            {
                                ReportItems = new TableCellReportItems()
                                {
                                    Textbox = new Textbox
                                    {
                                        Name = $"TextBoxB{colName}",
                                        Value = new Value { Text = $"=Fields!{colName}.Value" },
                                        CanGrow = "true"
                                    }
                                }
                            });

                            fields.Add(new Field
                            {
                                Name = colName,
                                DataField = colName,
                                TypeName = colType.ToString()
                            });

                        }
                    }

                }
            }

            // Create a new instance of the Report class
            var report = new Report
            {
                Description = description,
                Author = author,
                PageHeight = pageHeight,
                PageWidth = pageWidth,
                Width = width,
                TopMargin = topMargin,
                LeftMargin = leftMargin,
                RightMargin = rightMargin,
                BottomMargin = bottomMargin,
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
                            CommandText = commandText
                        },
                        Fields = new Fields
                        {
                            Field = fields
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
                            Value = new Value { Text = pageHeaderText },
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
                                            TableCell = headerTableCells
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
                                            TableCell = bodyTableCells
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

            var serializer = new XmlSerializer(typeof(Report));
            string xml;
            using (var writer = new Utf8StringWriter())
            {
                serializer.Serialize(writer, report);
                xml = writer.ToString();
            }

            var rdlp = new RDLParser(xml);
            var fyiReport = rdlp.Parse();
            return fyiReport;
        }
    }

    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }

}
