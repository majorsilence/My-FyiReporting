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
	/// ChartGridLines definition and processing.
	///</summary>
	[Serializable]
	internal class ChartGridLines : ReportLink
	{
		bool _ShowGridLines;	// Indicates the gridlines should be shown
		Style _Style;			// Line style properties for the gridlines and tickmarks
		
		internal ChartGridLines(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_ShowGridLines=true;
			_Style=null;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "ShowGridLines":
						_ShowGridLines = XmlUtil.Boolean(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "Style":
						_Style = new Style(r, this, xNodeLoop);
						break;
					default:	// TODO
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown ChartGridLines element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
		

		}
		
		override internal void FinalPass()
		{
			if (_Style != null)
				_Style.FinalPass();
			return;
		}

		internal bool ShowGridLines
		{
			get { return  _ShowGridLines; }
			set {  _ShowGridLines = value; }
		}

		internal Style Style
		{
			get { return  _Style; }
			set {  _Style = value; }
		}
	}
}
