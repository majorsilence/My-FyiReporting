using System.Collections.Generic;
using System.Xml.Serialization;

namespace fyiReporting.RdlCreator
{
    public class TableCells
    {
        [XmlElement(ElementName = "TableCell")]
        public List<TableCell> TableCell { get; set; }
    }
}