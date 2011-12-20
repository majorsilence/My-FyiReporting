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
