using System;
using System.Drawing;

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