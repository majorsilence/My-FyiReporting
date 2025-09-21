

using System;

namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// Filter operators
	///</summary>
	internal enum FilterOperatorEnum
	{
		Equal,
		Like,
		NotEqual,
		GreaterThan,
		GreaterThanOrEqual,
		LessThan,
		LessThanOrEqual,
		TopN,
		BottomN,
		TopPercent,
		BottomPercent,
		In,
		Between,
		Unknown					// prior to definition or illegal value
	}
	internal class FilterOperator
	{
		static internal FilterOperatorEnum GetStyle(string s)
		{
			FilterOperatorEnum rs;

			switch (s)
			{		
				case "Equal":
				case "=":
					rs = FilterOperatorEnum.Equal;
					break;
				case "TopN":
					rs = FilterOperatorEnum.TopN;
					break;
				case "BottomN":
					rs = FilterOperatorEnum.BottomN;
					break;
				case "TopPercent":
					rs = FilterOperatorEnum.TopPercent;
					break;
				case "BottomPercent":
					rs = FilterOperatorEnum.BottomPercent;
					break;
				case "In":
					rs = FilterOperatorEnum.In;
					break;
				case "LessThanOrEqual":
				case "<=":
					rs = FilterOperatorEnum.LessThanOrEqual;
					break;
				case "LessThan":
				case "<":
					rs = FilterOperatorEnum.LessThan;
					break;
				case "GreaterThanOrEqual":
				case ">=":
					rs = FilterOperatorEnum.GreaterThanOrEqual;
					break;
				case "GreaterThan":
				case ">":
					rs = FilterOperatorEnum.GreaterThan;
					break;
				case "NotEqual":
				case "!=":
					rs = FilterOperatorEnum.NotEqual;
					break;
				case "Between":
					rs = FilterOperatorEnum.Between;
					break;
				case "Like":
					rs = FilterOperatorEnum.Like;
					break;
				default:		// user error just force to normal TODO
					rs = FilterOperatorEnum.Unknown;
					break;
			}
			return rs;
		}
	}

}
