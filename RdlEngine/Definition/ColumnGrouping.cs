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
	/// ColumnGrouping definition and processing.
	///</summary>
	[Serializable]
	internal class ColumnGrouping : ReportLink
	{
		RSize _Height;		// Height of the column header
		DynamicColumns _DynamicColumns;	// Dynamic column headings for this grouping
		StaticColumns _StaticColumns;		// Static column headings for this grouping		
	
		internal ColumnGrouping(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Height=null;
			_DynamicColumns=null;
			_StaticColumns=null;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Height":
						_Height = new RSize(r, xNodeLoop);
						break;
					case "DynamicColumns":
						_DynamicColumns = new DynamicColumns(r, this, xNodeLoop);
						break;
					case "StaticColumns":
						_StaticColumns = new StaticColumns(r, this, xNodeLoop);
						break;
					default:
						break;
				}
			}
			if (_Height == null)
				OwnerReport.rl.LogError(8, "ColumnGrouping requires the Height element to be specified.");

			if ((_DynamicColumns != null && _StaticColumns != null) ||
				(_DynamicColumns == null && _StaticColumns == null))
				OwnerReport.rl.LogError(8, "ColumnGrouping requires either the DynamicColumns element or StaticColumns element but not both.");
		}
		
		override internal void FinalPass()
		{
			if (_DynamicColumns != null)
				_DynamicColumns.FinalPass();
			if (_StaticColumns != null)
				_StaticColumns.FinalPass();
			return ;
		}


		internal RSize Height
		{
			get { return  _Height; }
			set {  _Height = value; }
		}

		internal DynamicColumns DynamicColumns
		{
			get { return  _DynamicColumns; }
			set {  _DynamicColumns = value; }
		}

		internal StaticColumns StaticColumns
		{
			get { return  _StaticColumns; }
			set {  _StaticColumns = value; }
		}
	}
}
