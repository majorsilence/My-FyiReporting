
using System;

namespace Majorsilence.Reporting.Rdl
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
