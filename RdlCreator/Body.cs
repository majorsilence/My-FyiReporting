using System.Collections.Generic;
using System.Xml.Serialization;
using System;


namespace Majorsilence.Reporting.RdlCreator
{

    public class Body
    {
        [XmlElement(ElementName = "ReportItems")]
        public ReportItemsBody ReportItems { get; set; }

        [XmlElement(ElementName = "Height")]
        public string Height { get; set; }

        public Table WithReportItems()
        {
            this.ReportItems = new ReportItemsBody()
            {
                Table = new Table()
            };
            return ReportItems.Table;
        }

        public Body WithHeight(string height)
        {
            this.Height = height;
            return this;
        }
    }
}