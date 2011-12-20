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
	/// TableGroups definition and processing.
	///</summary>
	[Serializable]
	internal class TableGroups : ReportLink
	{
        List<TableGroup> _Items;			// list of TableGroup entries

		internal TableGroups(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			TableGroup tg;
            _Items = new List<TableGroup>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "TableGroup":
						tg = new TableGroup(r, this, xNodeLoop);
						break;
					default:	
						tg=null;		// don't know what this is
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown TableGroups element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
				if (tg != null)
					_Items.Add(tg);
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For TableGroups at least one TableGroup is required.");
			else
                _Items.TrimExcess();
		}
		
		override internal void FinalPass()
		{
			foreach(TableGroup tg in _Items)
			{
				tg.FinalPass();
			}

			return;
		}

		internal float DefnHeight()
		{
			float height=0;
			foreach(TableGroup tg in _Items)
			{
				height += tg.DefnHeight();
			}
			return height;
		}

        internal List<TableGroup> Items
		{
			get { return  _Items; }
		}
	}
}
