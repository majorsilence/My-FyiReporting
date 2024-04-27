using System.Xml.Serialization;


namespace fyiReporting.RdlCreator
{
    public class ReportItemsBody
    {

        [XmlElement(ElementName = "Table")]
        public Table Table { get; set; }
    }
}