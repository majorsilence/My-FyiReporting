using System.Xml.Serialization;


namespace Majorsilence.Reporting.RdlCreator
{
    public class Text
    {
        [XmlElement(ElementName = "Value")]
        public Value Value { get; set; }

        [XmlElement(ElementName = "Style")]
        public Style Style { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "Top")]
        public string Top { get; set; }

        [XmlElement(ElementName = "Left")]
        public string Left { get; set; }

        [XmlElement(ElementName = "Width")]
        public string Width { get; set; }

        [XmlElement(ElementName = "Height")]
        public string Height { get; set; }

        [XmlElement(ElementName = "CanGrow")]
        public bool CanGrow { get; set; }
    }
}