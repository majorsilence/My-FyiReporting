using System.Xml.Serialization;


namespace RdlCreator
{
    public class ReportItemsBody
    {

        [XmlElement(ElementName = "Table")]
        public Table Table { get; set; }
    }
}