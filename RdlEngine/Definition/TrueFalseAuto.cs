

using System;


namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// Three value state; true, false, auto (dependent on context)
	///</summary>
	internal enum TrueFalseAutoEnum
	{
		True,
		False,
		Auto
	}
	
	internal class TrueFalseAuto
	{
		static internal TrueFalseAutoEnum GetStyle(string s, ReportLog rl)
		{
			TrueFalseAutoEnum rs;

			switch (s.ToLowerInvariant())
			{		
				case "true":
					rs = TrueFalseAutoEnum.True;
					break;
				case "false":
					rs = TrueFalseAutoEnum.False;
					break;
				case "auto":
					rs = TrueFalseAutoEnum.Auto;
					break;
				default:		
					rl.LogError(4, "Unknown True False Auto value of '" + s + "'.  Auto assumed.");
					rs = TrueFalseAutoEnum.Auto;
					break;
			}
			return rs;
		}
	}
}
