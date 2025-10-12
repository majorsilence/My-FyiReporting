using System.Drawing;
using System.Security.Cryptography;
using SkiaSharp;


namespace Majorsilence.Drawing
{
    // Compatibility wrapper for Graphics
    public class Graphics : IDisposable
    {
        private SKCanvas _canvas;

        public Text.TextRenderingHint TextRenderingHint { get; set; }
        public Drawing.Drawing2D.InterpolationMode InterpolationMode { get; set; }
        public Drawing.Drawing2D.SmoothingMode SmoothingMode { get; set; }
        public Drawing.Drawing2D.PixelOffsetMode PixelOffsetMode { get; set; }
        public Drawing.Drawing2D.CompositingQuality CompositingQuality { get; set; }
        public Drawing.GraphicsUnit PageUnit { get; set; }
        public float DpiX { get; set; } = 96;
        public float DpiY { get; set; } = 96;
        public Drawing.Drawing2D.Matrix Transform { get; set; }

        public Graphics(SKCanvas canvas)
        {
            _canvas = canvas;
        }

        // Clear method

        // Draw rectangle method
        public void DrawRectangle(Pen pen, Rectangle rectangle)
        {
            _canvas.DrawRect(new SKRect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height), pen.ToSkPaint());
        }

        public void FillRectangle(Brush brush, Rectangle rectangle)
        {
            _canvas.DrawRect(new SKRect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height), brush.ToSkPaint());
        }

        public void FillRectangle(Brush brush, int x, int y, int width, int height)
        {
            _canvas.DrawRect(new SKRect(x, y, x + width, y + height), brush.ToSkPaint());
        }

        public void FillRectangle(Brush brush, float x, float y, float width, float height)
        {
            _canvas.DrawRect(new SKRect(x, y, x + width, y + height), brush.ToSkPaint());
        }

        public void FillEllipse(Brush brush, int x, int y, int width, int height)
        {
            _canvas.DrawOval(new SKRect(x, y, x + width, y + height), brush.ToSkPaint());
        }
        public void FillEllipse(Brush brush, Point point)
        {
            _canvas.DrawOval(new SKRect(point.X, point.Y, point.X + 1, point.Y + 1), brush.ToSkPaint());
        }

        public void FillEllipse(Brush brush, Rectangle r)
        {
            _canvas.DrawOval(new SKRect(r.X, r.Y, r.X + 1, r.Y + 1), brush.ToSkPaint());
        }

        public void FillPolygon(Brush b, PointF[] points)
        {
            if (points == null || points.Length < 3)
            {
                throw new ArgumentException("Polygon must have at least 3 points", nameof(points));
            }

            var skPoints = points.Select(p => new SKPoint(p.X, p.Y)).ToArray();
            using (var path = new SKPath())
            {
                path.AddPoly(skPoints, true);
                _canvas.DrawPath(path, b.ToSkPaint());
            }
        }

        public void DrawEllipse(Pen pen, Point p)
        {
            _canvas.DrawOval(new SKRect(p.X, p.Y, p.X + 1, p.Y + 1), pen.ToSkPaint());
        }

        public void DrawEllipse(Pen pen, Rectangle r)
        {
            _canvas.DrawOval(new SKRect(r.X, r.Y, r.X + 1, r.Y + 1), pen.ToSkPaint());
        }

        public void DrawPolygon(Pen pen, PointF[] points)
        {
            if (points == null || points.Length < 3)
            {
                throw new ArgumentException("Polygon must have at least 3 points", nameof(points));
            }
            var skPoints = points.Select(p => new SKPoint(p.X, p.Y)).ToArray();
            using (var path = new SKPath())
            {
                path.AddPoly(skPoints, true);
                _canvas.DrawPath(path, pen.ToSkPaint());
            }
        }

        public void DrawRectangle(Pen pen, int x, int y, int width, int height)
        {
            _canvas.DrawRect(new SKRect(x, y, x + width, y + height), pen.ToSkPaint());
        }

        public void DrawCurve(Pen pen, Point[] points, float tension)
        {
            if (points == null || points.Length < 2)
            {
                throw new ArgumentException("At least two points are required to draw a curve", nameof(points));
            }

            var skPoints = points.Select(p => new SKPoint(p.X, p.Y)).ToArray();
            using (var path = new SKPath())
            {
                path.MoveTo(skPoints[0]);
                for (int i = 1; i < skPoints.Length - 1; i++)
                {
                    var p0 = skPoints[i - 1];
                    var p1 = skPoints[i];
                    var p2 = skPoints[i + 1];

                    var controlPoint1 = new SKPoint(p0.X + (p1.X - p0.X) * tension, p0.Y + (p1.Y - p0.Y) * tension);
                    var controlPoint2 = new SKPoint(p1.X - (p2.X - p0.X) * tension, p1.Y - (p2.Y - p0.Y) * tension);

                    path.CubicTo(controlPoint1, controlPoint2, p1);
                }
                path.LineTo(skPoints.Last());
                _canvas.DrawPath(path, pen.ToSkPaint());
            }
        }

        public void FillPie(Brush brush, Rectangle r, float startAngle, float endAngle)
        {
            using (var path = new SKPath())
            {
                path.MoveTo(r.X + r.Width / 2, r.Y + r.Height / 2);
                path.ArcTo(new SKRect(r.X, r.Y, r.X + r.Width, r.Y + r.Height), startAngle, endAngle - startAngle, false);
                path.Close();
                _canvas.DrawPath(path, brush.ToSkPaint());
            }
        }

        public void DrawPie(Pen p, Rectangle r, float startAngle, float endAngle)
        {
            using (var path = new SKPath())
            {
                path.MoveTo(r.X + r.Width / 2, r.Y + r.Height / 2);
                path.ArcTo(new SKRect(r.X, r.Y, r.X + r.Width, r.Y + r.Height), startAngle, endAngle - startAngle, false);
                path.Close();
                _canvas.DrawPath(path, p.ToSkPaint());
            }
        }

        public void DrawLines(Pen p, Point[] points)
        {
            if (points == null || points.Length < 2)
            {
                throw new ArgumentException("At least two points are required to draw lines", nameof(points));
            }
            var skPoints = points.Select(p => new SKPoint(p.X, p.Y)).ToArray();
            _canvas.DrawPoints(SKPointMode.Lines, skPoints, p.ToSkPaint());
        }

        // Draw line method
        public void DrawLine(Pen pen, Point pt1, Point pt2)
        {
            _canvas.DrawLine(pt1.X, pt1.Y, pt2.X, pt2.Y, pen.ToSkPaint());
        }

        // Draw string method (you would also need to implement font compatibility here)
        public void DrawString(string s, Font font, Brush brush, PointF point)
        {
            var skPaint = brush.ToSkPaint();
            _canvas.DrawText(s, point.X, point.Y, font.ToSkFont(), skPaint);
        }

        public void DrawString(string s, Font font, Brush brush, Rectangle layoutRectangle, StringFormat format)
        {
            var skFont = font.ToSkFont();
            var skPaint = brush.ToSkPaint();
            float lineHeight = skFont.Metrics.Descent - skFont.Metrics.Ascent + skFont.Metrics.Leading;
            float x = layoutRectangle.X;
            float y = layoutRectangle.Y;
            float maxWidth = layoutRectangle.Width;

            var lines = s.Replace("\r\n", "\n").Split('\n');
            foreach (var line in lines)
            {
                var words = line.Split(' ');
                string currentLine = "";
                foreach (var word in words)
                {
                    string testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
                    var bounds = new SKRect();
                    skFont.MeasureText(testLine, out bounds);

                    if (maxWidth > 0 && bounds.Width > maxWidth)
                    {
                        // Draw currentLine
                        if (!string.IsNullOrEmpty(currentLine))
                        {
                            float drawX = x;
                            if (format.Alignment == StringAlignment.Center)
                                drawX += (maxWidth - bounds.Width) / 2;
                            else if (format.Alignment == StringAlignment.Far)
                                drawX += maxWidth - bounds.Width;

                            _canvas.DrawText(currentLine, drawX, y - skFont.Metrics.Ascent, skFont, skPaint);
                            y += lineHeight;
                        }
                        currentLine = word;
                    }
                    else
                    {
                        currentLine = testLine;
                    }
                }
                // Draw last line in this paragraph
                if (!string.IsNullOrEmpty(currentLine))
                {
                    var bounds = new SKRect();
                    skFont.MeasureText(currentLine, out bounds);
                    float drawX = x;
                    if (format.Alignment == StringAlignment.Center)
                        drawX += (maxWidth - bounds.Width) / 2;
                    else if (format.Alignment == StringAlignment.Far)
                        drawX += maxWidth - bounds.Width;

                    _canvas.DrawText(currentLine, drawX, y - skFont.Metrics.Ascent, skFont, skPaint);
                    y += lineHeight;
                }
            }
        }

        // Overloaded DrawString for RectangleF
        public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format)
        {
            DrawString(s, font, brush, new Rectangle(
                (int)layoutRectangle.X, (int)layoutRectangle.Y,
                (int)layoutRectangle.Width, (int)layoutRectangle.Height), format);
        }

        // Dispose of the graphics object
        public void Dispose()
        {
            _canvas?.Dispose();
        }

        public Drawing2D.GraphicsState Save()
        {
            var state = new Drawing2D.GraphicsState(_canvas);
            _canvas.Save();
            return state;
        }

        public SizeF MeasureString(string text, Font font)
        {
            return MeasureString(text, font, 0, StringFormat.GenericTypographic);
        }

        public SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat)
        {
            return MeasureString(text, font, (int)layoutArea.Width, stringFormat);
        }

        public SizeF MeasureString(string text, Font font, int maxWidth, StringFormat stringFormat)
        {
            var skFont = font.ToSkFont();
            var lineHeightAdjustment = 1.1f;
            float lineHeight = (skFont.Metrics.Descent - skFont.Metrics.Ascent + skFont.Metrics.Leading) * lineHeightAdjustment;
            float maxLineWidth = 0f;
            float totalHeight = 0f;

            if (string.IsNullOrEmpty(text))
                return new SizeF(0, lineHeight);

            var lines = text.Replace("\r\n", "\n").Split('\n');
            foreach (var line in lines)
            {
                var words = line.Split(' ');
                string currentLine = "";
                foreach (var word in words)
                {
                    string testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
                    var bounds = new SKRect();
                    skFont.MeasureText(testLine, out bounds);

                    if (maxWidth > 0 && bounds.Width > maxWidth)
                    {
                        // Commit currentLine, start new line
                        if (!string.IsNullOrEmpty(currentLine))
                        {
                            var currentBounds = new SKRect();
                            skFont.MeasureText(currentLine, out currentBounds);
                            maxLineWidth = Math.Max(maxLineWidth, currentBounds.Width);
                            totalHeight += lineHeight;
                            currentLine = word; // Start new line with the current word
                        }
                        else
                        {
                            // The word itself is longer than maxWidth, commit it as its own line
                            var wordBounds = new SKRect();
                            skFont.MeasureText(word, out wordBounds);
                            maxLineWidth = Math.Max(maxLineWidth, wordBounds.Width);
                            totalHeight += lineHeight;
                            currentLine = ""; // Start new line
                        }
                    }
                    else
                    {
                        currentLine = testLine;
                    }
                }
                // Commit last line in this paragraph (even if it's a single word)
                if (!string.IsNullOrEmpty(currentLine))
                {
                    var currentBounds = new SKRect();
                    skFont.MeasureText(currentLine, out currentBounds);
                    maxLineWidth = Math.Max(maxLineWidth, currentBounds.Width);
                    totalHeight += lineHeight;

                    // If the last line is more than 60% of the maxWidth, add a new blank line of height
                    if (maxWidth > 0 && currentBounds.Width > 0.6f * maxWidth)
                    {
                        totalHeight += lineHeight;
                    }
                }
            }

            // If at least one line, ensure totalHeight is at least one lineHeight
            if (totalHeight < lineHeight)
                totalHeight = lineHeight;

            var padding = 1.5f;
            return new SizeF(maxLineWidth, (totalHeight) * padding);
        }

        public Region[] MeasureCharacterRanges(string text, Font font, RectangleF layoutRect, StringFormat stringFormat)
        {
            var skPaint = font.ToSkFont();
            var regions = new List<Region>();

            foreach (var range in stringFormat.MeasurableCharacterRanges)
            {
                var substring = text.AsSpan(range.First, range.Length);
                var bounds = new SKRect();
                skPaint.MeasureText(substring, out bounds);

                var padding = 1.2f;
                var region = new Region(
                    (int)(layoutRect.X + bounds.Left),
                    (int)(layoutRect.Y + bounds.Top),
                    (int)(bounds.Width + 2 * padding),
                    (int)(bounds.Height + 2  * padding)
                );

                regions.Add(region);
            }

            return regions.ToArray();
        }

        public void ResetTransform()
        {
            // TODO: ?
        }

        public void AddMetafileComment(byte[] data)
        {
            // TODO: ?
        }

        // Restore the graphics state
        public void Restore(Drawing2D.GraphicsState state)
        {
            _canvas.RestoreToCount(state.SaveLayer.SaveCount);
        }

        public static Graphics FromImage(Bitmap bm)
        {
            return bm.GetGraphics();
        }
    }
}


