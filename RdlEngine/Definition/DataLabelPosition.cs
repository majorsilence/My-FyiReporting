

using System;


namespace Majorsilence.Reporting.Rdl
{
	
	internal enum DataLabelPositionEnum
	{
		Auto,
		Top,
		TopLeft,
		TopRight,
		Left,
		Center,
		Right,
		BottomRight,
		Bottom,
		BottomLeft
	}

	internal class DataLabelPosition
	{
		static internal DataLabelPositionEnum GetStyle(string s, ReportLog rl)
		{
			DataLabelPositionEnum dlp;

			switch (s.ToLowerInvariant())
			{		
				case "auto":
					dlp = DataLabelPositionEnum.Auto;
					break;
				case "top":
					dlp = DataLabelPositionEnum.Top;
					break;
				case "topleft":
					dlp = DataLabelPositionEnum.TopLeft;
					break;
				case "topright":
					dlp = DataLabelPositionEnum.TopRight;
					break;
				case "left":
					dlp = DataLabelPositionEnum.Left;
					break;
				case "center":
					dlp = DataLabelPositionEnum.Center;
					break;
				case "right":
					dlp = DataLabelPositionEnum.Right;
					break;
				case "bottomright":
					dlp = DataLabelPositionEnum.BottomRight;
					break;
				case "bottom":
					dlp = DataLabelPositionEnum.Bottom;
					break;
				case "bottomleft":
					dlp = DataLabelPositionEnum.BottomLeft;
					break;
				default:		
					rl.LogError(4, "Unknown DataLablePosition '" + s + "'.  Auto assumed.");
					dlp = DataLabelPositionEnum.Auto;
					break;
			}
			return dlp;
		}
	}


}
