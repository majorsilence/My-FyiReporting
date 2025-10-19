﻿using System.Collections.Generic;
using System.Xml.Serialization;


namespace Majorsilence.Reporting.RdlCreator
{
    public class TableRow
    {
        [XmlElement(ElementName = "Height")]
        public ReportItemSize Height { get; set; }

        [XmlElement(ElementName = "TableCells")]
        public TableCells TableCells { get; set; }

    }
}