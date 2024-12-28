
namespace Majorsilence.Drawing
{
    public struct SizeF
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public SizeF(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public static SizeF Empty => new SizeF(0, 0);

        public override string ToString()
        {
            return $"SizeF [Width={Width}, Height={Height}]";
        }
    }
}
