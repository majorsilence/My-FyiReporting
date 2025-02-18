using System.Xml.Serialization;


namespace Majorsilence.Reporting.RdlCreator
{
    public class ReportItemsFooter
    {
        [XmlElement(ElementName = "Textbox")]
        public Text Textbox { get; set; }

    }
}