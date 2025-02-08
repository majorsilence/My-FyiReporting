// 
// CairoExtensions.cs
//  
// Author:
//       Jonathan Pobst <monkey@jpobst.com>
// 
// Copyright (c) 2010 Jonathan Pobst
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using Cairo;
#if DRAWINGCOMPAT
using Draw2 = Majorsilence.Drawing;
#else
using Draw2 = System.Drawing;
#endif

namespace Majorsilence.Reporting.RdlGtk3
{
	public static class CairoExtensions
	{
		// Most of these functions return an affected area
		// This can be ignored if you don't need it
		
		#region context
		public static Rectangle DrawRectangle (this Context g, Rectangle r, Color color, int lineWidth)
		{
			// Put it on a pixel line
			if (lineWidth == 1)
				r = new Rectangle (r.X - 0.5, r.Y - 0.5, r.Width, r.Height);
			
			g.Save ();
			
			g.MoveTo (r.X, r.Y);
			g.LineTo (r.X + r.Width, r.Y);
			g.LineTo (r.X + r.Width, r.Y + r.Height);
			g.LineTo (r.X, r.Y + r.Height);
			g.LineTo (r.X, r.Y);
			
			g.SetSourceColor(color);
			g.LineWidth = lineWidth;
			g.LineCap = LineCap.Square;
			
			Rectangle dirty = g.StrokeExtents ();
			g.Stroke ();
			
			g.Restore ();
			
			return dirty;
		}
		
		public static Path CreateRectanglePath (this Context g, Rectangle r)
		{
			g.Save ();
			
			g.MoveTo (r.X, r.Y);
			g.LineTo (r.X + r.Width, r.Y);
			g.LineTo (r.X + r.Width, r.Y + r.Height);
			g.LineTo (r.X, r.Y + r.Height);
			g.LineTo (r.X, r.Y);
			
			Path path = g.CopyPath ();
			g.Restore ();
			
			return path;
		}

		public static Rectangle FillRectangle (this Context g, Rectangle r, Color color)
		{
			g.Save ();
			
			g.MoveTo (r.X, r.Y);
			g.LineTo (r.X + r.Width, r.Y);
			g.LineTo (r.X + r.Width, r.Y + r.Height);
			g.LineTo (r.X, r.Y + r.Height);
			g.LineTo (r.X, r.Y);
			
			g.SetSourceColor(color);
			
			Rectangle dirty = g.StrokeExtents ();

			g.Fill ();
			g.Restore ();

			return dirty;
		}

		public static Rectangle FillRectangle (this Context g, Rectangle r, Pattern pattern)
		{
			g.Save ();
			
			g.MoveTo (r.X, r.Y);
			g.LineTo (r.X + r.Width, r.Y);
			g.LineTo (r.X + r.Width, r.Y + r.Height);
			g.LineTo (r.X, r.Y + r.Height);
			g.LineTo (r.X, r.Y);

#pragma warning disable CS0618 // Type or member is obsolete
            g.Pattern = pattern;
#pragma warning restore CS0618 // Type or member is obsolete

            Rectangle dirty = g.StrokeExtents ();
			g.Fill ();

			g.Restore ();

			return dirty;
		}

		public static Rectangle DrawPolygonal (this Context g, PointD[] points, Color color)
		{
			Random rand=new Random();
			
			g.Save ();
			g.MoveTo (points [0]);
			foreach (var point in points) {
				g.LineTo (point.X - rand.NextDouble()*0, point.Y);
				//g.Stroke();
			}
			
			g.SetSourceColor(color);
			
			Rectangle dirty = g.StrokeExtents ();
			g.Stroke ();

			g.Restore ();

			return dirty;
		}

		public static Rectangle FillPolygonal (this Context g, PointD[] points, Color color)
		{
			g.Save ();
			
			g.MoveTo (points [0]);
			foreach (var point in points)
				g.LineTo (point);
			
			g.SetSourceColor(color);
			
			Rectangle dirty = g.StrokeExtents ();
			g.Fill ();

			g.Restore ();

			return dirty;
		}

