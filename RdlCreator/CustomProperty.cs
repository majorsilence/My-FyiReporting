using System.Xml.Serialization;

namespace Majorsilence.Reporting.RdlCreator
{
    public class CustomProperty
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "Value")]
        public string Value { get; set; }
    }
}