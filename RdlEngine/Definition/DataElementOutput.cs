

using System;


namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// Filter operators
	///</summary>
	public enum DataElementOutputEnum
	{
		Output,		// Indicates the item should appear in the output
		NoOutput,	// Indicates the item should not appear in the output
		ContentsOnly,	// Indicates the item should not appear in the XML, but its contents should be
						// rendered as if they were in this item�s
						// container. Only applies to Lists.
		Auto		// (Default): Will behave as NoOutput for
					// Textboxes with constant values,
					// ContentsOnly for Rectangles and Output for
					// all other items		

	}

	public class DataElementOutput
	{
        static public DataElementOutputEnum GetStyle(string s)
        {
            return GetStyle(s, null);
        }

		static internal DataElementOutputEnum GetStyle(string s, ReportLog rl)
		{
			DataElementOutputEnum rs;

			switch (s.ToLowerInvariant())
			{		
				case "output":
					rs = DataElementOutputEnum.Output;
					break;
				case "nooutput":
					rs = DataElementOutputEnum.NoOutput;
					break;
				case "contentsonly":
					rs = DataElementOutputEnum.ContentsOnly;
					break;
				case "auto":
					rs = DataElementOutputEnum.Auto;
					break;
				default:		
                    if (rl != null)
					    rl.LogError(4, "Unknown DataElementOutput '" + s + "'.  Auto assumed.");
					rs = DataElementOutputEnum.Auto;
					break;
			}
			return rs;
		}
	}

}
