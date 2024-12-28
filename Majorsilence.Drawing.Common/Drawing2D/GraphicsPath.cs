using SkiaSharp;


namespace Majorsilence.Drawing.Drawing2D
{
    public class GraphicsPath
    {
        private SKPath _path;

        public GraphicsPath()
        {
            _path = new SKPath();
        }

        public void AddLine(float x1, float y1, float x2, float y2)
        {
            _path.MoveTo(x1, y1);
            _path.LineTo(x2, y2);
        }

        public void AddRectangle(float x, float y, float width, float height)
        {
            _path.AddRect(new SKRect(x, y, x + width, y + height));
        }

        public SKPath ToSKPath() => _path;
    }

}
