

using System;


namespace Majorsilence.Reporting.Rdl
{

	internal enum ThreeDPropertiesShadingEnum
	{
		None,
		Simple,
		Real
	}

	internal class ThreeDPropertiesShading
	{
		static internal ThreeDPropertiesShadingEnum GetStyle(string s, ReportLog rl)
		{
			ThreeDPropertiesShadingEnum sh;

			switch (s.ToLowerInvariant())
			{		
				case "none":
					sh = ThreeDPropertiesShadingEnum.None;
					break;
				case "simple":
					sh = ThreeDPropertiesShadingEnum.Simple;
					break;
				case "real":
					sh = ThreeDPropertiesShadingEnum.Real;
					break;
				default:	
					rl.LogError(4, "Unknown Shading '" + s + "'.  None assumed.");
					sh = ThreeDPropertiesShadingEnum.None;
					break;
			}
			return sh;
		}
	}


}
