
namespace Majorsilence.Drawing
{
    public struct RectangleF
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public float Left => X;
        public float Top => Y;
        public float Right => X + Width;
        public float Bottom => Y + Height;

        public SizeF Size => new SizeF(Width, Height);

        public bool Contains(float x, float y)
        {
            return x >= Left && x <= Right && y >= Top && y <= Bottom;
        }

        public bool IntersectsWith(RectangleF rect)
        {
            return rect.Left < Right && rect.Right > Left && rect.Top < Bottom && rect.Bottom > Top;
        }

        public static RectangleF Intersect(RectangleF a, RectangleF b)
        {
            if (!a.IntersectsWith(b)) return new RectangleF(0, 0, 0, 0);

            float x = Math.Max(a.Left, b.Left);
            float y = Math.Max(a.Top, b.Top);
            float width = Math.Min(a.Right, b.Right) - x;
            float height = Math.Min(a.Bottom, b.Bottom) - y;

            return new RectangleF(x, y, width, height);
        }

        public static RectangleF Union(RectangleF a, RectangleF b)
        {
            float x = Math.Min(a.Left, b.Left);
            float y = Math.Min(a.Top, b.Top);
            float width = Math.Max(a.Right, b.Right) - x;
            float height = Math.Max(a.Bottom, b.Bottom) - y;

            return new RectangleF(x, y, width, height);
        }

        public bool IsEmpty => Width == 0 && Height == 0;
        public static RectangleF Empty => new RectangleF(0, 0, 0, 0);

        public override string ToString()
        {
            return $"RectangleF [X={X}, Y={Y}, Width={Width}, Height={Height}]";
        }
    }
}
