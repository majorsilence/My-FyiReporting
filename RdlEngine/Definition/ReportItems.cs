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
	/// Collection of specific reportitems (e.g. TextBoxs, Images, ...)
	///</summary>
	[Serializable]
    internal class ReportItems : ReportLink, IEnumerable
	{
		List<ReportItem> _Items;				// list of report items

		internal ReportItems(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			ReportItem ri;
            _Items = new List<ReportItem>();

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Rectangle":
						ri = new Rectangle(r, this, xNodeLoop);
						break;
					case "Line":
						ri = new Line(r, this, xNodeLoop);
						break;
					case "Textbox":
						ri = new Textbox(r, this, xNodeLoop);
						break;
					case "Image":
						ri = new Image(r, this, xNodeLoop);
						break;
					case "Subreport":
						ri = new Subreport(r, this, xNodeLoop);
						break;
					// DataRegions: list, table, matrix, chart
					case "List": 
						ri = new List(r, this, xNodeLoop);
						break;
					case "Table":
                    case "Grid":
                    case "fyi:Grid":
                        ri = new Table(r, this, xNodeLoop);
						break;
					case "Matrix": 
						ri = new Matrix(r, this, xNodeLoop);
						break;
					case "Chart": 
						ri = new Chart(r, this, xNodeLoop);
						break;
					case "ChartExpression":		// For internal use only 
						ri = new ChartExpression(r, this, xNodeLoop);
						break;
                    case "CustomReportItem":
                        ri = new CustomReportItem(r, this, xNodeLoop);
                        break;
					default:
						ri=null;		// don't know what this is
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown ReportItems element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
				if (ri != null)
				{
					_Items.Add(ri);
				}
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "At least one item must be in the ReportItems.");
			else
                _Items.TrimExcess();
		}

        internal ReportItem this[int i]
        {
            get
            {
                return _Items[i];
            }
        }
		
		override internal void FinalPass()
		{
			foreach (ReportItem ri in _Items)
			{
				ri.FinalPass();
			}
			_Items.Sort();				// sort on ZIndex; y, x (see ReportItem compare routine)

            for (int i = 0; i < _Items.Count; i++)
            {
                ReportItem ri = _Items[i];
                ri.PositioningFinalPass(i, _Items);
            }
            //foreach (ReportItem ri in _Items)	
            //    ri.PositioningFinalPass(_Items);

			return;
		}

		internal void Run(IPresent ip, Row row)
		{
			foreach (ReportItem ri in _Items)
			{
				ri.Run(ip, row);
			}
			return;
		}

		internal void RunPage(Pages pgs, Row row, float xOffset)
		{
			SetXOffset(pgs.Report, xOffset);
			foreach (ReportItem ri in _Items)
			{
				ri.RunPage(pgs, row);
			}
			return;
		}

        internal List<ReportItem> Items
		{
			get { return  _Items; }
		}

		internal float GetXOffset(Report rpt)
		{
			OFloat of = rpt.Cache.Get(this, "xoffset") as OFloat;
			return of == null? 0: of.f;
		}

		internal void SetXOffset(Report rpt, float f)
		{
			OFloat of = rpt.Cache.Get(this, "xoffset") as OFloat;
			if (of == null)
				rpt.Cache.Add(this, "xoffset", new OFloat(f));
			else
				of.f = f;
		}

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return _Items.GetEnumerator();
        }

        #endregion
    }
}