		public static Rectangle FillStrokedRectangle (this Context g, Rectangle r, Color fill, Color stroke, int lineWidth)
		{
			double x = r.X;
			double y = r.Y;
			
			g.Save ();

			// Put it on a pixel line
			if (lineWidth == 1) {
				x += 0.5;
				y += 0.5;
			}
			
			g.MoveTo (x, y);
			g.LineTo (x + r.Width, y);
			g.LineTo (x + r.Width, y + r.Height);
			g.LineTo (x, y + r.Height);
			g.LineTo (x, y);
			
			g.SetSourceColor(fill);
			g.FillPreserve ();
			
			g.SetSourceColor(stroke);
			g.LineWidth = lineWidth;
			g.LineCap = LineCap.Square;
			
			Rectangle dirty = g.StrokeExtents ();
			
			g.Stroke ();
			g.Restore ();
			
			return dirty;
		}

		public static Rectangle DrawEllipse (this Context g, Rectangle r, Color color, int lineWidth)
		{
			double rx = r.Width / 2;
			double ry = r.Height / 2;
			double cx = r.X + rx;
			double cy = r.Y + ry;
			double c1 = 0.552285;
			
			g.Save ();
			
			g.MoveTo (cx + rx, cy);
			
			g.CurveTo (cx + rx, cy - c1 * ry, cx + c1 * rx, cy - ry, cx, cy - ry);
			g.CurveTo (cx - c1 * rx, cy - ry, cx - rx, cy - c1 * ry, cx - rx, cy);
			g.CurveTo (cx - rx, cy + c1 * ry, cx - c1 * rx, cy + ry, cx, cy + ry);
			g.CurveTo (cx + c1 * rx, cy + ry, cx + rx, cy + c1 * ry, cx + rx, cy);
			
			g.ClosePath ();
			
			g.SetSourceColor(color);
			g.LineWidth = lineWidth;
			
			Rectangle dirty = g.StrokeExtents ();

			g.Stroke ();
			g.Restore ();

			return dirty;
		}

		public static Rectangle FillEllipse (this Context g, Rectangle r, Color color)
		{
			double rx = r.Width / 2;
			double ry = r.Height / 2;
			double cx = r.X + rx;
			double cy = r.Y + ry;
			double c1 = 0.552285;
			
			g.Save ();
			
			g.MoveTo (cx + rx, cy);
			
			g.CurveTo (cx + rx, cy - c1 * ry, cx + c1 * rx, cy - ry, cx, cy - ry);
			g.CurveTo (cx - c1 * rx, cy - ry, cx - rx, cy - c1 * ry, cx - rx, cy);
			g.CurveTo (cx - rx, cy + c1 * ry, cx - c1 * rx, cy + ry, cx, cy + ry);
			g.CurveTo (cx + c1 * rx, cy + ry, cx + rx, cy + c1 * ry, cx + rx, cy);
			
			g.ClosePath ();
			
			g.SetSourceColor(color);
			
			Rectangle dirty = g.StrokeExtents ();
			
			g.Fill ();
			g.Restore ();
			
			return dirty;
		}

		public static Path CreateEllipsePath (this Context g, Rectangle r)
		{
			double rx = r.Width / 2;
			double ry = r.Height / 2;
			double cx = r.X + rx;
			double cy = r.Y + ry;
			double c1 = 0.552285;
			
			g.Save ();
			
			g.MoveTo (cx + rx, cy);
			
			g.CurveTo (cx + rx, cy - c1 * ry, cx + c1 * rx, cy - ry, cx, cy - ry);
			g.CurveTo (cx - c1 * rx, cy - ry, cx - rx, cy - c1 * ry, cx - rx, cy);
			g.CurveTo (cx - rx, cy + c1 * ry, cx - c1 * rx, cy + ry, cx, cy + ry);
			g.CurveTo (cx + c1 * rx, cy + ry, cx + rx, cy + c1 * ry, cx + rx, cy);
			
			g.ClosePath ();

			Path path = g.CopyPath ();
			
			g.Restore ();
			
			return path;
		}

