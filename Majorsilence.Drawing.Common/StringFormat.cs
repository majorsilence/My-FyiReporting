namespace Majorsilence.Drawing
{

    public class StringFormat : IDisposable
    {
        public StringAlignment Alignment { get; set; }
        public StringAlignment LineAlignment { get; set; }
        public StringFormatFlags FormatFlags { get; set; }
        public StringTrimming Trimming { get; set; }

        public static readonly StringFormat GenericTypographic = new StringFormat
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Near,
            FormatFlags = StringFormatFlags.MeasureTrailingSpaces,
            Trimming = StringTrimming.None
        };

        public StringFormat()
        {
            Alignment = StringAlignment.Near;
            LineAlignment = StringAlignment.Near;
            FormatFlags = StringFormatFlags.None;
            Trimming = StringTrimming.None;
        }

        public void SetMeasurableCharacterRanges(CharacterRange[] ranges)
        {
            // Assuming we need to store the ranges in a private field
            MeasurableCharacterRanges = ranges;
        }

        // Add a private field to store the character ranges
        public CharacterRange[] MeasurableCharacterRanges { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
        }
    }
}
