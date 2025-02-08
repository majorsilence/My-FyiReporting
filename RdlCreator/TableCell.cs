using System.Xml.Serialization;

namespace Majorsilence.Reporting.RdlCreator
{

    public class TableCell
    {
        [XmlElement(ElementName = "ReportItems")]
        public TableCellReportItems ReportItems { get; set; }
    }
}