using SkiaSharp;


namespace Majorsilence.Drawing
{
    public class Text
    {
        public string Content { get; set; }
        public Color Color { get; set; }
        public float Size { get; set; }
        public string FontFamily { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public enum TextRenderingHint
        {
            SystemDefault,
            SingleBitPerPixel,
            AntiAlias,
            ClearTypeGridFit
        }

        public Text(string content, Color color, float size, string fontFamily = "Arial", bool bold = false, bool italic = false)
        {
            Content = content;
            Color = color;
            Size = size;
            FontFamily = fontFamily;
            Bold = bold;
            Italic = italic;
        }

        public SKFont ToSkFont()
        {
            return new SKFont(
                SKTypeface.FromFamilyName(FontFamily, Bold ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal,
                    SKFontStyleWidth.Normal, Italic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright),
                Size, 1F, 0F);
        }
    }

}
