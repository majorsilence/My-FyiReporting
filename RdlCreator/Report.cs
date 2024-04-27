using System.Xml.Serialization;


namespace fyiReporting.RdlCreator
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
    }
}