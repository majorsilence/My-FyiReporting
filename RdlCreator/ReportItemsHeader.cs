using System.Xml.Serialization;


namespace Majorsilence.Reporting.RdlCreator
{
    public class ReportItemsHeader
    {
        [XmlElement(ElementName = "Textbox")]
        public Text Textbox { get; set; }

    }
}