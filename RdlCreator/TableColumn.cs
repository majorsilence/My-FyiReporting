using System.Xml.Serialization;


namespace RdlCreator
{
    public class TableColumn
    {
        [XmlElement(ElementName = "Width")]
        public string Width { get; set; }
    }
}