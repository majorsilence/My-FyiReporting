using System.Xml.Serialization;

namespace Majorsilence.Reporting.RdlCreator
{
    public class CustomReportItems
    {
        public string Type { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Left { get; set; }
        public string Top { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "CustomProperties")]
        public CustomProperties CustomProperties { get; set; }
        [XmlElement(ElementName = "Style")]
        public Style Style { get; set; }
        [XmlElement(ElementName = "CanGrow")]
        public string CanGrow { get; set; }
    }
}