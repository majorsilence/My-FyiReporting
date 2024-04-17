using System.Xml.Serialization;


namespace RdlCreator
{
    public class Header
    {
        [XmlElement(ElementName = "TableRows")]
        public TableRows TableRows { get; set; }

        [XmlElement(ElementName = "RepeatOnNewPage")]
        public string RepeatOnNewPage { get; set; }
    }
}