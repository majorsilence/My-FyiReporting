using System.Xml.Serialization;


namespace fyiReporting.RdlCreator
{
    public class DataSet
    {
        [XmlElement(ElementName = "Query")]
        public Query Query { get; set; }

        [XmlElement(ElementName = "Fields")]
        public Fields Fields { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }
}