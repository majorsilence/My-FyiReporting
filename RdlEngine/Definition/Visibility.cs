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
	/// Visibility definition and processing.
	///</summary>
	[Serializable]
	internal class Visibility : ReportLink
	{
		Expression _Hidden;		// (Boolean) Indicates if the item should be initially hidden.
		string _ToggleItem;		// The name of the textbox used to
					// hide/unhide this report item. Clicking on
					//an instance of the ToggleItem will toggle
					//the hidden state of every corresponding
					//instance of this item. If the Toggle item
					//becomes hidden, this item should become
					//hidden.
					//Must be a textbox in the same grouping
					//scope as this item or in any containing (ancestor) grouping scope
					//If omitted, no item will toggle the hidden
					//state of this item.
					//Not allowed on and cannot refer to report
					//items contained in a page header or
					//footer.
					//Cannot refer to a report item contained
					//within the current report item unless
					//current grouping scope has a Parent.		
	
		internal Visibility(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Hidden=null;
			_ToggleItem=null;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Hidden":
						_Hidden = new Expression(r, this, xNodeLoop, ExpressionType.Boolean);
						break;
					case "ToggleItem":
						_ToggleItem = xNodeLoop.InnerText;
						break;
					default:	
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown Visibility element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
		}

		// Handle parsing of function in final pass
		override internal void FinalPass()
		{
			if (_Hidden != null)
				_Hidden.FinalPass();
			return;
		}

		internal Expression Hidden
		{
			get { return  _Hidden; }
			set {  _Hidden = value; }
		}

		internal bool IsHidden(Report rpt, Row r)
		{
			if (_Hidden == null)
				return false;

			return _Hidden.EvaluateBoolean(rpt, r);
		}

		internal string ToggleItem
		{
			get { return  _ToggleItem; }
			set {  _ToggleItem = value; }
		}
	}
}
