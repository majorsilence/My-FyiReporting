using System.Xml.Serialization;

namespace Majorsilence.Reporting.RdlCreator
{
    public class BorderStyle
    {
        [XmlElement(ElementName = "Default")]
        public string Default { get; set; }

        [XmlElement(ElementName = "Left")]
        public string Left { get; set; }

        [XmlElement(ElementName = "Right")]
        public string Right { get; set; }

        [XmlElement(ElementName = "Top")]
        public string Top { get; set; }

        [XmlElement(ElementName = "Bottom")]
        public string Bottom { get; set; }

        public static class Values
        {
            public const string None = "None";
            public const string Solid = "Solid";
            public const string Dashed = "Dashed";
        }
    }
}