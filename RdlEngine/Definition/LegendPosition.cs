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
	/// Handle Legend position enumeration: TopLeft, LeftTop, ...
	///</summary>
	public enum LegendPositionEnum
	{
		TopLeft,
		TopCenter,
		TopRight,
		LeftTop,
		LeftCenter,
		LeftBottom,
		RightTop,
		RightCenter,
		RightBottom,
		BottomRight,
		BottomCenter,
		BottomLeft
	}
	public class LegendPosition
	{
        static public LegendPositionEnum GetStyle(string s)
        {
            return LegendPosition.GetStyle(s, null);
        }
		static internal LegendPositionEnum GetStyle(string s, ReportLog rl)
		{
			LegendPositionEnum rs;

			switch (s)
			{		
				case "TopLeft":
					rs = LegendPositionEnum.TopLeft;
					break;
				case "TopCenter":
					rs = LegendPositionEnum.TopCenter;
					break;
				case "TopRight":
					rs = LegendPositionEnum.TopRight;
					break;
				case "LeftTop":
					rs = LegendPositionEnum.LeftTop;
					break;
				case "LeftCenter":
					rs = LegendPositionEnum.LeftCenter;
					break;
				case "LeftBottom":
					rs = LegendPositionEnum.LeftBottom;
					break;
				case "RightTop":
					rs = LegendPositionEnum.RightTop;
					break;
				case "RightCenter":
					rs = LegendPositionEnum.RightCenter;
					break;
				case "RightBottom":
					rs = LegendPositionEnum.RightBottom;
					break;
				case "BottomRight":
					rs = LegendPositionEnum.BottomRight;
					break;
				case "BottomCenter":
					rs = LegendPositionEnum.BottomCenter;
					break;
				case "BottomLeft":
					rs = LegendPositionEnum.BottomLeft;
					break;
				default:		
                    if (rl != null)
					    rl.LogError(4, "Unknown LegendPosition '" + s + "'.  RightTop assumed.");
					rs = LegendPositionEnum.RightTop;
					break;
			}
			return rs;
		}
	}

}
