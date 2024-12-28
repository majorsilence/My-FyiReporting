using SkiaSharp;


namespace Majorsilence.Drawing.Drawing2D
{
    public class GraphicsState
    {
        internal SKCanvas SaveLayer { get; }

        internal GraphicsState(SKCanvas canvas)
        {
            SaveLayer = canvas;
        }
    }

}
