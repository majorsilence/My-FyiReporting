using System;

namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// ChartElementOutput parsing.
	///</summary>
	internal enum ChartElementOutputEnum
	{
		Output,
		NoOutput
	}

	internal class ChartElementOutput
	{
		static internal ChartElementOutputEnum GetStyle(string s, ReportLog rl)
		{
			ChartElementOutputEnum ceo;

			switch (s.ToLowerInvariant())
			{		
				case "output":
					ceo = ChartElementOutputEnum.Output;
					break;
				case "nooutput":
					ceo = ChartElementOutputEnum.NoOutput;
					break;
				default:		
					rl.LogError(4, "Unknown ChartElementOutput '" + s + "'.  Output assumed.");
					ceo = ChartElementOutputEnum.Output;
					break;
			}
			return ceo;
		}
	}


}
