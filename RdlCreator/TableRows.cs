using System.Xml.Serialization;


namespace Majorsilence.Reporting.RdlCreator
{
    public class TableRows
    {
        [XmlElement(ElementName = "TableRow")]
        public TableRow TableRow { get; set; }
    }
}