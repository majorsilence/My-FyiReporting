using System;

namespace Majorsilence.Reporting.Rdl
{
	public class PageEllipse : PageItem, ICloneable
	{
		public PageEllipse()
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