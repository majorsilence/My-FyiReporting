using System.Xml.Serialization;

namespace fyiReporting.RdlCreator
{

    public class TableCell
    {
        [XmlElement(ElementName = "ReportItems")]
        public TableCellReportItems ReportItems { get; set; }
    }
}