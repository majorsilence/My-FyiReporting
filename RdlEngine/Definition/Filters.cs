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
	/// Collection of Filters for a DataSet.
	///</summary>
	[Serializable]
	internal class Filters : ReportLink
	{
        List<Filter> _Items;			// list of Filter

		internal Filters(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			Filter f;
            _Items = new List<Filter>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Filter":
						f = new Filter(r, this, xNodeLoop);
						break;
					default:	
						f=null;		// don't know what this is
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown Filters element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
				if (f != null)
					_Items.Add(f);
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "Filters require at least one Filter be defined.");
			else
				_Items.TrimExcess();
		}
		
		override internal void FinalPass()
		{
			foreach (Filter f in _Items)
			{
				f.FinalPass();
			}
			return;
		}

		internal bool Apply(Report rpt, Row datarow)
		{
			foreach (Filter f in _Items)
			{
				if (!f.FilterOperatorSingleRow)		// have to handle Top/Bottom in ApplyFinalFilters
					return true;
				if (!f.Apply(rpt, datarow))
					return false;
			}
			return true;
		}

		internal void ApplyFinalFilters(Report rpt, Rows data, bool makeCopy)
		{
			// Need to apply the Top/Bottom and then the rest of the data
			
			// Loop to the first top/bottom (Apply has already handled the SingleRow filters prior to
			//   the first top/bottom
			int iFilter;
			for (iFilter = 0; iFilter < _Items.Count; iFilter++)
			{
				Filter f = (Filter) _Items[iFilter];
				if (!f.FilterOperatorSingleRow)		
					break;
			}
			if (iFilter >= _Items.Count)	// nothing left to do?
				return;						// good this is a lot cheaper

			// make copy of data if necessary
			if (makeCopy)
			{
				List<Row> ar = new List<Row>(data.Data);	// Make a copy of the data!
				data.Data = ar;
			}

			// Handling the remaining filters
			for (; iFilter < _Items.Count && data.Data.Count > 0; iFilter++)
			{
				Filter f = (Filter) _Items[iFilter];
				f.Apply(rpt, data);
			}
			
			// trim the space
            data.Data.TrimExcess();
			
			// reset the row numbers
			int rowCount=0;
			foreach (Row r in data.Data)
				r.RowNumber = rowCount++;

			return;
		}

		internal List<Filter> Items
		{
			get { return  _Items; }
		}
	}
}
