using SkiaSharp;


namespace Majorsilence.Drawing
{
    // Compatibility wrapper for System.Drawing.Font
    public class Font : IDisposable
    {
        private SKTypeface _typeface;
        private SKFont _paint;

        // Properties for Family, Size, Style
        public string FontFamily { get; }
        public float Size { get; }
        public FontStyle Style { get; }

        public Font(string fontFamily, float size)
            : this(fontFamily, size, FontStyle.Regular)
        {
        }

        // Constructor to initialize the Font with family, size, and style
        public Font(string fontFamily, float size, FontStyle style)
        {
            FontFamily = fontFamily;
            Size = size;
            Style = style;

            // Determine SkiaSharp style flags based on FontStyle

            var typefaceStyle = SKFontStyle.Normal;
            if ((style & FontStyle.Bold) != 0 && (style & FontStyle.Italic) != 0)
                typefaceStyle = SKFontStyle.BoldItalic;
            else if ((style & FontStyle.Bold) != 0)
                typefaceStyle = SKFontStyle.Bold;
            else if ((style & FontStyle.Italic) != 0)
                typefaceStyle = SKFontStyle.Italic;

            // Create the SkiaSharp Typeface based on the family and style
            _typeface = SKTypeface.FromFamilyName(fontFamily, typefaceStyle);

            // Initialize the SKFont for text rendering
            _paint = new SKFont(_typeface, size);

        }

        public Font(Drawing.FontFamily fontFamily, float size, FontStyle style)
            : this(fontFamily.Name, size, style)
        {
        }

        public double GetHeight(Graphics g)
        {
            if (g == null)
            {
                throw new ArgumentNullException(nameof(g));
            }

            // Measure the height of a sample text to determine the font height
            using (var paint = new SKFont(_typeface, Size))
            {
                var metrics = paint.Metrics;
                return Math.Ceiling(metrics.Descent - metrics.Ascent);
            }
        }

        // Property to get SKPaint (useful for drawing text)
        public SKFont ToSkFont()
        {
            return _paint;
        }

        // Dispose method to release resources
        public void Dispose()
        {
            _paint?.Dispose();
            _typeface?.Dispose();
        }

        // Override ToString to show font details
        public override string ToString()
        {
            return $"{FontFamily} {Size}pt {Style}";
        }

    }

}