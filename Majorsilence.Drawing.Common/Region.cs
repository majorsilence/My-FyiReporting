using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majorsilence.Drawing
{
    public class Region : IDisposable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Region(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public int Area()
        {
            return Width * Height;
        }

        public bool Contains(int x, int y)
        {
            return x >= X && x <= X + Width && y >= Y && y <= Y + Height;
        }

        public RectangleF GetBounds(Graphics g)
        {
            if (g == null)
            {
                throw new ArgumentNullException(nameof(g));
            }

            return new RectangleF(X, Y, Width, Height);
        }

        public override string ToString()
        {
            return $"Region [X={X}, Y={Y}, Width={Width}, Height={Height}]";
        }

        public void Dispose()
        {
        }
    }
}
