

using System;


namespace Majorsilence.Reporting.Rdl
{
	
	internal enum PlotTypeEnum
	{
		Auto,
		Line
	}

	internal class PlotType
	{
		static internal PlotTypeEnum GetStyle(string s, ReportLog rl)
		{
			PlotTypeEnum pt;

			switch (s.ToLowerInvariant())
			{		
				case "auto":
					pt = PlotTypeEnum.Auto;
					break;
				case "line":
					pt = PlotTypeEnum.Line;
					break;
				default:		
					rl.LogError(4, "Unknown PlotType '" + s + "'.  Auto assumed.");
					pt = PlotTypeEnum.Auto;
					break;
			}
			return pt;
		}
	}


}
