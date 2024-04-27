using System.Xml.Serialization;


namespace fyiReporting.RdlCreator
{
    public class TableRows
    {
        [XmlElement(ElementName = "TableRow")]
        public TableRow TableRow { get; set; }
    }
}