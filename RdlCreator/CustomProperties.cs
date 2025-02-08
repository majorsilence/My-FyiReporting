using System.Xml.Serialization;

namespace fyiReporting.RdlCreator
{

    public class CustomProperties
    {
        [XmlElement(ElementName = "CustomProperty")]
        public CustomProperty CustomProperty { get; set; }
    }
}