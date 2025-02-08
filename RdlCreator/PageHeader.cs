using System.Xml.Serialization;


namespace Majorsilence.Reporting.RdlCreator
{
    public class PageHeader
    {
        [XmlElement(ElementName = "Height")]
        public string Height { get; set; }

        [XmlElement(ElementName = "ReportItems")]
        public ReportItemsHeader ReportItems { get; set; }

        [XmlElement(ElementName = "PrintOnFirstPage")]
        public string PrintOnFirstPage { get; set; }

        [XmlElement(ElementName = "PrintOnLastPage")]
        public string PrintOnLastPage { get; set; }
    }
}