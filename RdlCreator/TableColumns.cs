using System.Collections.Generic;
using System.Xml.Serialization;


namespace Majorsilence.Reporting.RdlCreator
{
    public class TableColumns
    {
        [XmlElement(ElementName = "TableColumn")]
        public List<TableColumn> TableColumn { get; set; }
    }
}