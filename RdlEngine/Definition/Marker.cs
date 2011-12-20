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
	/// Represents a marker on a chart.
	///</summary>
	[Serializable]
	internal class Marker : ReportLink
	{
		MarkerTypeEnum _Type;	// Defines the marker type for values. Default: none
		RSize _Size;		// Represents the height and width of the
							//  plotting area of marker(s).
		Style _Style;		// Defines the border and background style
							//  properties for the marker(s).		
	
		internal Marker(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Type=MarkerTypeEnum.None;
			_Size=null;
			_Style=null;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Type":
						_Type = MarkerType.GetStyle(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "Size":
						_Size = new RSize(r, xNodeLoop);
						break;
					case "Style":
						_Style = new Style(r, this, xNodeLoop);
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

		internal MarkerTypeEnum Type
		{
			get { return  _Type; }
			set {  _Type = value; }
		}

		internal RSize Size
		{
			get { return  _Size; }
			set {  _Size = value; }
		}

		internal Style Style
		{
			get { return  _Style; }
			set {  _Style = value; }
		}
	}

}