		public static Rectangle FillStrokedEllipse (this Context g, Rectangle r, Color fill, Color stroke, int lineWidth)
		{
			double rx = r.Width / 2;
			double ry = r.Height / 2;
			double cx = r.X + rx;
			double cy = r.Y + ry;
			double c1 = 0.552285;
			
			g.Save ();
			
			g.MoveTo (cx + rx, cy);
			
			g.CurveTo (cx + rx, cy - c1 * ry, cx + c1 * rx, cy - ry, cx, cy - ry);
			g.CurveTo (cx - c1 * rx, cy - ry, cx - rx, cy - c1 * ry, cx - rx, cy);
			g.CurveTo (cx - rx, cy + c1 * ry, cx - c1 * rx, cy + ry, cx, cy + ry);
			g.CurveTo (cx + c1 * rx, cy + ry, cx + rx, cy + c1 * ry, cx + rx, cy);
			
			g.ClosePath ();
			
			g.SetSourceColor(fill);
			g.FillPreserve ();
			
			g.SetSourceColor(stroke);
			g.LineWidth = lineWidth;
			
			Rectangle dirty = g.StrokeExtents ();
			
			g.Stroke ();
			g.Restore ();
			
			return dirty;
		}

		public static Rectangle FillStrokedRoundedRectangle (this Context g, Rectangle r, double radius, Color fill, Color stroke, int lineWidth)
		{
			g.Save ();

			if ((radius > r.Height / 2) || (radius > r.Width / 2))
				radius = Math.Min (r.Height / 2, r.Width / 2);

			g.MoveTo (r.X, r.Y + radius);
			g.Arc (r.X + radius, r.Y + radius, radius, Math.PI, -Math.PI / 2);
			g.LineTo (r.X + r.Width - radius, r.Y);
			g.Arc (r.X + r.Width - radius, r.Y + radius, radius, -Math.PI / 2, 0);
			g.LineTo (r.X + r.Width, r.Y + r.Height - radius);
			g.Arc (r.X + r.Width - radius, r.Y + r.Height - radius, radius, 0, Math.PI / 2);
			g.LineTo (r.X + radius, r.Y + r.Height);
			g.Arc (r.X + radius, r.Y + r.Height - radius, radius, Math.PI / 2, Math.PI);
			g.ClosePath ();

			g.Restore ();
			
			g.SetSourceColor(fill);
			g.FillPreserve ();
			
			g.SetSourceColor(stroke);
			g.LineWidth = lineWidth;
			
			Rectangle dirty = g.StrokeExtents ();
			
			g.Stroke ();
			g.Restore ();
			
			return dirty;
		}

		public static Rectangle FillRoundedRectangle (this Context g, Rectangle r, double radius, Color fill)
		{
			g.Save ();

			if ((radius > r.Height / 2) || (radius > r.Width / 2))
				radius = Math.Min (r.Height / 2, r.Width / 2);

			g.MoveTo (r.X, r.Y + radius);
			g.Arc (r.X + radius, r.Y + radius, radius, Math.PI, -Math.PI / 2);
			g.LineTo (r.X + r.Width - radius, r.Y);
			g.Arc (r.X + r.Width - radius, r.Y + radius, radius, -Math.PI / 2, 0);
			g.LineTo (r.X + r.Width, r.Y + r.Height - radius);
			g.Arc (r.X + r.Width - radius, r.Y + r.Height - radius, radius, 0, Math.PI / 2);
			g.LineTo (r.X + radius, r.Y + r.Height);
			g.Arc (r.X + radius, r.Y + r.Height - radius, radius, Math.PI / 2, Math.PI);
			g.ClosePath ();
			
			//g.Restore ();

			g.SetSourceColor(fill);
			
			Rectangle dirty = g.StrokeExtents ();

			g.Fill ();
			g.Restore ();

			return dirty;
		}

		public static void FillRegion (this Context g, Cairo.Region region, Color color)
		{
			g.Save ();
			
			g.SetSourceColor(color);
			
			for (int i=0;i<region.NumRectangles;i++)
			{
				var r = region.GetRectangle(i);
				g.MoveTo (r.X, r.Y);
				g.LineTo (r.X + r.Width, r.Y);
				g.LineTo (r.X + r.Width, r.Y + r.Height);
				g.LineTo (r.X, r.Y + r.Height);
				g.LineTo (r.X, r.Y);
				
				g.SetSourceColor(color);

				g.StrokeExtents ();
				g.Fill ();
			}
			
			g.Restore ();
		}

