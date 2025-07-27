
namespace Majorsilence.Drawing
{
    public struct Rectangle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Left => X;
        public int Top => Y;
        public int Right => X + Width;
        public int Bottom => Y + Height;
        public static Rectangle Empty => new Rectangle(0, 0, 0, 0);
        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rectangle(Point p, Size s)
        {
            X = p.X;
            Y = p.Y;
            Width = s.Width;
            Height = s.Height;
        }

        public bool IntersectsWith(Rectangle other)
        {
            return !(other.Left > Right ||
                     other.Right < Left ||
                     other.Top > Bottom ||
                     other.Bottom < Top);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Rectangle)
            {
                var other = (Rectangle)obj;
                return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
            }
            return false;
        }

        public override int GetHashCode()
        {
#if NET8_0
            return HashCode.Combine(X, Y, Width, Height);
#else
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                hash = hash * 23 + Width.GetHashCode();
                hash = hash * 23 + Height.GetHashCode();
                return hash;
            }
#endif
        }

        public static bool operator ==(Rectangle left, Rectangle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Rectangle left, Rectangle right)
        {
            return !(left == right);
        }
    }
}
