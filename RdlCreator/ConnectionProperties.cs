using System.Xml.Serialization;


namespace fyiReporting.RdlCreator
{
    public class ConnectionProperties
    {
        [XmlElement(ElementName = "DataProvider")]
        public string DataProvider { get; set; }

        [XmlElement(ElementName = "ConnectString")]
        public string ConnectString { get; set; }
    }
}