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
using System.IO;

namespace fyiReporting.RDL
{
	///<summary>
	///  Definition of footer rows for a table or group.
	///</summary>
	[Serializable]
	internal class Footer : ReportLink
	{
		TableRows _TableRows;	// The footer rows for the table or group
		bool _RepeatOnNewPage;	// Indicates this footer should be displayed on
								// each page that the table (or group) is displayed		
	
		internal Footer(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_TableRows=null;
			_RepeatOnNewPage=false;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "TableRows":
						_TableRows = new TableRows(r, this, xNodeLoop);
						break;
					case "RepeatOnNewPage":
						_RepeatOnNewPage = XmlUtil.Boolean(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					default:
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown Footer element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
			if (_TableRows == null)
				OwnerReport.rl.LogError(8, "TableRows element is required with a Footer but not specified.");
		}
		
		override internal void FinalPass()
		{
			_TableRows.FinalPass();
			return;
		}

		internal void Run(IPresent ip, Row row)
		{
			_TableRows.Run(ip, row);
			return;
		}

		internal void RunPage(Pages pgs, Row row)
		{

			Page p = pgs.CurrentPage;
			if (p.YOffset + HeightOfRows(pgs, row) > pgs.BottomOfPage)
			{
				p = OwnerTable.RunPageNew(pgs, p);
				OwnerTable.RunPageHeader(pgs, row, false, null);
			}
			_TableRows.RunPage(pgs, row);

			return;
		}

		internal TableRows TableRows
		{
			get { return  _TableRows; }
			set {  _TableRows = value; }
		}

		internal float HeightOfRows(Pages pgs, Row r)
		{
			return _TableRows.HeightOfRows(pgs, r);
		}

		internal bool RepeatOnNewPage
		{
			get { return  _RepeatOnNewPage; }
			set {  _RepeatOnNewPage = value; }
		}

		internal Table OwnerTable
		{
			get 
			{ 
				ReportLink rl = this.Parent;
				while (rl != null)
				{
					if (rl is Table)
						return rl as Table;
					rl = rl.Parent;
				}
				return null;
			}
		}
	}
}
