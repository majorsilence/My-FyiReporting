
namespace Majorsilence.Drawing
{
    public class PointF
    {
        public float X { get; set; }
        public float Y { get; set; }
        public PointF() { }
        public PointF(float x, float y)
        {
            X = x;
            Y = y;
        }
        public override string ToString()
        {
            return $"PointF [X={X}, Y={Y}]";
        }

        public static implicit operator Point(PointF p)
        {
            return new Point((int)p.X, (int)p.Y);
        }

        //#if !DRAWINGCOMPAT
        public static implicit operator System.Drawing.PointF(PointF p)
        {
            return new System.Drawing.PointF(p.X, p.Y);
        }

        //#endif
    }

}
