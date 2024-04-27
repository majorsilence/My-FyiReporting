using System.Xml.Serialization;


namespace fyiReporting.RdlCreator
{
    public class Details
    {
        [XmlElement(ElementName = "TableRows")]
        public TableRows TableRows { get; set; }
    }
}