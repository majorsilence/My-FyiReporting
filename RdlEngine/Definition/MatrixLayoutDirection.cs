
using System;

namespace Majorsilence.Reporting.Rdl
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
