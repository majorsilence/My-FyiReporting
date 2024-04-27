using System.Xml.Serialization;


namespace fyiReporting.RdlCreator
{
    public class ReportItemsHeader
    {
        [XmlElement(ElementName = "Textbox")]
        public Textbox Textbox { get; set; }

    }
}