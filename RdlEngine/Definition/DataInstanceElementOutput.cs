

using System;


namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// DataInstanceElement definition and processing.
	///</summary>
	public enum DataInstanceElementOutputEnum
	{
		Output,			// Indicates the list instances should appear in the output
		NoOutput		// Indicates the list instances should not appear in the output		
	}

	public class DataInstanceElementOutput
	{
        static public DataInstanceElementOutputEnum GetStyle(string s)
        {
            return GetStyle(s, null);
        }
		static internal DataInstanceElementOutputEnum GetStyle(string s, ReportLog rl)
		{
			DataInstanceElementOutputEnum rs;

			switch (s)
			{		
				case "Output":
					rs = DataInstanceElementOutputEnum.Output;
					break;
				case "NoOutput":
					rs = DataInstanceElementOutputEnum.NoOutput;
					break;
				default:		
                    if (rl != null)
					    rl.LogError(4, "Unknown DataInstanceElementOutput '" + s + "'.  Output assumed.");
					rs = DataInstanceElementOutputEnum.Output;
					break;
			}
			return rs;
		}
	}

}
