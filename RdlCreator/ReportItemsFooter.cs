using System.Xml.Serialization;


namespace RdlCreator
{
    public class ReportItemsFooter
    {
        [XmlElement(ElementName = "Textbox")]
        public Textbox Textbox { get; set; }

    }
}