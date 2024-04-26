using System;
#if LINUX
using System.DrawingCore;
#else
using System.Drawing;
#endif

namespace fyiReporting.RDL
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