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
	/// TableColumn definition and processing.
	///</summary>
	[Serializable]
	internal class TableColumn : ReportLink
	{
		RSize _Width;		// Width of the column
		Visibility _Visibility;	// Indicates if the column should be hidden	
		bool _FixedHeader=false;	// Header of this column should be display even when scrolled
	
		internal TableColumn(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Width=null;
			_Visibility=null;

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
					case "Visibility":
						_Visibility = new Visibility(r, this, xNodeLoop);
						break;
					case "FixedHeader":
						_FixedHeader = XmlUtil.Boolean(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					default:
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown TableColumn element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
			if (_Width == null)
				OwnerReport.rl.LogError(8, "TableColumn requires the Width element.");
		}
		
		override internal void FinalPass()
		{
			if (_Visibility != null)
				_Visibility.FinalPass();
			return;
		}

		internal void Run(IPresent ip, Row row)
		{
		}

		internal RSize Width
		{
			get { return  _Width; }
			set {  _Width = value; }
		}

		internal float GetXPosition(Report rpt)
		{
			WorkClass wc = GetWC(rpt);
			return wc.XPosition;
		}

		internal void SetXPosition(Report rpt, float xp)
		{
			WorkClass wc = GetWC(rpt);
			wc.XPosition = xp;
		}

		internal Visibility Visibility
		{
			get { return  _Visibility; }
			set {  _Visibility = value; }
		}

		internal bool IsHidden(Report rpt, Row r)
		{
			if (_Visibility == null)
				return false;
			return _Visibility.IsHidden(rpt, r);
		}

		private WorkClass GetWC(Report rpt)
		{
			if (rpt == null)	
				return new WorkClass();

			WorkClass wc = rpt.Cache.Get(this, "wc") as WorkClass;
			if (wc == null)
			{
				wc = new WorkClass();
				rpt.Cache.Add(this, "wc", wc);
			}
			return wc;
		}

		private void RemoveWC(Report rpt)
		{
			rpt.Cache.Remove(this, "wc");
		}

		class WorkClass
		{
			internal float XPosition;	// Set at runtime by Page processing; potentially dynamic at runtime
			//  since visibility is an expression
			internal WorkClass()
			{
				XPosition=0;
			}
		}
	}
}
