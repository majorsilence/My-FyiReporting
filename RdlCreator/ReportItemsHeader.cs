using System.Xml.Serialization;


namespace RdlCreator
{
    public class ReportItemsHeader
    {
        [XmlElement(ElementName = "Textbox")]
        public Textbox Textbox { get; set; }

    }
}