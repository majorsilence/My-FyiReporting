using SkiaSharp;


namespace Majorsilence.Drawing
{
    // Compatibility wrapper for Brush (only supports solid color for now)
    public class Brush : IDisposable
    {
        private SKPaint _paint;

        public Brush(Color color)
        {
            _paint = new SKPaint
            {
                Color = new SKColor((byte)color.R, (byte)color.G, (byte)color.B, (byte)color.A),
                Style = SKPaintStyle.Fill
            };
        }

        public void Dispose()
        {
            _paint?.Dispose();
        }

        // Convert Brush to SKPaint
        internal SKPaint ToSkPaint()
        {
            return _paint;
        }
    }

}
