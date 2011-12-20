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
	/// Matrix row grouping definition.
	///</summary>
	[Serializable]
	internal class RowGrouping : ReportLink
	{
		RSize _Width;	// Width of the row header
		DynamicRows _DynamicRows;	// Dynamic row headings for this grouping
		StaticRows _StaticRows;	// Static row headings for this grouping		
	
		internal RowGrouping(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Width=null;
			_DynamicRows=null;
			_StaticRows=null;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Width":
						_Width = new RSize(r, xNodeLoop);
						break;
					case "DynamicRows":
						_DynamicRows = new DynamicRows(r, this, xNodeLoop);
						break;
					case "StaticRows":
						_StaticRows = new StaticRows(r, this, xNodeLoop);
						break;
					default:	
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown RowGrouping element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
			if (_Width == null)
				OwnerReport.rl.LogError(8, "RowGrouping requires the Width element.");
		}
		
		override internal void FinalPass()
		{
			if (_DynamicRows != null)
				_DynamicRows.FinalPass();
			if (_StaticRows != null)
				_StaticRows.FinalPass();
			return;
		}

		internal RSize Width
		{
			get { return  _Width; }
			set {  _Width = value; }
		}

		internal DynamicRows DynamicRows
		{
			get { return  _DynamicRows; }
			set {  _DynamicRows = value; }
		}

		internal StaticRows StaticRows
		{
			get { return  _StaticRows; }
			set {  _StaticRows = value; }
		}
	}
}
