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

        public Table WithReportTable()
        {
            this.ReportItems = new ReportItemsBody()
            {
                Table = new Table(),
                Text = new List<Textbox>()
            };
            return ReportItems.Table;
        }

        public Body WithReportText(Textbox textbox)
        {
            if (this.ReportItems == null)
            {
                this.ReportItems = new ReportItemsBody()
                {
                    Text = new List<Textbox>()
                };

                this.ReportItems.Text.Add(textbox);
            }
            else
            {
                this.ReportItems.Text.Add(textbox);
            }

            return this;
        }

        public Body WithHeight(string height)
        {
            this.Height = height;
            return this;
        }
    }
}