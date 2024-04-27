using System.Xml.Serialization;


namespace fyiReporting.RdlCreator
{
    public class PageFooter
    {
        [XmlElement(ElementName = "Height")]
        public string Height { get; set; }

        [XmlElement(ElementName = "ReportItems")]
        public ReportItemsFooter ReportItems { get; set; }

        [XmlElement(ElementName = "PrintOnFirstPage")]
        public string PrintOnFirstPage { get; set; }

        [XmlElement(ElementName = "PrintOnLastPage")]
        public string PrintOnLastPage { get; set; }
    }
}