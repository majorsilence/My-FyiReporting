using System;
#if DRAWINGCOMPAT
using Majorsilence.Drawing;
#else
using System.Drawing;
#endif

namespace Majorsilence.Reporting.Rdl
{
	public class PagePolygon : PageItem, ICloneable
	{
		PointF[] Ps;
		public PagePolygon()
		{
		}
		public PointF[] Points
		{
			get { return Ps; }
			set { Ps = value; }
		}
	}
}