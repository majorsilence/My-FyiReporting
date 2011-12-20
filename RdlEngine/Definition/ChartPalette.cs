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
	/// ChartPalette enum handling.
	///</summary>
	internal enum ChartPaletteEnum
	{
		Default,
		EarthTones,
		Excel,
		GrayScale,
		Light,
		Pastel,
		SemiTransparent,// 20022008 AJM GJL
        Patterned, //GJL
        PatternedBlack,// 20022008 AJM GJL
        Custom
	}

	internal class ChartPalette
	{
		static internal ChartPaletteEnum GetStyle(string s, ReportLog rl)
		{
			ChartPaletteEnum p;

			switch (s)
			{		
				case "Default":
					p = ChartPaletteEnum.Default;
					break;
				case "EarthTones":
					p = ChartPaletteEnum.EarthTones;
					break;
				case "Excel":
					p = ChartPaletteEnum.Excel;
					break;
				case "GrayScale":
					p = ChartPaletteEnum.GrayScale;
					break;
				case "Light":
					p = ChartPaletteEnum.Light;
					break;
				case "Pastel":
					p = ChartPaletteEnum.Pastel;
					break;
				case "SemiTransparent":
					p = ChartPaletteEnum.SemiTransparent;
					break;
                case "Patterned": //GJL
                    p = ChartPaletteEnum.Patterned;
                    break;
                case "PatternedBlack": //GJL
                    p = ChartPaletteEnum.PatternedBlack;
                    break;
                case "Custom":
                    p = ChartPaletteEnum.Custom;
                    break;
				default:		
					rl.LogError(4, "Unknown ChartPalette '" + s + "'.  Default assumed.");
					p = ChartPaletteEnum.Default;
					break;
			}
			return p;
		}
	}

}
