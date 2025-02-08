using System.Xml.Serialization;


namespace Majorsilence.Reporting.RdlCreator
{
    public class Table
    {
        [XmlElement(ElementName = "DataSetName")]
        public string DataSetName { get; set; }

        [XmlElement(ElementName = "NoRows")]
        public string NoRows { get; set; }

        [XmlElement(ElementName = "TableColumns")]
        public TableColumns TableColumns { get; set; }

        [XmlElement(ElementName = "Header")]
        public Header Header { get; set; }

        [XmlElement(ElementName = "Details")]
        public Details Details { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string TableName { get; set; }

        public Table WithTableColumns(TableColumns tableColumns)
        {
            this.TableColumns = tableColumns;
            return this;
        }

        public Table WithHeader(Header header)
        {
            this.Header = header;
            return this;
        }

        public Table WithDetails(Details details)
        {
            this.Details = details;
            return this;
        }

        public Table WithDataSetName(string dataSetName)
        {
            this.DataSetName = dataSetName;
            return this;
        }

        public Table WithNoRows(string noRows)
        {
            this.NoRows = noRows;
            return this;
        }

        public Table WithTableName(string tableName)
        {
            this.TableName = tableName;
            return this;
        }
    }
}