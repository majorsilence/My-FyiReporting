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
using System.Collections.Generic;
using System.Drawing;

namespace fyiReporting.RDL
{
	///<summary>
	/// Represents the report item for a List (i.e. absolute positioning)
	///</summary>
	[Serializable]
	internal class List : DataRegion
	{
		Grouping _Grouping;		//The expressions to group the data by
								// Required if there are any DataRegions
								// contained within this List
		Sorting _Sorting;		// The expressions to sort the repeated list regions by
		ReportItems _ReportItems;	// The elements of the list layout
		string _DataInstanceName;	// The name to use for the data element for the
								// each instance of this list. Ignored if there is a
								// grouping for the list.
								// Default: "Item"
		DataInstanceElementOutputEnum _DataInstanceElementOutput;
							// Indicates whether the list instances should
							// appear in a data rendering. Ignored if there is
							// a grouping for the list.  Default: output
		bool _CanGrow;			// indicates that row height can increase in size
		List<Textbox> _GrowList;	// list of TextBox's that need to be checked for growth

		internal List(ReportDefn r, ReportLink p, XmlNode xNode):base(r,p,xNode)
		{
			_Grouping=null;
			_Sorting=null;
			_ReportItems=null;
			_DataInstanceName="Item";
			_DataInstanceElementOutput=DataInstanceElementOutputEnum.Output;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Grouping":
						_Grouping = new Grouping(r, this, xNodeLoop);
						break;
					case "Sorting":
						_Sorting = new Sorting(r, this, xNodeLoop);
						break;
					case "ReportItems":
						_ReportItems = new ReportItems(r, this, xNodeLoop);
						break;
					case "DataInstanceName":
						_DataInstanceName = xNodeLoop.InnerText;
						break;
					case "DataInstanceElementOutput":
						_DataInstanceElementOutput = fyiReporting.RDL.DataInstanceElementOutput.GetStyle(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					default:	
						if (DataRegionElement(xNodeLoop))	// try at DataRegion level
							break;
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown List element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
			DataRegionFinish();			// Tidy up the DataRegion
		}

		override internal void FinalPass()
		{
			base.FinalPass();

			if (_Grouping != null)
				_Grouping.FinalPass();
			if (_Sorting != null)
				_Sorting.FinalPass();
			if (_ReportItems != null)
				_ReportItems.FinalPass();

			// determine if the size is dynamic depending on any of its
			//   contained textbox have cangrow true
			if (ReportItems == null)	// no report items in List region
				return;

			foreach (ReportItem ri in this.ReportItems.Items)
			{
				if (!(ri is Textbox))
					continue;
				Textbox tb = ri as Textbox;
				if (tb.CanGrow)
				{
					if (this._GrowList == null)
                        _GrowList = new List<Textbox>();
					_GrowList.Add(tb);
					_CanGrow = true;
				}
			}

			if (_CanGrow)				// shrink down the resulting list
                _GrowList.TrimExcess();

			return;
		}

		override internal void Run(IPresent ip, Row row)
		{
			Report r = ip.Report();
			WorkClass wc = GetValue(r);

			wc.Data = GetFilteredData(r, row);

			if (!AnyRows(ip, wc.Data))		// if no rows return
				return;					//   nothing left to do

			RunSetGrouping(r, wc);

			base.Run(ip, row);

			if (!ip.ListStart(this, row))	
				return;							// renderer doesn't want to continue
						   
			RunGroups(ip, wc, wc.Groups);

			ip.ListEnd(this, row);
			RemoveValue(r);
		}

		override internal void RunPage(Pages pgs, Row row)
		{
			Report r = pgs.Report;
			if (IsHidden(r, row))
				return;

			WorkClass wc = GetValue(r);
			wc.Data = GetFilteredData(r, row);

			SetPagePositionBegin(pgs);

			if (!AnyRowsPage(pgs, wc.Data))		// if no rows return
				return;						//   nothing left to do

			RunPageRegionBegin(pgs);

			RunSetGrouping(pgs.Report, wc);

			RunPageGroups(pgs, wc, wc.Groups);

			RunPageRegionEnd(pgs);
			SetPagePositionEnd(pgs, pgs.CurrentPage.YOffset);
			RemoveValue(r);
		}

		private void RunSetGrouping(Report rpt, WorkClass wc)
		{
			GroupEntry[] currentGroups; 

			// We have some data
			if (_Grouping != null || 
				_Sorting != null)		// fix up the data
			{
				Rows saveData = wc.Data;
				wc.Data = new Rows(rpt, null, _Grouping, _Sorting);
				wc.Data.Data = saveData.Data;
				wc.Data.Sort();
				PrepGroups(rpt, wc);
			}
			// If we haven't formed any groups then form one with all rows
			if (wc.Groups == null)
			{
                wc.Groups = new List<GroupEntry>();
				GroupEntry ge = new GroupEntry(null, null, 0);
				if (wc.Data.Data != null)		// Do we have any data?
					ge.EndRow = wc.Data.Data.Count-1;	// yes
				else
					ge.EndRow = -1;					// no
				wc.Groups.Add(ge);			// top group
				currentGroups = new GroupEntry[1];
			}
			else
				currentGroups = new GroupEntry[1];

			wc.Data.CurrentGroups = currentGroups;

			return;
		}

		private void PrepGroups(Report rpt, WorkClass wc)
		{
			if (_Grouping == null)
				return;

			int i=0;
			// 1) Build array of all GroupExpression objects
			List<GroupExpression> gea = _Grouping.GroupExpressions.Items;
			GroupEntry[] currentGroups = new GroupEntry[1];
			_Grouping.SetIndex(rpt, 0);	// set the index of this group (so we can find the GroupEntry)
			currentGroups[0] = new GroupEntry(_Grouping, _Sorting, 0);

			// Save the typecodes, and grouping by groupexpression; for later use
			TypeCode[] tcs = new TypeCode[gea.Count];
			Grouping[] grp = new Grouping[gea.Count];
			i=0;
			foreach (GroupExpression ge in gea)
			{
				grp[i] = (Grouping) (ge.Parent.Parent);			// remember the group
				tcs[i++] = ge.Expression.GetTypeCode();	// remember type of expression
			}

			// 2) Loop thru the data, then loop thru the GroupExpression list
			wc.Groups = new List<GroupEntry>();
			object[] savValues=null;
			object[] grpValues=null;
			int rowCurrent = 0;

			foreach (Row row in wc.Data.Data)
			{
				// Get the values for all the group expressions
				if (grpValues == null)
					grpValues = new object[gea.Count];

				i=0;
				foreach (GroupExpression ge in gea)  // Could optimize to only calculate as needed in comparison loop below??
				{
					grpValues[i++] = ge.Expression.Evaluate(rpt, row);
				}

				// For first row we just primed the pump; action starts on next row
				if (rowCurrent == 0)			// always start new group on first row
				{
					rowCurrent++;
					savValues = grpValues;
					grpValues = null;
					continue;
				}

				// compare the values; if change then we have a group break
				for (i = 0; i < savValues.Length; i++)
				{
					if (Filter.ApplyCompare(tcs[i], savValues[i], grpValues[i]) != 0)
					{
						// start a new group; and force a break on every subgroup
						GroupEntry saveGe=null;	
						for (int j = grp[i].GetIndex(rpt); j < currentGroups.Length; j++)
						{
							currentGroups[j].EndRow = rowCurrent-1;
							if (j == 0)
								wc.Groups.Add(currentGroups[j]);		// top group
							else if (saveGe == null)
								currentGroups[j-1].NestedGroup.Add(currentGroups[j]);
							else 
								saveGe.NestedGroup.Add(currentGroups[j]);

							saveGe = currentGroups[j];	// retain this GroupEntry
							currentGroups[j] = new GroupEntry(currentGroups[j].Group,currentGroups[j].Sort, rowCurrent);
						}
						savValues = grpValues;
						grpValues = null;
						break;		// break out of the value comparison loop
					}
				}
				rowCurrent++;
			}

			// End of all rows force break on end of rows
			for (i = 0; i < currentGroups.Length; i++)
			{
				currentGroups[i].EndRow = rowCurrent-1;
				if (i == 0)
					wc.Groups.Add(currentGroups[i]);		// top group
				else
					currentGroups[i-1].NestedGroup.Add(currentGroups[i]);
			}
			return;
		}

		private void RunGroups(IPresent ip, WorkClass wc, List<GroupEntry> groupEntries)
		{
			foreach (GroupEntry ge in groupEntries)
			{
				// set the group entry value
				int index;
				if (ge.Group != null)	// groups?
				{
					ge.Group.ResetHideDuplicates(ip.Report());	// reset duplicate checking
					index = ge.Group.GetIndex(ip.Report());	// yes
				}
				else					// no; must be main dataset
					index = 0;
				wc.Data.CurrentGroups[index] = ge;
				if (ge.NestedGroup.Count > 0)
					RunGroupsSetGroups(ip.Report(), wc, ge.NestedGroup);

				if (ge.Group == null)
				{	// need to run all the rows since no group defined
					for (int r=ge.StartRow; r <= ge.EndRow; r++)
					{
						ip.ListEntryBegin(this,  wc.Data.Data[r]);
                        if (_ReportItems != null)
						    _ReportItems.Run(ip, wc.Data.Data[r]);
						ip.ListEntryEnd(this, wc.Data.Data[r]);
					}
				}
				else
				{	// need to process just whole group as a List entry
					ip.ListEntryBegin(this,  wc.Data.Data[ge.StartRow]);

					// pass the first row of the group
                    if (_ReportItems != null)
					    _ReportItems.Run(ip, wc.Data.Data[ge.StartRow]);

					ip.ListEntryEnd(this, wc.Data.Data[ge.StartRow]);
				}
			}
		}

		private void RunPageGroups(Pages pgs, WorkClass wc, List<GroupEntry> groupEntries)
		{
			Report rpt = pgs.Report;
			Page p = pgs.CurrentPage;
			float pagebottom = OwnerReport.BottomOfPage;
//			p.YOffset += (Top == null? 0: this.Top.Points);
            p.YOffset += this.RelativeY(rpt);
            float listoffset = GetOffsetCalc(pgs.Report) + LeftCalc(pgs.Report);

			float height;	
			Row row;

			foreach (GroupEntry ge in groupEntries)
			{
				// set the group entry value
				int index;
				if (ge.Group != null)	// groups?
				{
					ge.Group.ResetHideDuplicates(rpt);	// reset duplicate checking
					index = ge.Group.GetIndex(rpt);	// yes
				}
				else					// no; must be main dataset
					index = 0;
				wc.Data.CurrentGroups[index] = ge;
				if (ge.NestedGroup.Count > 0)
					RunGroupsSetGroups(rpt, wc, ge.NestedGroup);

				if (ge.Group == null)
				{	// need to run all the rows since no group defined
					for (int r=ge.StartRow; r <= ge.EndRow; r++)
					{
						row = wc.Data.Data[r];
						height = HeightOfList(rpt, pgs.G, row);
                        
                        if (p.YOffset + height > pagebottom && !p.IsEmpty())		// need another page for this row?
                            p = RunPageNew(pgs, p);					// yes; if at end this page is empty

                        float saveYoffset = p.YOffset;              // this can be affected by other page items
                        if (Style != null) {
	                        var border = new PageRectangle();
	                        SetPagePositionAndStyle(rpt, border, row);
	                        p.AddObject(border);
                        }

                        if (_ReportItems != null)
						    _ReportItems.RunPage(pgs, row, listoffset);

                        if (p == pgs.CurrentPage)       // did subitems force new page?
                        {   // no use the height of the list
                            p.YOffset = saveYoffset + height;
                        }
                        else
                        {   // got forced to new page; just add the padding on
                            p = pgs.CurrentPage;        // set to new page
                            if (this.Style != null)
                            {
                                p.YOffset += this.Style.EvalPaddingBottom(rpt, row);
                            }
                        }
					}
				}
				else
				{	// need to process just whole group as a List entry
					if (ge.Group.PageBreakAtStart && !p.IsEmpty())
						p = RunPageNew(pgs, p);

					// pass the first row of the group
					row = wc.Data.Data[ge.StartRow];
					height = HeightOfList(rpt, pgs.G, row);
                    
                    if (p.YOffset + height > pagebottom && !p.IsEmpty())		// need another page for this row?
                        p = RunPageNew(pgs, p);					// yes; if at end this page is empty
                    float saveYoffset = p.YOffset;              // this can be affected by other page items
                    
                    if (_ReportItems != null)
                        _ReportItems.RunPage(pgs, row, listoffset);


                    if (p == pgs.CurrentPage)       // did subitems force new page?
                    {   // no use the height of the list
                        p.YOffset = saveYoffset + height;
                    }
                    else
                    {   // got forced to new page; just add the padding on
                        p = pgs.CurrentPage;        // set to new page
                        if (this.Style != null)
                        {
                            p.YOffset += this.Style.EvalPaddingBottom(rpt, row);
                        }
                    }
					
                    if (ge.Group.PageBreakAtEnd ||					// need another page for next group?
						p.YOffset + height > pagebottom)
					{
						p = RunPageNew(pgs, p);						// yes; if at end empty page will be cleaned up later
					}
				}
                RemoveWC(rpt);
			}
		}

        internal override void RemoveWC(Report rpt)
        {
            base.RemoveWC(rpt);
            
            if (this._ReportItems == null)
                return;

            foreach (ReportItem ri in this._ReportItems.Items)
            {
                ri.RemoveWC(rpt);
            }
        }

		private void RunGroupsSetGroups(Report rpt, WorkClass wc, List<GroupEntry> groupEntries)
		{
			// set the group entry value
			GroupEntry ge = groupEntries[0];
			wc.Data.CurrentGroups[ge.Group.GetIndex(rpt)] = ge;

			if (ge.NestedGroup.Count > 0)
				RunGroupsSetGroups(rpt, wc, ge.NestedGroup);
		}

		internal Grouping Grouping
		{
			get { return  _Grouping; }
			set {  _Grouping = value; }
		}

		internal float HeightOfList(Report rpt, Graphics g, Row r)
		{		   
			WorkClass wc = GetValue(rpt);

			float defnHeight = this.HeightOrOwnerHeight;
			if (!_CanGrow)
				return defnHeight;

			float height;
			foreach (Textbox tb in this._GrowList)
			{
				float top = (float) (tb.Top == null? 0.0 : tb.Top.Points);
				height = top + tb.RunTextCalcHeight(rpt, g, r);
				if (tb.Style != null)
					height += (tb.Style.EvalPaddingBottom(rpt, r) + tb.Style.EvalPaddingTop(rpt, r));
				defnHeight = Math.Max(height, defnHeight);
			}
			wc.CalcHeight = defnHeight;
			return defnHeight;
		}

		internal Sorting Sorting
		{
			get { return  _Sorting; }
			set {  _Sorting = value; }
		}

		internal ReportItems ReportItems
		{
			get { return  _ReportItems; }
			set {  _ReportItems = value; }
		}

		internal string DataInstanceName
		{
			get { return  _DataInstanceName == null? "Item": _DataInstanceName; }
			set {  _DataInstanceName = value; }
		}

		internal DataInstanceElementOutputEnum DataInstanceElementOutput
		{
			get { return  _DataInstanceElementOutput; }
			set {  _DataInstanceElementOutput = value; }
		}

		private WorkClass GetValue(Report rpt)
		{
			WorkClass wc = rpt.Cache.Get(this, "wc") as WorkClass;
			if (wc == null)
			{
				wc = new WorkClass(this);
				rpt.Cache.Add(this, "wc", wc);
			}
			return wc;
		}

		private void RemoveValue(Report rpt)
		{
			rpt.Cache.Remove(this, "wc");
		}

		class WorkClass
		{
			internal float CalcHeight;		// dynamic when CanGrow true
			internal Rows Data;	// Runtime data; either original query if no groups
						// or sorting or a copied version that is grouped/sorted
            internal List<GroupEntry> Groups;			// Runtime groups; array of GroupEntry
			internal WorkClass(List l)
			{
				CalcHeight = l.Height == null? 0: l.Height.Points;
				Data=null;
				Groups=null;
			}
		}
	}
}
