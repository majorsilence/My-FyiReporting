using System.Xml.Serialization;


namespace Majorsilence.Reporting.RdlCreator
{
    public class Header
    {
        [XmlElement(ElementName = "TableRows")]
        public TableRows TableRows { get; set; }

        [XmlElement(ElementName = "RepeatOnNewPage")]
        public string RepeatOnNewPage { get; set; }
    }
}