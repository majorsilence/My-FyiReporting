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
	
	internal enum DataLabelPositionEnum
	{
		Auto,
		Top,
		TopLeft,
		TopRight,
		Left,
		Center,
		Right,
		BottomRight,
		Bottom,
		BottomLeft
	}

	internal class DataLabelPosition
	{
		static internal DataLabelPositionEnum GetStyle(string s, ReportLog rl)
		{
			DataLabelPositionEnum dlp;

			switch (s)
			{		
				case "Auto":
					dlp = DataLabelPositionEnum.Auto;
					break;
				case "Top":
					dlp = DataLabelPositionEnum.Top;
					break;
				case "TopLeft":
					dlp = DataLabelPositionEnum.TopLeft;
					break;
				case "TopRight":
					dlp = DataLabelPositionEnum.TopRight;
					break;
				case "Left":
					dlp = DataLabelPositionEnum.Left;
					break;
				case "Center":
					dlp = DataLabelPositionEnum.Center;
					break;
				case "Right":
					dlp = DataLabelPositionEnum.Right;
					break;
				case "BottomRight":
					dlp = DataLabelPositionEnum.BottomRight;
					break;
				case "Bottom":
					dlp = DataLabelPositionEnum.Bottom;
					break;
				case "BottomLeft":
					dlp = DataLabelPositionEnum.BottomLeft;
					break;
				default:		
					rl.LogError(4, "Unknown DataLablePosition '" + s + "'.  Auto assumed.");
					dlp = DataLabelPositionEnum.Auto;
					break;
			}
			return dlp;
		}
	}


}
