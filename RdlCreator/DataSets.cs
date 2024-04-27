using System.Xml.Serialization;


namespace fyiReporting.RdlCreator
{
    public class DataSets
    {
        [XmlElement(ElementName = "DataSet")]
        public DataSet DataSet { get; set; }
    }
}