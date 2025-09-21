

using System;
using System.Threading.Tasks;
using System.Xml;

namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// Linking mechanism defining the tree of the report.
	///</summary>
	[Serializable]
	abstract public class ReportLink
	{
		internal ReportDefn OwnerReport;			// Main Report instance
		internal ReportLink Parent;			// Parent instance
		internal int ObjectNumber;

		internal ReportLink(ReportDefn r, ReportLink p)
		{
			OwnerReport = r;
			Parent = p;
			ObjectNumber = r.GetObjectNumber();
		}

		// Give opportunity for report elements to do additional work
		//   e.g.  expressions should be parsed at this point

		virtual internal Task FinalPass()
		{
			return Task.CompletedTask;
		}

		internal bool InPageHeaderOrFooter()
		{
			for (ReportLink rl = this.Parent; rl != null; rl = rl.Parent)
			{
				if (rl is PageHeader || rl is PageFooter)
					return true;
			}
			return false;
		}
	}
}
