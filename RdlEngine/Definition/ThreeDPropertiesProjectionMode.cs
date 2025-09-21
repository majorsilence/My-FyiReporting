

using System;


namespace Majorsilence.Reporting.Rdl
{
	internal enum ThreeDPropertiesProjectionModeEnum
	{
		Perspective,
		Orthographic
	}

	internal class ThreeDPropertiesProjectionMode
	{
		static internal ThreeDPropertiesProjectionModeEnum GetStyle(string s)
		{
			ThreeDPropertiesProjectionModeEnum pm;

			switch (s.ToLowerInvariant())
			{		
				case "perspective":
					pm = ThreeDPropertiesProjectionModeEnum.Perspective;
					break;
				case "orthographic":
					pm = ThreeDPropertiesProjectionModeEnum.Orthographic;
					break;
				default:
					pm = ThreeDPropertiesProjectionModeEnum.Perspective;
					break;
			}
			return pm;
		}
	}


}
