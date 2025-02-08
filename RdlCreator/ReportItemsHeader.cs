using System.Xml.Serialization;


namespace Majorsilence.Reporting.RdlCreator
{
    public class ReportItemsHeader
    {
        [XmlElement(ElementName = "Textbox")]
        public Textbox Textbox { get; set; }

    }
}