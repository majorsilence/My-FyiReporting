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
using System.IO;

namespace fyiReporting.RDL
{
	///<summary>
	/// TableColumns definition and processing.
	///</summary>
	[Serializable]
	internal class TableColumns : ReportLink, IEnumerable<TableColumn>
	{
        List<TableColumn> _Items;			// list of TableColumn

		internal TableColumns(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			TableColumn tc;
            _Items = new List<TableColumn>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "TableColumn":
						tc = new TableColumn(r, this, xNodeLoop);
						break;
					default:	
						tc=null;		// don't know what this is
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown TableColumns element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
				if (tc != null)
					_Items.Add(tc);
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For TableColumns at least one TableColumn is required.");
			else
                _Items.TrimExcess();
		}

		internal TableColumn this[int ci]
		{
			get
			{
				return _Items[ci] as TableColumn;
			}
		}
		
		override internal void FinalPass()
		{
			foreach (TableColumn tc in _Items)
			{
				tc.FinalPass();
			}
			return;
		}

		internal void Run(IPresent ip, Row row)
		{
			foreach (TableColumn tc in _Items)
			{
				tc.Run(ip, row);
			}
			return;
		}

		// calculate the XPositions of all the columns
		internal void CalculateXPositions(Report rpt, float startpos, Row row)
		{
			float x = startpos;

			foreach (TableColumn tc in _Items)
			{
				if (tc.IsHidden(rpt, row))
					continue;
				tc.SetXPosition(rpt, x);
				x += tc.Width.Points;
			}
			return;
		}

        internal List<TableColumn> Items
		{
			get { return  _Items; }
		}


        #region IEnumerable<TableColumn> Members

        public IEnumerator<TableColumn> GetEnumerator()
        {
            return _Items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Items.GetEnumerator();
        }

        #endregion
    }
}
