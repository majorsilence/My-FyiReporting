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
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace fyiReporting.RDL
{
	///<summary>
	/// Collection of matrix static rows.
	///</summary>
	[Serializable]
	internal class StaticRows : ReportLink
	{
        List<StaticRow> _Items;			// list of StaticRow

		internal StaticRows(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			StaticRow sr;
            _Items = new List<StaticRow>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "StaticRow":
						sr = new StaticRow(r, this, xNodeLoop);
						break;
					default:	
						sr=null;		// don't know what this is
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown StaticRows element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
				if (sr != null)
					_Items.Add(sr);
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For StaticRows at least one StaticRow is required.");
			else
                _Items.TrimExcess();
		}
		
		override internal void FinalPass()
		{
			foreach (StaticRow r in _Items)
			{
				r.FinalPass();
			}
			return;
		}

        internal List<StaticRow> Items
		{
			get { return  _Items; }
		}
	}
}
