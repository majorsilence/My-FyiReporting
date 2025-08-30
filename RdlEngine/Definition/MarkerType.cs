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

namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// Handle MarkerType enumeration: Square, circle, ...
	///</summary>
	internal enum MarkerTypeEnum
	{
		None,
		Square,
		Circle,
		Diamond,
		Triangle,
		Cross,
		Auto
	}
	internal class MarkerType
	{
		static internal MarkerTypeEnum GetStyle(string s, ReportLog rl)
		{
			MarkerTypeEnum rs;

			switch (s.ToLowerInvariant())
			{		
				case "none":
					rs = MarkerTypeEnum.None;
					break;
				case "square":
					rs = MarkerTypeEnum.Square;
					break;
				case "circle":
					rs = MarkerTypeEnum.Circle;
					break;
				case "diamond":
					rs = MarkerTypeEnum.Diamond;
					break;
				case "triangle":
					rs = MarkerTypeEnum.Triangle;
					break;
				case "cross":
					rs = MarkerTypeEnum.Cross;
					break;
				case "auto":
					rs = MarkerTypeEnum.Auto;
					break;
				default:		
					rl.LogError(4, "Unknown MarkerType '" + s + "'.  None assumed.");
					rs = MarkerTypeEnum.None;
					break;
			}
			return rs;
		}
	}

}
