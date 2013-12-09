using System.ComponentModel;
using fyiReporting.RdlDesign.RdlProperties;

namespace fyiReporting.RdlDesign
{
	public class LocalizedDescriptionAttribute : DescriptionAttribute
	{
		public LocalizedDescriptionAttribute(string description)
			: base(description)
		{
		}

		public override string Description
		{
			get
			{
				return Descriptions.ResourceManager.GetString(DescriptionValue, DisplayNames.Culture) ?? base.Description;
			}
		}
	}
}
