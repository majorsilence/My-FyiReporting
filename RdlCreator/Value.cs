using System.Xml.Serialization;


namespace Majorsilence.Reporting.RdlCreator
{
    public class Value
    {
        [XmlText]
        public string Text { get; set; }
    }
}