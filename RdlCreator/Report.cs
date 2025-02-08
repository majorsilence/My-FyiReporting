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

        public Body WithBody()
        {
            Body = new Body();
            return Body;
        }

        public Report WithPageFooter(PageFooter pageFooter)
        {
            PageFooter = pageFooter;
            return this;
        }
    }
}