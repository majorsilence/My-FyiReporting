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
	/// For tabular reports, defines the detail rows with grouping and sorting.
	///</summary>
	[Serializable]
	internal class Details : ReportLink
	{
		TableRows _TableRows;	// The details rows for the table. The details rows
								// cannot contain any DataRegions in any of their TableCells.
		Grouping _Grouping;		// The expressions to group the detail data by
		Sorting _Sorting;		// The expressions to sort the detail data by
		Visibility _Visibility;	// Indicates if the details should be hidden	
		Textbox _ToggleTextbox;	//  resolved TextBox for toggling visibility
	
		internal Details(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_TableRows=null;
			_Grouping=null;
			_Sorting=null;
			_Visibility=null;
			_ToggleTextbox = null;

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
					case "Grouping":
						_Grouping = new Grouping(r, this, xNodeLoop);
						break;
					case "Sorting":
						_Sorting = new Sorting(r, this, xNodeLoop);
						break;
					case "Visibility":
						_Visibility = new Visibility(r, this, xNodeLoop);
						break;
					default:
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown Details element " + xNodeLoop.Name + " ignored.");
						break;
				}
			}
			if (_TableRows == null)
				OwnerReport.rl.LogError(8, "Details requires the TableRows element.");
		}
		
		override internal void FinalPass()
		{
			_TableRows.FinalPass();
			if (_Grouping != null)
				_Grouping.FinalPass();
			if (_Sorting != null)
				_Sorting.FinalPass();
			if (_Visibility != null)
			{
				_Visibility.FinalPass();
				if (_Visibility.ToggleItem != null)
				{
					_ToggleTextbox = (Textbox) (OwnerReport.LUReportItems[_Visibility.ToggleItem]);
					if (_ToggleTextbox != null)
						_ToggleTextbox.IsToggle = true;
				}
			}
			return;
		}
		
		internal void Run(IPresent ip, Rows rs, int start, int end)
		{
			// if no rows output or rows just leave
			if (rs == null || rs.Data == null)
				return;
            if (this.Visibility != null && Visibility.IsHidden(ip.Report(), rs.Data[start]) && Visibility.ToggleItem == null)
                return;                 // not visible

			for (int r=start; r <= end; r++)
			{
				_TableRows.Run(ip, rs.Data[r]);
			}
			return;
		}
		
		internal void RunPage(Pages pgs, Rows rs, int start, int end, float footerHeight)
		{
			// if no rows output or rows just leave
			if (rs == null || rs.Data == null)
				return;

            if (this.Visibility != null && Visibility.IsHidden(pgs.Report, rs.Data[start]))
                return;                 // not visible

			Page p;

			Row row;
			for (int r=start; r <= end; r++)
			{
				p = pgs.CurrentPage;			// this can change after running a row
				row = rs.Data[r];
				float hrows = HeightOfRows(pgs, row);	// height of all the rows in the details
				float height = p.YOffset + hrows;

                // add the footerheight that must be on every page
                height += OwnerTable.GetPageFooterHeight(pgs, row);

				if (r == end) 
					height += footerHeight;		// on last row; may need additional room for footer
				if (height > pgs.BottomOfPage)
				{
                    OwnerTable.RunPageFooter(pgs, row, false);
					p = OwnerTable.RunPageNew(pgs, p);
					OwnerTable.RunPageHeader(pgs, row, false, null);
                    _TableRows.RunPage(pgs, row, true);   // force checking since header + hrows might be > BottomOfPage
                }
                else 
				    _TableRows.RunPage(pgs, row, hrows > pgs.BottomOfPage);
			}
			return;
		}
  
		internal TableRows TableRows
		{
			get { return  _TableRows; }
			set {  _TableRows = value; }
		}

		internal float HeightOfRows(Pages pgs, Row r)
		{
            if (this.Visibility != null && Visibility.IsHidden(pgs.Report, r))
            {
                return 0;
            }

			return _TableRows.HeightOfRows(pgs, r);
		}

		internal Grouping Grouping
		{
			get { return  _Grouping; }
			set {  _Grouping = value; }
		}

		internal Sorting Sorting
		{
			get { return  _Sorting; }
			set {  _Sorting = value; }
		}

		internal Table OwnerTable
		{
			get { return (Table) (this.Parent); }
		}

		internal Visibility Visibility
		{
			get { return  _Visibility; }
			set {  _Visibility = value; }
		}

		internal Textbox ToggleTextbox
		{
			get { return  _ToggleTextbox; }
		}
	}
}
