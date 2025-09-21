
using System;

namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// Handle the LegendLayout enumeration: Column, Row, Table
	///</summary>
	public enum LegendLayoutEnum
	{
		Column,
		Row,
		Table
	}
	public class LegendLayout
	{
        static public LegendLayoutEnum GetStyle(string s)
        {
            return LegendLayout.GetStyle(s, null);
        }

		static internal LegendLayoutEnum GetStyle(string s, ReportLog rl)
		{
			LegendLayoutEnum rs;

			switch (s.ToLowerInvariant())
			{		
				case "column":
					rs = LegendLayoutEnum.Column;
					break;
				case "row":
					rs = LegendLayoutEnum.Row;
					break;
				case "table":
					rs = LegendLayoutEnum.Table;
					break;
				default:		
                    if (rl != null)
					    rl.LogError(4, "Unknown LegendLayout '" + s + "'.  Column assumed.");
					rs = LegendLayoutEnum.Column;
					break;
			}
			return rs;
		}
	}

}
