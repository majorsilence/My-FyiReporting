using System.Xml.Serialization;


namespace Majorsilence.Reporting.RdlCreator
{
    public class ReportItemsFooter
    {
        [XmlElement(ElementName = "Textbox")]
        public Textbox Textbox { get; set; }

    }
}