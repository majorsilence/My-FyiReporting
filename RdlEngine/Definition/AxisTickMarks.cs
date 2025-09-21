using System;

namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// AxisTickMarks definition and processing.
	///</summary>
	public enum AxisTickMarksEnum
	{
		None,
		Inside,
		Outside,
		Cross
	}

	public class AxisTickMarks
	{
        static public AxisTickMarksEnum GetStyle(string s)
        {
            return AxisTickMarks.GetStyle(s, null);
        }

		static internal AxisTickMarksEnum GetStyle(string s, ReportLog rl)
		{
			AxisTickMarksEnum rs;

			switch (s.ToLowerInvariant())
			{		
				case "none":
					rs = AxisTickMarksEnum.None;
					break;
				case "inside":
					rs = AxisTickMarksEnum.Inside;
					break;
				case "outside":
					rs = AxisTickMarksEnum.Outside;
					break;
				case "cross":
					rs = AxisTickMarksEnum.Cross;
					break;
				default:		
                    if (rl != null)
					    rl.LogError(4, "Unknown Axis Tick Mark '" + s + "'.  None assumed.");
					rs = AxisTickMarksEnum.None;
					break;
			}
			return rs;
		}
	}

}
