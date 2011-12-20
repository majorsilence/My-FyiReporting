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
	/// ColumnGroupings definition and processing.
	///</summary>
	[Serializable]
	internal class ColumnGroupings : ReportLink
	{
        List<ColumnGrouping> _Items;			// list of report items
		int _StaticCount;

		internal ColumnGroupings(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			ColumnGrouping g;
            _Items = new List<ColumnGrouping>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "ColumnGrouping":
						g = new ColumnGrouping(r, this, xNodeLoop);
						break;
					default:	
						g=null;		// don't know what this is
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown ColumnGroupings element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
				if (g != null)
					_Items.Add(g);
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For ColumnGroups at least one ColumnGrouping is required.");
			else
			{
                _Items.TrimExcess();
				_StaticCount = GetStaticCount();
			}
		}
		
		override internal void FinalPass()
		{
			foreach (ColumnGrouping g in _Items)
			{
				g.FinalPass();
			}
			return;
		}

		internal float DefnHeight()
		{
			float height=0;
			foreach (ColumnGrouping g in _Items)
			{
				height += g.Height.Points;
			}
			return height;
		}
/// <summary>
/// Calculates the number of static columns
/// </summary>
/// <returns></returns>
		private int GetStaticCount()
		{
			// Find the static column
			foreach (ColumnGrouping cg in _Items)
			{
				if (cg.StaticColumns == null)
					continue;
				return cg.StaticColumns.Items.Count;
			}
			return 0;
		}
/// <summary>
/// # of static columns;  0 if no static columns defined
/// </summary>
		internal int StaticCount
		{
			get {return _StaticCount;}
		}

        internal List<ColumnGrouping> Items
		{
			get { return  _Items; }
		}

		internal MatrixEntry GetME(Report rpt)
		{
			WorkClass wc = GetWC(rpt);
			return wc.ME;
		}

		internal void SetME(Report rpt, MatrixEntry me)
		{
			WorkClass wc = GetWC(rpt);
			wc.ME = me;
		}

		private WorkClass GetWC(Report rpt)
		{
			if (rpt == null)
				return new WorkClass();

			WorkClass wc = rpt.Cache.Get(this, "wc") as WorkClass;
			if (wc == null)
			{
				wc = new WorkClass();
				rpt.Cache.Add(this, "wc", wc);
			}
			return wc;
		}

		private void RemoveWC(Report rpt)
		{
			rpt.Cache.Remove(this, "wc");
		}

		class WorkClass
		{
			internal MatrixEntry ME;	// Used at runtime to contain data values	
			internal WorkClass()
			{
				ME=null;
			}
		}
	}
}
