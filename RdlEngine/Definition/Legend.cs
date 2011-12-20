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
	/// Chart legend definition (style, position, ...)
	///</summary>
	[Serializable]
	internal class Legend : ReportLink
	{
		bool _Visible;		// Specifies whether a legend is displayed.
							// Defaults to false.
		Style _Style;		// Defines text, border and background style
							// properties for the legend. All Textbox properties apply.
		LegendPositionEnum _Position;	// The position of the legend
									// Default: RightTop
		LegendLayoutEnum _Layout;	// The arrangement of labels within the legend
								// Default: Column
		bool _InsidePlotArea;	//Boolean If true, draw legend inside plot area, otherwise
								// draw outside plot area (default).
	
		internal Legend(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Visible=false;
			_Style=null;
			_Position=LegendPositionEnum.RightTop;
			_Layout=LegendLayoutEnum.Column;
			_InsidePlotArea=false;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Visible":
						_Visible = XmlUtil.Boolean(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "Style":
						_Style = new Style(r, this, xNodeLoop);
						break;
					case "Position":
						_Position = LegendPosition.GetStyle(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "Layout":
						_Layout = LegendLayout.GetStyle(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "InsidePlotArea":
						_InsidePlotArea = XmlUtil.Boolean(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					default:
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

		internal bool Visible
		{
			get { return  _Visible; }
			set {  _Visible = value; }
		}

		internal Style Style
		{
			get { return  _Style; }
			set {  _Style = value; }
		}

		internal LegendPositionEnum Position
		{
			get { return  _Position; }
			set {  _Position = value; }
		}

		internal LegendLayoutEnum Layout
		{
			get { return  _Layout; }
			set {  _Layout = value; }
		}

		internal bool InsidePlotArea
		{
			get { return  _InsidePlotArea; }
			set {  _InsidePlotArea = value; }
		}
	}
}
