
using System;

namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// Handle the matrix subtotal position: before, after
	///</summary>
	internal enum SubtotalPositionEnum
	{
		Before,			// left/above
		After			// right/below

	}

	internal class SubtotalPosition
	{
		static internal SubtotalPositionEnum GetStyle(string s, ReportLog rl)
		{
			SubtotalPositionEnum rs;

			switch (s)
			{		
				case "Before":
					rs = SubtotalPositionEnum.Before;
					break;
				case "After":
					rs = SubtotalPositionEnum.After;
					break;
				default:		
					rl.LogError(4, "Unknown SubtotalPosition '" + s + "'.  Before assumed.");
					rs = SubtotalPositionEnum.Before;
					break;
			}
			return rs;
		}
	}

}
