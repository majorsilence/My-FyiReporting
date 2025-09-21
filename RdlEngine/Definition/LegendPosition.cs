
using System;

namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// Handle Legend position enumeration: TopLeft, LeftTop, ...
	///</summary>
	public enum LegendPositionEnum
	{
		TopLeft,
		TopCenter,
		TopRight,
		LeftTop,
		LeftCenter,
		LeftBottom,
		RightTop,
		RightCenter,
		RightBottom,
		BottomRight,
		BottomCenter,
		BottomLeft
	}
	public class LegendPosition
	{
        static public LegendPositionEnum GetStyle(string s)
        {
            return LegendPosition.GetStyle(s, null);
        }
		static internal LegendPositionEnum GetStyle(string s, ReportLog rl)
		{
			LegendPositionEnum rs;

			switch (s.ToLowerInvariant())
			{		
				case "topleft":
					rs = LegendPositionEnum.TopLeft;
					break;
				case "topcenter":
					rs = LegendPositionEnum.TopCenter;
					break;
				case "topright":
					rs = LegendPositionEnum.TopRight;
					break;
				case "lefttop":
					rs = LegendPositionEnum.LeftTop;
					break;
				case "leftcenter":
					rs = LegendPositionEnum.LeftCenter;
					break;
				case "leftbottom":
					rs = LegendPositionEnum.LeftBottom;
					break;
				case "righttop":
					rs = LegendPositionEnum.RightTop;
					break;
				case "rightcenter":
					rs = LegendPositionEnum.RightCenter;
					break;
				case "rightbottom":
					rs = LegendPositionEnum.RightBottom;
					break;
				case "bottomright":
					rs = LegendPositionEnum.BottomRight;
					break;
				case "bottomcenter":
					rs = LegendPositionEnum.BottomCenter;
					break;
				case "bottomleft":
					rs = LegendPositionEnum.BottomLeft;
					break;
				default:		
                    if (rl != null)
					    rl.LogError(4, "Unknown LegendPosition '" + s + "'.  RightTop assumed.");
					rs = LegendPositionEnum.RightTop;
					break;
			}
			return rs;
		}
	}

}
