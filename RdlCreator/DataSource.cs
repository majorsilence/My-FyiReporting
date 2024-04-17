using System.Xml.Serialization;


namespace RdlCreator
{
    public class DataSource
    {
        [XmlElement(ElementName = "ConnectionProperties")]
        public ConnectionProperties ConnectionProperties { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }
}