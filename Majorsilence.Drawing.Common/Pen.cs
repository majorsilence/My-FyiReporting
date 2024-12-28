using Majorsilence.Drawing.Drawing2D;
using Majorsilence.Drawing.Imaging;
using SkiaSharp;
using System.Drawing;


namespace Majorsilence.Drawing
{

    // Compatibility wrapper for Pen
    public class Pen : IDisposable
    {
        private SKPaint _paint;

        public Pen(Color color)
        {
            _paint = new SKPaint
            {
                Color = new SKColor((byte)color.R, (byte)color.G, (byte)color.B, (byte)color.A),
                StrokeWidth = 1,
                Style = SKPaintStyle.Stroke
            };
        }

        public Pen(Brush brush)
        {
            // FIXME; what does brush do?
            _paint = new SKPaint
            {
                StrokeWidth = 1,
                Style = SKPaintStyle.Stroke
            };
        }

        public Pen(Color color, float width)
        {
            _paint = new SKPaint
            {
                Color = new SKColor((byte)color.R, (byte)color.G, (byte)color.B, (byte)color.A),
                StrokeWidth = width,
                Style = SKPaintStyle.Stroke
            };
        }

        public Pen(Brush brush, float width)
        {
            // FIXME; what does brush do?
            _paint = new SKPaint
            {
                StrokeWidth = width,
                Style = SKPaintStyle.Stroke
            };
        }
        public LineJoin LineJoin { get; set; }
        public LineCap LineCap { get; set; }
        public Brush Brush { get; set; }
        public Drawing2D.DashStyle DashStyle { get; set; }

        public void Dispose()
        {
            _paint?.Dispose();
        }

        // Convert Pen to SKPaint
        public SKPaint ToSkPaint()
        {
            _paint.StrokeJoin = LineJoin switch
            {
                LineJoin.Miter => SKStrokeJoin.Miter,
                LineJoin.Round => SKStrokeJoin.Round,
                LineJoin.Bevel => SKStrokeJoin.Bevel,
                _ => _paint.StrokeJoin
            };

            _paint.StrokeCap = LineCap switch
            {
                LineCap.Flat => SKStrokeCap.Butt,
                LineCap.Square => SKStrokeCap.Square,
                LineCap.Round => SKStrokeCap.Round,
                _ => _paint.StrokeCap
            };

            return _paint;
        }
    }



}
