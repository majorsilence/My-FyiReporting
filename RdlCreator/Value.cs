using System.Xml.Serialization;


namespace RdlCreator
{
    public class Value
    {
        [XmlText]
        public string Text { get; set; }
    }
}