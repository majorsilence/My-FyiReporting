using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majorsilence.Drawing.Drawing2D
{

    public sealed class LinearGradientBrush : Brush
    {
        public LinearGradientBrush(Color color) : base(color)
        {
        }

        public LinearGradientBrush(Color color1, Color color2, float angle) : base(color1)
        {
            throw new NotImplementedException();
        }

        public LinearGradientBrush(Rectangle rect, Color color1, Color color2, LinearGradientMode mode) : base(color1)
        {
            // Implementation for initializing with rectangle, two colors, and gradient mode
            throw new NotImplementedException();
        }
    }
}
