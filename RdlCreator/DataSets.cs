using System.Xml.Serialization;


namespace Majorsilence.Reporting.RdlCreator
{
    public class DataSets
    {
        [XmlElement(ElementName = "DataSet")]
        public DataSet DataSet { get; set; }
    }
}