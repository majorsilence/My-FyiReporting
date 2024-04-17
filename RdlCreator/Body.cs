using System.Collections.Generic;
using System.Xml.Serialization;
using System;


namespace RdlCreator
{

    public class Body
    {
        [XmlElement(ElementName = "ReportItems")]
        public ReportItemsBody ReportItems { get; set; }

        [XmlElement(ElementName = "Height")]
        public string Height { get; set; }
    }
}