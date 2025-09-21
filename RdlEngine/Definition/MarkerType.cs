

using System;

namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// Handle MarkerType enumeration: Square, circle, ...
	///</summary>
	internal enum MarkerTypeEnum
	{
		None,
		Square,
		Circle,
		Diamond,
		Triangle,
		Cross,
		Auto
	}
	internal class MarkerType
	{
		static internal MarkerTypeEnum GetStyle(string s, ReportLog rl)
		{
			MarkerTypeEnum rs;

			switch (s.ToLowerInvariant())
			{		
				case "none":
					rs = MarkerTypeEnum.None;
					break;
				case "square":
					rs = MarkerTypeEnum.Square;
					break;
				case "circle":
					rs = MarkerTypeEnum.Circle;
					break;
				case "diamond":
					rs = MarkerTypeEnum.Diamond;
					break;
				case "triangle":
					rs = MarkerTypeEnum.Triangle;
					break;
				case "cross":
					rs = MarkerTypeEnum.Cross;
					break;
				case "auto":
					rs = MarkerTypeEnum.Auto;
					break;
				default:		
					rl.LogError(4, "Unknown MarkerType '" + s + "'.  None assumed.");
					rs = MarkerTypeEnum.None;
					break;
			}
			return rs;
		}
	}

}
