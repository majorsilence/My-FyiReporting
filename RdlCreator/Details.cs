using System.Xml.Serialization;


namespace RdlCreator
{
    public class Details
    {
        [XmlElement(ElementName = "TableRows")]
        public TableRows TableRows { get; set; }
    }
}