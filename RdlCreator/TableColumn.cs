using System.Xml.Serialization;


namespace fyiReporting.RdlCreator
{
    public class TableColumn
    {
        [XmlElement(ElementName = "Width")]
        public string Width { get; set; }
    }
}