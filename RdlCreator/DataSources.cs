using System.Xml.Serialization;


namespace fyiReporting.RdlCreator
{
    public class DataSources
    {
        [XmlElement(ElementName = "DataSource")]
        public DataSource DataSource { get; set; }
    }
}