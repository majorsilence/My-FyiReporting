using System.Xml.Serialization;


namespace RdlCreator
{
    public class DataSets
    {
        [XmlElement(ElementName = "DataSet")]
        public DataSet DataSet { get; set; }
    }
}