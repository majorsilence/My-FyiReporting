using System.Collections.Generic;
using System.Xml.Serialization;


namespace RdlCreator
{

    public class TableCells
    {
        [XmlElement(ElementName = "TableCell")]
        public List<TableCell> TableCell { get; set; }
    }

    public class TableCell
    {
        [XmlElement(ElementName = "ReportItems")]
        public TableCellReportItems ReportItems { get; set; }
    }

    public class TableCellReportItems
    {
        [XmlElement(ElementName = "Textbox")]
        public Textbox Textbox { get; set; }
    }
}