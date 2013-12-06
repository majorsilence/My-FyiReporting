using fyiReporting.RdlDesign.RdlProperties;
using System.ComponentModel;
using System.Threading;

namespace fyiReporting.RdlDesign
{
	internal class LocalizedCategoryAttribute : CategoryAttribute
	{
		internal LocalizedCategoryAttribute(string category)
			: base(category)
		{
		}

		protected override string GetLocalizedString(string value)
		{
			return Categories.ResourceManager.GetString(value, Thread.CurrentThread.CurrentUICulture) ?? base.GetLocalizedString(value);
		}
	}
}
