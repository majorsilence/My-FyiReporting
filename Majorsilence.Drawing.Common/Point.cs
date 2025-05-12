using System;

namespace Majorsilence.Drawing
{
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsEmpty => X == 0 && Y == 0;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public override string ToString()
        {
            return $"Point [X={X}, Y={Y}]";
        }

        public static implicit operator PointF(Point p)
        {
            return new PointF(p.X, p.Y);
        }

        //#if !DRAWINGCOMPAT
        public static implicit operator System.Drawing.Point(Point p)
        {
            return new System.Drawing.Point(p.X, p.Y);
        }
        //#endif

        public static Point Empty => new Point(0, 0);

        public override bool Equals(object? obj)
        {
            if (obj is Point point)
            {
                return X == point.X && Y == point.Y;
            }
            return false;
        }

        public static bool operator ==(Point left, Point right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !(left == right);
        }

    }
}
