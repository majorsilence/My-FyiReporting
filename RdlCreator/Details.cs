using System.Xml.Serialization;


namespace Majorsilence.Reporting.RdlCreator
{
    public class Details
    {
        [XmlElement(ElementName = "TableRows")]
        public TableRows TableRows { get; set; }
    }
}