		public static Rectangle DrawRoundedRectangle (this Context g, Rectangle r, double radius, Color stroke, int lineWidth)
		{
			g.Save ();
			
			Path p = g.CreateRoundedRectanglePath (r, radius);
			
			g.AppendPath (p);
			
			g.SetSourceColor(stroke);
			g.LineWidth = lineWidth;
			
			Rectangle dirty = g.StrokeExtents ();

			g.Stroke ();
			g.Restore ();

			(p as IDisposable).Dispose ();
			
			return dirty;
		}

		public static Path CreateRoundedRectanglePath (this Context g, Rectangle r, double radius)
		{
			g.Save ();

			if ((radius > r.Height / 2) || (radius > r.Width / 2))
				radius = Math.Min (r.Height / 2, r.Width / 2);

			g.MoveTo (r.X, r.Y + radius);
			g.Arc (r.X + radius, r.Y + radius, radius, Math.PI, -Math.PI / 2);
			g.LineTo (r.X + r.Width - radius, r.Y);
			g.Arc (r.X + r.Width - radius, r.Y + radius, radius, -Math.PI / 2, 0);
			g.LineTo (r.X + r.Width, r.Y + r.Height - radius);
			g.Arc (r.X + r.Width - radius, r.Y + r.Height - radius, radius, 0, Math.PI / 2);
			g.LineTo (r.X + radius, r.Y + r.Height);
			g.Arc (r.X + radius, r.Y + r.Height - radius, radius, Math.PI / 2, Math.PI);
			g.ClosePath ();
		
			Path p = g.CopyPath ();
			g.Restore ();
			
			return p;
		}

		public static Rectangle DrawLine (this Context g, PointD p1, PointD p2, Color color, int lineWidth)
		{
			// Put it on a pixel line
			if (lineWidth == 1)
				p1 = new PointD (p1.X - 0.5, p1.Y - 0.5);

			g.Save ();

			g.MoveTo (p1.X, p1.Y);
			g.LineTo (p2.X, p2.Y);

			g.SetSourceColor(color);
			g.LineWidth = lineWidth;
			g.LineCap = LineCap.Square;

			Rectangle dirty = g.StrokeExtents ();
			g.Stroke ();

			g.Restore ();

			return dirty;
		}

		public static Rectangle DrawText (this Context g, PointD p, string family, FontSlant slant, FontWeight weight, double size, Color color, string text)
		{
			g.Save ();

			g.MoveTo (p.X, p.Y);
			g.SelectFontFace (family, slant, weight);
			g.SetFontSize (size);
			g.SetSourceColor(color);
			
			TextExtents te = g.TextExtents(text);
			//TODO alignment
			
			// Center text on bottom
			/*// TODO cut the string in char array and for each center on bottom
			 * TextExtents te = g.TextExtents("a");
				cr.MoveTo(0.5 - te.Width  / 2 - te.XBearing, 0.5 - te.Height / 2 - te.YBearing);
*/
			//// Draw
			g.ShowText (text);
			//or
			//g.TextPath(text);
			//g.Fill();
			
			g.Restore ();

			return new Rectangle(te.XBearing, te.YBearing, te.Width, te.Height);
		}

		public static void DrawPixbuf (this Context g, Gdk.Pixbuf pixbuf, Point dest)
		{
			g.DrawPixbuf (pixbuf, dest.X, dest.Y);
		}
		
		public static void DrawPixbuf (this Context g, Gdk.Pixbuf pixbuf, int x, int y)
		{
			g.Save ();

			Gdk.CairoHelper.SetSourcePixbuf (g, pixbuf, x, y);
			g.Paint ();
			g.Restore ();
		}

