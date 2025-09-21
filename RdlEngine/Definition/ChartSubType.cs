
using System;

namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// The full list of supported chart subtypes
	///</summary>
	internal enum ChartSubTypeEnum
	{
		Plain, 
		Stacked,
		PercentStacked,
		Smooth,
		Exploded,
		Line, 
		SmoothLine,
		HighLowClose, 
		OpenHighLowClose, 
		Candlestick
	}

	internal class ChartSubType
	{
		static internal ChartSubTypeEnum GetStyle(string s, ReportLog rl)
		{
			ChartSubTypeEnum st;

			switch (s)
			{		
				case "Plain":
					st = ChartSubTypeEnum.Plain;
					break;
				case "Stacked":
					st = ChartSubTypeEnum.Stacked;
					break;
				case "PercentStacked":
					st = ChartSubTypeEnum.PercentStacked;
					break;
				case "Smooth":
					st = ChartSubTypeEnum.Smooth;
					break;
				case "Exploded":
					st = ChartSubTypeEnum.Exploded;
					break;
				case "Line":
					st = ChartSubTypeEnum.Line;
					break;
				case "SmoothLine":
					st = ChartSubTypeEnum.SmoothLine;
					break;
				case "HighLowClose":
					st = ChartSubTypeEnum.HighLowClose;
					break;
				case "OpenHighLowClose":
					st = ChartSubTypeEnum.OpenHighLowClose;
					break;
				case "Candlestick":
					st = ChartSubTypeEnum.Candlestick;
					break;
				default:		
					rl.LogError(4, "Unknown ChartSubType '" + s + "'.  Plain assumed.");
					st = ChartSubTypeEnum.Plain;
					break;
			}
			return st;
		}
	}

}
