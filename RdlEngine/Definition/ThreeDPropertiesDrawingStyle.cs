

using System;


namespace Majorsilence.Reporting.Rdl
{
	
	internal enum ThreeDPropertiesDrawingStyleEnum
	{
		Cylinder,
		Cube
	}

	internal class ThreeDPropertiesDrawingStyle
	{
		static internal ThreeDPropertiesDrawingStyleEnum GetStyle(string s, ReportLog rl)
		{
			ThreeDPropertiesDrawingStyleEnum ds;

			switch (s.ToLowerInvariant())
			{		
				case "cylinder":
					ds = ThreeDPropertiesDrawingStyleEnum.Cylinder;
					break;
				case "cube":
					ds = ThreeDPropertiesDrawingStyleEnum.Cube;
					break;
				default:	
					rl.LogError(4, "Unknown DrawingStyle '" + s + "'.  Cube assumed.");
					ds = ThreeDPropertiesDrawingStyleEnum.Cube;
					break;
			}
			return ds;
		}
	}
}
