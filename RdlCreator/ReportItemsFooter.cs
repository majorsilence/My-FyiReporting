using System.Xml.Serialization;


namespace fyiReporting.RdlCreator
{
    public class ReportItemsFooter
    {
        [XmlElement(ElementName = "Textbox")]
        public Textbox Textbox { get; set; }

    }
}