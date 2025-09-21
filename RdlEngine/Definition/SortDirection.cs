
using System;

namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// Handle sort direction enumeration: ascending, descending.
	///</summary>
	internal enum SortDirectionEnum
	{
		Ascending,
		Descending
	}
	internal class SortDirection
	{
		static internal SortDirectionEnum GetStyle(string s, ReportLog rl)
		{
			SortDirectionEnum rs;

			switch (s.ToLowerInvariant())
			{		
				case "ascending":
					rs = SortDirectionEnum.Ascending;
					break;
				case "descending":
					rs = SortDirectionEnum.Descending;
					break;
				default:		
					rl.LogError(4, "Unknown SortDirection '" + s + "'.  Ascending assumed.");
					rs = SortDirectionEnum.Ascending;
					break;
			}
			return rs;
		}
	}

}
