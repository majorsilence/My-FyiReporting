using System;

namespace Majorsilence.Reporting.Rdl
{
	public class PageRectangle : PageItem, ICloneable
	{
		public PageRectangle()
		{
		}
		#region ICloneable Members

		new public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion
	}
}