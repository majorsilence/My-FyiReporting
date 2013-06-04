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
using System.Xml;
using System.IO;

namespace fyiReporting.RDL
{
	///<summary>
	/// Definition of the header rows for a table.
	///</summary>
	[Serializable]
	internal class Header : ReportLink, ICacheData
	{
		TableRows _TableRows;	// The header rows for the table or group
		bool _RepeatOnNewPage;	// Indicates this header should be displayed on
								// each page that the table (or group) is displayed		

		internal Header(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_TableRows=null;
			_RepeatOnNewPage=false;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "TableRows":
						_TableRows = new TableRows(r, this, xNodeLoop);
						break;
					case "RepeatOnNewPage":
						_RepeatOnNewPage = XmlUtil.Boolean(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					default:
						break;
				}
			}
			if (_TableRows == null)
				OwnerReport.rl.LogError(8, "Header requires the TableRows element.");
		}
		
		override internal void FinalPass()
		{
			_TableRows.FinalPass();

			OwnerReport.DataCache.Add(this);
			return;
		}

		internal void Run(IPresent ip, Row row)
		{
			_TableRows.Run(ip, row);
			return;
		}

		internal void RunPage(Pages pgs, Row row)
		{
			WorkClass wc = this.GetValue(pgs.Report);

			if (wc.OutputRow == row && wc.OutputPage == pgs.CurrentPage)
				return;

			Page p = pgs.CurrentPage;

			float height = p.YOffset + HeightOfRows(pgs, row);
            height += OwnerTable.GetPageFooterHeight(pgs, row);
			if (height > pgs.BottomOfPage)
			{
				Table t = OwnerTable;
                t.RunPageFooter(pgs, row, false);
				p = t.RunPageNew(pgs, p);
				t.RunPageHeader(pgs, row, false, null);
				if (this.RepeatOnNewPage)
					return;		// should already be on the page
			}

			_TableRows.RunPage(pgs, row);
			wc.OutputRow = row;
			wc.OutputPage = pgs.CurrentPage;
			return;
		}

		internal Table OwnerTable
		{
			get 
			{
				for (ReportLink rl = this.Parent; rl != null; rl = rl.Parent)
				{
					if (rl is Table)
						return rl as Table;
				}

				return null; 
			}
		}

		internal TableRows TableRows
		{
			get { return  _TableRows; }
			set {  _TableRows = value; }
		}
 
		internal float HeightOfRows(Pages pgs, Row r)
		{
			return _TableRows.HeightOfRows(pgs, r);
		}

		internal bool RepeatOnNewPage
		{
			get { return  _RepeatOnNewPage; }
			set {  _RepeatOnNewPage = value; }
		}
		#region ICacheData Members

		public void ClearCache(Report rpt)
		{
			rpt.Cache.Remove(this, "wc");
		}

		#endregion

		private WorkClass GetValue(Report rpt)
		{
			WorkClass wc = rpt.Cache.Get(this, "wc") as WorkClass;
			if (wc == null)
			{
				wc = new WorkClass();
				rpt.Cache.Add(this, "wc", wc);
			}
			return wc;
		}

		private void SetValue(Report rpt, WorkClass w)
		{
			rpt.Cache.AddReplace(this, "wc", w);
		}

		class WorkClass
		{
			internal Row OutputRow;		// the previous outputed row
			internal Page OutputPage;	// the previous outputed row
			internal WorkClass()
			{
				OutputRow = null;
				OutputPage = null;
			}
		}
	}
}
