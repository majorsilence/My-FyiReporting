using System;

namespace fyiReporting.RDL
{
	public class PagePie : PageItem, ICloneable
	{
		Single SA;
		Single SW;
		public PagePie()
		{
		}
		public Single StartAngle
		{
			get { return SA; }
			set { SA = value; }
		}
		public Single SweepAngle
		{
			get { return SW; }
			set { SW = value; }
		}

		#region ICloneable Members

		new public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion
	}
}