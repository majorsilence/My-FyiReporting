

using System;


namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// Three value state; true, false, auto (dependent on context)
	///</summary>
	internal enum QueryCommandTypeEnum
	{
		Text,
		StoredProcedure,
		TableDirect
	}
	internal class QueryCommandType
	{
		static internal QueryCommandTypeEnum GetStyle(string s, ReportLog rl)
		{
			QueryCommandTypeEnum rs;

			switch (s.ToLowerInvariant())
			{		
				case "text":
					rs = QueryCommandTypeEnum.Text;
					break;
				case "storedprocedure":
					rs = QueryCommandTypeEnum.StoredProcedure;
					break;
				case "tabledirect":
					rs = QueryCommandTypeEnum.TableDirect;
					break;
				default:		// user error just force to normal TODO
					rl.LogError(4, "Unknown Query CommandType '" + s + "'.  Text assumed.");
					rs = QueryCommandTypeEnum.Text;
					break;
			}
			return rs;
		}
	}

}
