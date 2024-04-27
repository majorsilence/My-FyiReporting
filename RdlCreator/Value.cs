using System.Xml.Serialization;


namespace fyiReporting.RdlCreator
{
    public class Value
    {
        [XmlText]
        public string Text { get; set; }
    }
}