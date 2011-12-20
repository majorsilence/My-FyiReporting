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
	/// In Charts, the dynamic series definition.
	///</summary>
	[Serializable]
	internal class DynamicSeries : ReportLink
	{
		Grouping _Grouping;	// The expressions to group the data by. Page
							// breaks in the grouping are not allowed.
		Sorting _Sorting;	// The expressions to sort the columns by
		Expression _Label;	// (string) The label displayed on the legend.		
	
		internal DynamicSeries(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Grouping=null;
			_Sorting=null;
			_Label=null;

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
					case "Label":
						_Label = new Expression(r, this, xNodeLoop, ExpressionType.String);
						break;
					default:
						break;
				}
			}
			if (_Grouping == null)
				OwnerReport.rl.LogError(8, "DynamicSeries requires the Grouping element.");
			if (_Label == null)
				OwnerReport.rl.LogError(8, "DynamicSeries requires the Label element.");
		}

		// Handle parsing of function in final pass
		override internal void FinalPass()
		{
			if (_Grouping != null)
				_Grouping.FinalPass();
			if (_Sorting != null)
				_Sorting.FinalPass();
			if (_Label != null)
				_Label.FinalPass();
			return;
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

		internal Expression Label
		{
			get { return  _Label; }
			set {  _Label = value; }
		}
	}
}
