using Majorsilence.Drawing.Drawing2D;
using SkiaSharp;


namespace Majorsilence.Drawing
{
    // Compatibility wrapper for Color
    public class Color
    {
        public static Color Black => new Color(0, 0, 0);
        public static Color White => new Color(255, 255, 255);
        public static Color Red => new Color(255, 0, 0);
        public static Color Green => new Color(0, 255, 0);
        public static Color Blue => new Color(0, 0, 255);
        public static Color Yellow => new Color(255, 255, 0);
        public static Color Cyan => new Color(0, 255, 255);
        public static Color Magenta => new Color(255, 0, 255);
        public static Color Transparent => new Color(0, 0, 0, 0);
        public static Color Orange => new Color(255, 165, 0);
        public static Color Purple => new Color(128, 0, 128);
        public static Color Brown => new Color(165, 42, 42);
        public static Color Pink => new Color(255, 192, 203);
        public static Color Lime => new Color(0, 255, 0);
        public static Color Gray => new Color(128, 128, 128);
        public static Color Navy => new Color(0, 0, 128);
        public static Color Olive => new Color(128, 128, 0);
        public static Color Teal => new Color(0, 128, 128);
        public static Color Silver => new Color(192, 192, 192);
        public static Color Empty => new Color(0, 0, 0, 0);
        public static Color Maroon => new Color(128, 0, 0);
        public static Color Chocolate => new Color(210, 105, 30);
        public static Color IndianRed => new Color(205, 92, 92);
        public static Color Peru => new Color(205, 133, 63);
        public static Color BurlyWood => new Color(222, 184, 135);
        public static Color AntiqueWhite => new Color(250, 235, 215);
        public static Color FloralWhite => new Color(255, 250, 240);
        public static Color Ivory => new Color(255, 255, 240);
        public static Color LightCoral => new Color(240, 128, 128);
        public static Color DarkSalmon => new Color(233, 150, 122);
        public static Color LightSalmon => new Color(255, 160, 122);
        public static Color PeachPuff => new Color(255, 218, 185);
        public static Color NavajoWhite => new Color(255, 222, 173);
        public static Color Moccasin => new Color(255, 228, 181);
        public static Color PapayaWhip => new Color(255, 239, 213);
        public static Color Goldenrod => new Color(218, 165, 32);
        public static Color DarkGoldenrod => new Color(184, 134, 11);
        public static Color DarkKhaki => new Color(189, 183, 107);
        public static Color Khaki => new Color(240, 230, 140);
        public static Color Beige => new Color(245, 245, 220);
        public static Color Cornsilk => new Color(255, 248, 220);
        public static Color DeepSkyBlue => new Color(0, 191, 255);
        public static Color Gold => new Color(255, 215, 0);


        public static Color SlateGray => new Color(112, 128, 144);
        public static Color DarkGray => new Color(169, 169, 169);
        public static Color LightGray => new Color(211, 211, 211);
        public static Color DarkSlateGray => new Color(47, 79, 79);
        public static Color DimGray => new Color(105, 105, 105);
        public static Color LightSlateGray => new Color(119, 136, 153);
        public static Color Gainsboro => new Color(220, 220, 220);
        public static Color LightBlue => new Color(173, 216, 230);
        public static Color LightCyan => new Color(224, 255, 255);
        public static Color LightGoldenrodYellow => new Color(250, 250, 210);
        public static Color LightGreen => new Color(144, 238, 144);
        public static Color LightPink => new Color(255, 182, 193);
        public static Color LightSeaGreen => new Color(32, 178, 170);
        public static Color LightSkyBlue => new Color(135, 206, 250);
        public static Color LightSteelBlue => new Color(176, 196, 222);
        public static Color LightYellow => new Color(255, 255, 224);


        public static Color CadetBlue => new Color(95, 158, 160);
        public static Color MediumTurquoise => new Color(72, 209, 204);
        public static Color Aquamarine => new Color(127, 255, 212);
        public static Color Azure => new Color(240, 255, 255);
        public static Color AliceBlue => new Color(240, 248, 255);
        public static Color MintCream => new Color(245, 255, 250);
        public static Color DarkSeaGreen => new Color(143, 188, 143);
        public static Color PaleGreen => new Color(152, 251, 152);
        public static Color MediumPurple => new Color(147, 112, 219);
        public static Color CornflowerBlue => new Color(100, 149, 237);
        public static Color Lavender => new Color(230, 230, 250);
        public static Color GhostWhite => new Color(248, 248, 255);
        public static Color PaleGoldenrod => new Color(238, 232, 170);
        public static Color LemonChiffon => new Color(255, 250, 205);
        public static Color Orchid => new Color(218, 112, 214);
        public static Color Plum => new Color(221, 160, 221);
        public static Color LavenderBlush => new Color(255, 240, 245);
        public static Color Linen => new Color(250, 240, 230);
        public static Color PaleTurquoise => new Color(175, 238, 238);
        public static Color OldLace => new Color(253, 245, 230);


        public byte R { get; }
        public byte G { get; }
        public byte B { get; }
        public byte A { get; }

        public bool IsEmpty => R == 0 && G == 0 && B == 0 && A == 0;


        public Color(int r, int g, int b, int a = 255)
        {
            R = (byte)r;
            G = (byte)g;
            B = (byte)b;
            A = (byte)a;
        }

        public Color(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public static Color FromName(string name)
        {
            switch (name.ToLower())
            {
                case "black": return Black;
                case "white": return White;
                case "red": return Red;
                case "green": return Green;
                case "blue": return Blue;
                case "yellow": return Yellow;
                case "cyan": return Cyan;
                case "magenta": return Magenta;
                case "transparent": return Transparent;
                case "orange": return Orange;
                case "purple": return Purple;
                case "brown": return Brown;
                case "pink": return Pink;
                case "lime": return Lime;
                case "gray": return Gray;
                case "navy": return Navy;
                case "olive": return Olive;
                case "teal": return Teal;
                case "silver": return Silver;
                default: throw new ArgumentException($"Unknown color name: {name}");
            }
        }

        // Convert to SkiaSharp's SKColor
        public SKColor ToSkColor()
        {
            return new SKColor((byte)R, (byte)G, (byte)B, (byte)A);
        }

//#if !DRAWINGCOMPAT
        public static implicit operator System.Drawing.Color(Color p)
        {
            return System.Drawing.Color.FromArgb(p.A, p.R, p.G, p.B);
        }
//#endif
    }

}
