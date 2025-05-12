using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majorsilence.Drawing.Drawing2D
{
    public sealed class HatchBrush : Brush
    {
        private readonly HatchStyle _hatchStyle;
        public readonly Color ForegroundColor;
        private readonly Color _backColor;

        public HatchBrush(HatchStyle hatchStyle, Color foreColor, Color backColor)
            : base(foreColor)
        {
            _hatchStyle = hatchStyle;
            ForegroundColor = foreColor;
            _backColor = backColor;
        }

        internal new SKPaint ToSkPaint()
        {
            var paint = base.ToSkPaint();
            paint.Style = SKPaintStyle.Fill;
            paint.Shader = CreateTwoColorHatchShader(_hatchStyle, ForegroundColor.ToSkColor(), _backColor.ToSkColor());
            return paint;
        }

        private SKShader CreateTwoColorHatchShader(HatchStyle hatchStyle, SKColor foreColor, SKColor backColor)
        {
            // Implement the logic to create a two-color hatch shader based on the hatch style
            // This is a placeholder implementation and should be replaced with actual logic
            return SKShader.CreateLinearGradient(
                new SKPoint(0, 0),
                new SKPoint(10, 10),
                new SKColor[] { foreColor, backColor },
                new float[] { 0, 1 },
                SKShaderTileMode.Repeat);
        }
    }
}
