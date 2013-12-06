using System.ComponentModel;
using System.Threading;
using RdlMapFile.RdlProperties;

namespace fyiReporting.RdlMapFile
{
	internal class RdlCategoryAttribute : CategoryAttribute
	{
		internal RdlCategoryAttribute(string category)
			: base(category)
		{
		}

		protected override string GetLocalizedString(string value)
		{
			return Categories.ResourceManager.GetString(value, Thread.CurrentThread.CurrentUICulture) ?? base.GetLocalizedString(value);
		}
	}
}
