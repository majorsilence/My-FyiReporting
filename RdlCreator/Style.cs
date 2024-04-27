using System.Xml.Serialization;


namespace fyiReporting.RdlCreator
{
    public class Style
    {
        [XmlElement(ElementName = "FontSize")]
        public string FontSize { get; set; }

        [XmlElement(ElementName = "FontWeight")]
        public string FontWeight { get; set; }

        [XmlElement(ElementName = "TextAlign")]
        public string TextAlign { get; set; }
    }
}