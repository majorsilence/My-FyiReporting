using System;
using System.Drawing;

namespace fyiReporting.RDL
{
	public class PageCurve : PageItem, ICloneable
	{
		PointF[] _pointsF;
		int _offset;
		float _Tension;

		public PageCurve()
		{
		}

		public PointF[] Points
		{
			get { return _pointsF; }
			set { _pointsF = value; }
		}

		public int Offset
		{
			get { return _offset; }
			set { _offset = value; }
		}

		public float Tension
		{
			get { return _Tension; }
			set { _Tension = value; }
		}
		#region ICloneable Members

		new public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion
	}
}