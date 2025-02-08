using System.Xml.Serialization;


namespace Majorsilence.Reporting.RdlCreator
{
    public class Field
    {
        [XmlElement(ElementName = "DataField")]
        public string DataField { get; set; }

        [XmlElement(ElementName = "TypeName", Namespace = "rd")]
        public string TypeName { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }
}