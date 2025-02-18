using System;
using System.Xml.Serialization;


namespace Majorsilence.Reporting.RdlCreator
{
    public class Card
    {
        private readonly Report _rpt = null;

        public Card() { }
        public Card(Report rpt)
        {
            _rpt = rpt;
        }

        [XmlElement(ElementName = "Value")]
        public Value Value { get; set; }

        [XmlElement(ElementName = "Style")]
        public Style Style { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "Top")]
        public string Top { get; set; }

        [XmlElement(ElementName = "Left")]
        public string Left { get; set; }

        [XmlElement(ElementName = "Width")]
        public string Width { get; set; }

        [XmlElement(ElementName = "Height")]
        public string Height { get; set; }

        [XmlElement(ElementName = "CanGrow")]
        public bool CanGrow { get; set; }

        public bool PageBreakAtEnd { get; set; }

        public bool PageBreakAtStart { get; set; }

        public void Add(Text text)
        {
            // Parse the Card's dimensions
            float cardTop = float.Parse(this.Top);
            float cardLeft = float.Parse(this.Left);
            float cardWidth = float.Parse(this.Width);
            float cardHeight = float.Parse(this.Height);

            // Parse the Text's dimensions
            float textTop = float.Parse(text.Top);
            float textLeft = float.Parse(text.Left);
            float textWidth = float.Parse(text.Width);
            float textHeight = float.Parse(text.Height);

            // Calculate the relative position of the Text
            float relativeTop = cardTop + textTop;
            float relativeLeft = cardLeft + textLeft;

            // Set the Text's position relative to the Card
            text.Top = relativeTop.ToString();
            text.Left = relativeLeft.ToString();

            _rpt?.Body?.ReportItems?.Text?.Add(text);
        }

        public void Add(CustomReportItems cri)
        {
            // Parse the Card's dimensions
            float cardTop = float.Parse(this.Top);
            float cardLeft = float.Parse(this.Left);
            float cardWidth = float.Parse(this.Width);
            float cardHeight = float.Parse(this.Height);

            // Parse the Text's dimensions
            float textTop = float.Parse(cri.Top);
            float textLeft = float.Parse(cri.Left);
            float textWidth = float.Parse(cri.Width);
            float textHeight = float.Parse(cri.Height);

            // Calculate the relative position of the Text
            float relativeTop = cardTop + textTop;
            float relativeLeft = cardLeft + textLeft;

            // Set the Text's position relative to the Card
            cri.Top = relativeTop.ToString();
            cri.Left = relativeLeft.ToString();

            _rpt?.Body?.ReportItems?.CustomReportItems?.Add(cri);
        }
    }
}