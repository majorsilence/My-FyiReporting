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
	/// Handle Matrix layout direction enumeration: LTR (left to right), RTL (right to left)
	///</summary>
	internal enum MatrixLayoutDirectionEnum
	{
		LTR,				// Left to Right
		RTL					// Right to Left
	}

	internal class MatrixLayoutDirection
	{
		static internal MatrixLayoutDirectionEnum GetStyle(string s, ReportLog rl)
		{
			MatrixLayoutDirectionEnum rs;

			switch (s)
			{		
				case "LTR":
				case "LeftToRight":
					rs = MatrixLayoutDirectionEnum.LTR;
					break;
				case "RTL":
				case "RightToLeft":
					rs = MatrixLayoutDirectionEnum.RTL;
					break;
				default:		
					rl.LogError(4, "Unknown MatrixLayoutDirection '" + s + "'.  LTR assumed.");
					rs = MatrixLayoutDirectionEnum.LTR;
					break;
			}
			return rs;
		}
	}

}
