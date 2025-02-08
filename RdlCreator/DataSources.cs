using System.Xml.Serialization;


namespace Majorsilence.Reporting.RdlCreator
{
    public class DataSources
    {
        [XmlElement(ElementName = "DataSource")]
        public DataSource DataSource { get; set; }
    }
}