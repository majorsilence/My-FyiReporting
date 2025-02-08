using System.Xml.Serialization;


namespace Majorsilence.Reporting.RdlCreator
{
    public class ReportItemsBody
    {
        [XmlElement(ElementName = "Table")]
        public Table Table { get; set; }

        public ReportItemsBody WithTable(Table table)
        {
            this.Table = table;
            return this;
        }
    }
}