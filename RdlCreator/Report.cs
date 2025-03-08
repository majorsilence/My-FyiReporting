using Majorsilence.Reporting.Rdl;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Serialization;

namespace Majorsilence.Reporting.RdlCreator
{
    // Report class representing the root element
    [XmlRoot(ElementName = "Report", Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition")]
    public class Report
    {
        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "Author")]
        public string Author { get; set; }

        [XmlElement(ElementName = "PageHeight")]
        public string PageHeight { get; set; }

        [XmlElement(ElementName = "PageWidth")]
        public string PageWidth { get; set; }

        [XmlElement(ElementName = "DataSources")]
        public DataSources DataSources { get; set; }

        [XmlElement(ElementName = "Width")]
        public string Width { get; set; }

        [XmlElement(ElementName = "TopMargin")]
        public string TopMargin { get; set; }

        [XmlElement(ElementName = "LeftMargin")]
        public string LeftMargin { get; set; }

        [XmlElement(ElementName = "RightMargin")]
        public string RightMargin { get; set; }

        [XmlElement(ElementName = "BottomMargin")]
        public string BottomMargin { get; set; }

        [XmlElement(ElementName = "DataSets")]
        public DataSets DataSets { get; set; }

        [XmlElement(ElementName = "PageHeader")]
        public PageHeader PageHeader { get; set; }

        [XmlElement(ElementName = "Body")]
        public Body Body { get; set; }

        [XmlElement(ElementName = "PageFooter")]
        public PageFooter PageFooter { get; set; }

        // Fluid style methods

        public Report WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public Report WithAuthor(string author)
        {
            Author = author;
            return this;
        }

        public Report WithPageHeight(string pageHeight)
        {
            PageHeight = pageHeight;
            return this;
        }

        public Report WithPageWidth(string pageWidth)
        {
            PageWidth = pageWidth;
            return this;
        }

        public Report WithDataSources(DataSources dataSources)
        {
            DataSources = dataSources;
            return this;
        }

        public Report WithWidth(string width)
        {
            Width = width;
            return this;
        }

        public Report WithTopMargin(string topMargin)
        {
            TopMargin = topMargin;
            return this;
        }

        public Report WithLeftMargin(string leftMargin)
        {
            LeftMargin = leftMargin;
            return this;
        }

        public Report WithRightMargin(string rightMargin)
        {
            RightMargin = rightMargin;
            return this;
        }

        public Report WithBottomMargin(string bottomMargin)
        {
            BottomMargin = bottomMargin;
            return this;
        }

        public Report WithDataSets(DataSets dataSets)
        {
            DataSets = dataSets;
            return this;
        }

        public Report WithPageHeader(PageHeader pageHeader)
        {
            PageHeader = pageHeader;
            return this;
        }

        public Report WithBody(string height)
        {
            Body = new Body()
            {
                Height = height
            };
            return this;
        }

        public Report WithBody(Body body)
        {
            Body = body;
            return this;
        }

        public Report WithPageBreak(string yPos="1pt")
        {
            var pageBreakCard = new Card(this)
            {
                CanGrow = false,
                Height = "0pt",
                Width = "1pt",
                PageBreakAtEnd = true,
                Top=yPos
            };
            this.Body.ReportItems.Cards.Add(pageBreakCard);
            return this;
        }

        public Report WithPageFooter(PageFooter pageFooter)
        {
            PageFooter = pageFooter;
            return this;
        }

        public Report WithTable()
        {
            InitReportItemBody(true);
            return this;
        }

        public Report WithReportText(Text textbox)
        {
            InitReportItemBody(false);
            this.Body.ReportItems.Text.Add(textbox);

            return this;
        }

        private void InitReportItemBody(bool includeTable)
        {
            if (this.Body.ReportItems == null)
            {
                this.Body.ReportItems = new ReportItemsBody()
                {
                    Text = new List<Text>(),
                    Cards = new List<Card>(),
                    CustomReportItems = new List<CustomReportItems>()
                };
            }

            if (includeTable && this.Body.ReportItems.Table == null)
            {
                this.Body.ReportItems.Table = new Table();
            }
        }

        public Report WithCard(Card card)
        {
            InitReportItemBody(false);

            this.Body.ReportItems.Cards.Add(card);

            return this;
        }

        public Report WithTableColumns(TableColumns tableColumns)
        {
            this.Body.ReportItems.Table.TableColumns = tableColumns;
            return this;
        }

        public Report WithTableHeader(TableRow header, string repeatOnNewPage = "true")
        {
            this.Body.ReportItems.Table.Header = new Header()
            {
                TableRows = new TableRows()
                {
                    TableRow = header
                },
                RepeatOnNewPage = repeatOnNewPage
            };
            return this;
        }

        public Report WithTableDetails(TableRow row)
        {
            this.Body.ReportItems.Table.Details = new Details();
            this.Body.ReportItems.Table.Details.TableRows = new TableRows()
            {
                TableRow = row
            };
            return this;
        }

        public Report WithTableDataSetName(string dataSetName)
        {
            this.Body.ReportItems.Table.DataSetName = dataSetName;
            return this;
        }

        public Report WithTableNoRows(string noRows)
        {
            this.Body.ReportItems.Table.NoRows = noRows;
            return this;
        }

        public Report WithTableName(string tableName)
        {
            this.Body.ReportItems.Table.TableName = tableName;
            return this;
        }

        public Report WithImage(ReportItemImage image)
        {
            InitReportItemBody(false);
            if (this.Body.ReportItems.Images == null)
            {
                this.Body.ReportItems.Images = new List<ReportItemImage>();
            }
            this.Body.ReportItems.Images.Add(image);
            return this;
        }

    }
}