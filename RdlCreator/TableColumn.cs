using System.Xml.Serialization;


namespace Majorsilence.Reporting.RdlCreator
{
    public class TableColumn
    {
        [XmlElement(ElementName = "Width")]
        public ReportItemSize Width { get; set; }
    }
}