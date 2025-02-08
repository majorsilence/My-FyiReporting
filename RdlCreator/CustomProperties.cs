using System.Xml.Serialization;

namespace Majorsilence.Reporting.RdlCreator
{

    public class CustomProperties
    {
        [XmlElement(ElementName = "CustomProperty")]
        public CustomProperty CustomProperty { get; set; }
    }
}