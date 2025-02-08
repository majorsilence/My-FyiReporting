using System.Xml.Serialization;

namespace Majorsilence.Reporting.RdlCreator
{
    public class TableCellReportItems
    {
        [XmlElement("Textbox", typeof(Textbox))]
        [XmlElement("CustomReportItem", typeof(CustomReportItems))]
        public object ReportItem { get; set; }
    }
}