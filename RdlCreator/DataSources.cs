using System.Xml.Serialization;


namespace RdlCreator
{
    public class DataSources
    {
        [XmlElement(ElementName = "DataSource")]
        public DataSource DataSource { get; set; }
    }
}