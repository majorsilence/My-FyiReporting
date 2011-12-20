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
	/// ChartData definition and processing.
	///</summary>
	[Serializable]
	internal class ChartData : ReportLink
	{
        List<ChartSeries> _Items;			// list of chart series

		internal ChartData(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			ChartSeries cs;
            _Items = new List<ChartSeries>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "ChartSeries":
						cs = new ChartSeries(r, this, xNodeLoop);
						break;
					default:	
						cs=null;		// don't know what this is
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown ChartData element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
				if (cs != null)
					_Items.Add(cs);
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For ChartData at least one ChartSeries is required.");
			else
                _Items.TrimExcess();
		}
		
		override internal void FinalPass()
		{
			foreach (ChartSeries cs in _Items)
			{
				cs.FinalPass();
			}
			return;
		}

        internal List<ChartSeries> Items
		{
			get { return  _Items; }
		}
	}
}
