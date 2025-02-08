using System.Xml.Serialization;

namespace fyiReporting.RdlCreator
{
    public class TableCellReportItems
    {
        [XmlElement("Textbox", typeof(Textbox))]
        [XmlElement("CustomReportItem", typeof(CustomReportItems))]
        public object ReportItem { get; set; }
    }
}