
using System;

namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// Handle title position enumeration: center, near, far.
	///</summary>
	internal enum TitlePositionEnum
	{
		Center,
		Near,
		Far
	}
	internal class TitlePosition
	{
		static internal TitlePositionEnum GetStyle(string s, ReportLog rl)
		{
			TitlePositionEnum rs;

			switch (s.ToLowerInvariant())
			{		
				case "center":
					rs = TitlePositionEnum.Center;
					break;
				case "near":
					rs = TitlePositionEnum.Near;
					break;
				case "far":
					rs = TitlePositionEnum.Far;
					break;
				default:	
					rl.LogError(4, "Unknown TitlePosition '" + s + "'.  Center assumed.");
					rs = TitlePositionEnum.Center;
					break;
			}
			return rs;
		}
	}

}
