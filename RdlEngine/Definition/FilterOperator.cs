/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC
   Copyright (C) 2011  Peter Gill <peter@majorsilence.com>

   This file is part of the fyiReporting RDL project.
	
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.


   For additional information, email info@fyireporting.com or visit
   the website www.fyiReporting.com.
*/

using System;

namespace fyiReporting.RDL
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
