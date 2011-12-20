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

namespace fyiReporting.RDL
{
	///<summary>
	/// Chart Series grouping (both dynamic and static).
	///</summary>
	[Serializable]
	internal class SeriesGrouping : ReportLink
	{
		DynamicSeries _DynamicSeries;	// Dynamic Series headings for this grouping
		StaticSeries _StaticSeries;		// Static Series headings for this grouping	
		Style _Style;					// border and background properties for series legend itmes and data points
										//   when dynamic exprs are evaluated per group instance
	
		internal SeriesGrouping(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_DynamicSeries=null;
			_StaticSeries=null;
			_Style=null;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "DynamicSeries":
						_DynamicSeries = new DynamicSeries(r, this, xNodeLoop);
						break;
					case "StaticSeries":
						_StaticSeries = new StaticSeries(r, this, xNodeLoop);
						break;
					case "Style":
						_Style = new Style(OwnerReport, this, xNodeLoop);
						OwnerReport.rl.LogError(4, "Style element in SeriesGrouping is currently ignored."); // TODO
						break;
					default:	
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown SeriesGrouping element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
		}
		
		override internal void FinalPass()
		{
			if (_DynamicSeries != null)
				_DynamicSeries.FinalPass();
			if (_StaticSeries != null)
				_StaticSeries.FinalPass();
			if (_Style != null)
				_Style.FinalPass();

			return;
		}

		internal DynamicSeries DynamicSeries
		{
			get { return  _DynamicSeries; }
			set {  _DynamicSeries = value; }
		}

		internal StaticSeries StaticSeries
		{
			get { return  _StaticSeries; }
			set {  _StaticSeries = value; }
		}

		internal Style Style
		{
			get { return  _Style; }
			set {  _Style = value; }
		}

	}
}
