using System.Collections.Generic;
using System.Xml.Serialization;
using System;


namespace Majorsilence.Reporting.RdlCreator
{

    public class Body
    {
        [XmlElement("ReportItems", typeof(ReportItemsBody))]
        public ReportItemsBody ReportItems { get; set; }

        [XmlElement(ElementName = "Height")]
        public string Height { get; set; }
    }
}