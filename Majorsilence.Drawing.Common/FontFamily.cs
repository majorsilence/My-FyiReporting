namespace Majorsilence.Drawing
{
    public class FontFamily
    {
        public static FontFamily GenericMonospace { get; } = new FontFamily("monospace");
        public static FontFamily GenericSansSerif { get; } = new FontFamily("sans-serif");
        public static FontFamily GenericSerif { get; } = new FontFamily("serif");
        public string Name { get; }
        public FontFamily(string name)
        {
            Name = name;
        }

        public float GetCellDescent(FontStyle fs)
        {
            // Placeholder implementation, replace with actual logic
            switch (fs)
            {
                case FontStyle.Bold:
                    return 2.0f;
                case FontStyle.Italic:
                    return 1.5f;
                case FontStyle.Underline:
                    return 1.0f;
                case FontStyle.Strikeout:
                    return 0.5f;
                default:
                    return 1.0f;
            }
        }

        public float GetEmHeight(FontStyle fs)
        {
            // Placeholder implementation, replace with actual logic
            switch (fs)
            {
                case FontStyle.Bold:
                    return 16.0f;
                case FontStyle.Italic:
                    return 14.0f;
                case FontStyle.Underline:
                    return 12.0f;
                case FontStyle.Strikeout:
                    return 10.0f;
                default:
                    return 12.0f;
            }
        }

        public override string ToString()
        {
            return Name;
        }

    }

}