		public static void DrawPixbufRect (this Context g, Gdk.Pixbuf pixbuf, Rectangle r, float scale)
		{
			var wScale = (r.Width / scale)  / pixbuf.Width;
			var hScale = (r.Height / scale) / pixbuf.Height;
			g.Save();
			g.Scale(scale * wScale, scale * hScale);
			Gdk.CairoHelper.SetSourcePixbuf(g, pixbuf, (int)(r.X / scale / wScale), (int)(r.Y / scale / hScale));
			g.Paint();
			g.Restore();
		}

		#endregion
		
		public static double Distance (this PointD s, PointD e)
		{
			return Magnitude (new PointD (s.X - e.X, s.Y - e.Y));
		}
		
		public static double Magnitude(this PointD p)
        {
            return Math.Sqrt(p.X * p.X + p.Y * p.Y);
        }

		public static Cairo.Rectangle ToCairoRectangle (this Gdk.Rectangle r)
		{
			return new Cairo.Rectangle (r.X, r.Y, r.Width, r.Height);
		}

		public static Cairo.Point Location (this Cairo.Rectangle r)
		{
			return new Cairo.Point ((int)r.X, (int)r.Y);
		}

		public static Cairo.Rectangle Clamp (this Cairo.Rectangle r)
		{
			double x = r.X;
			double y = r.Y;
			double w = r.Width;
			double h = r.Height;
			
			if (x < 0) {
				w -= x;
				x = 0;
			}
			
			if (y < 0) {
				h -= y;
				y = 0;
			}
			
			return new Cairo.Rectangle (x, y, w, h);
		}

		public static Gdk.Rectangle ToGdkRectangle (this Cairo.Rectangle r)
		{
			return new Gdk.Rectangle ((int)r.X, (int)r.Y, (int)r.Width, (int)r.Height);
		}

		public static bool ContainsPoint (this Cairo.Rectangle r, double x, double y)
		{
			if (x < r.X || x >= r.X + r.Width)
				return false;
			
			if (y < r.Y || y >= r.Y + r.Height)
				return false;
			
			return true;
		}
		
		public static Gdk.Size ToSize (this Cairo.Point point)
		{
			return new Gdk.Size (point.X, point.Y);
		}
		
		public static ImageSurface Clone (this ImageSurface surf)
		{
			ImageSurface newsurf = new ImageSurface (surf.Format, surf.Width, surf.Height);

			using (Context g = new Context (newsurf)) {
				g.SetSource (surf);
				g.Paint ();
			}

			return newsurf;
		}
		
		public static Gdk.Color ToGdkColor (this Cairo.Color color)
		{
			Gdk.Color c = new Gdk.Color ();
			c.Blue = (ushort)(color.B * ushort.MaxValue);
			c.Red = (ushort)(color.R * ushort.MaxValue);
			c.Green = (ushort)(color.G * ushort.MaxValue);
			
			return c;
		}
		
		public static ushort GdkColorAlpha (this Cairo.Color color)
		{
			return (ushort)(color.A * ushort.MaxValue);
		}

		public static double GetBottom (this Rectangle rect)
		{
			return rect.Y + rect.Height;
		}

		public static double GetRight (this Rectangle rect)
		{
			return rect.X + rect.Width;
		}

        /// <summary>
        /// Determines if the requested pixel coordinate is within bounds.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>true if (x,y) is in bounds, false if it's not.</returns>
        public static bool IsVisible(this ImageSurface surf, int x, int y)
        {
            return x >= 0 && x < surf.Width && y >= 0 && y < surf.Height;
        }
		
		public static Path CreatePolygonPath (this Context g, Point[][] polygonSet)
		{
			g.Save ();
			Point p;
			for (int i =0; i < polygonSet.Length; i++)
			{
				if (polygonSet[i].Length == 0)
					continue;
				
				p = polygonSet[i][0];
				g.MoveTo (p.X, p.Y);
				
				for (int j =1; j < polygonSet[i].Length; j++)
				{
					p = polygonSet[i][j];
					g.LineTo (p.X, p.Y);	
				}
				g.ClosePath ();
			}
			
			Path path = g.CopyPath ();
			
			g.Restore ();
			
			return path;
		}
		
		public static Color ToCairoColor (this Draw2.Color color)
		{
			return new Color (color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
		}
	}
}

