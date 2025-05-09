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
