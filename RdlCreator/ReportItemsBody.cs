using System.Xml.Serialization;


namespace Majorsilence.Reporting.RdlCreator
{
    public class ReportItemsBody
    {

        [XmlElement(ElementName = "Table")]
        public Table Table { get; set; }
    }
}