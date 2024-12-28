
namespace Majorsilence.Drawing
{
    public struct Size
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public static Size Empty => new Size(0, 0);

        public override string ToString()
        {
            return $"Size [Width={Width}, Height={Height}]";
        }
    }
}
