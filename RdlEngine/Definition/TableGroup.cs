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
	/// TableGroup definition and processing.
	///</summary>
	[Serializable]
	internal class TableGroup : ReportLink
	{
		Grouping _Grouping;		// The expressions to group the data by.
		Sorting _Sorting;		// The expressions to sort the data by.
		Header _Header;			// A group header row.
		Footer _Footer;			// A group footer row.
		Visibility _Visibility;	// Indicates if the group (and all groups embedded
								// within it) should be hidden.		
		Textbox _ToggleTextbox;	//  resolved TextBox for toggling visibility
	
		internal TableGroup(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Grouping=null;
			_Sorting=null;
			_Header=null;
			_Footer=null;
			_Visibility=null;
			_ToggleTextbox=null;

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
					case "Header":
						_Header = new Header(r, this, xNodeLoop);
						break;
					case "Footer":
						_Footer = new Footer(r, this, xNodeLoop);
						break;
					case "Visibility":
						_Visibility = new Visibility(r, this, xNodeLoop);
						break;
					default:	
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown TableGroup element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
			if (_Grouping == null)
				OwnerReport.rl.LogError(8, "TableGroup requires the Grouping element.");
		}
		
		override internal void FinalPass()
		{
			if (_Grouping != null)
				_Grouping.FinalPass();
			if (_Sorting != null)
				_Sorting.FinalPass();
			if (_Header != null)
				_Header.FinalPass();
			if (_Footer != null)
				_Footer.FinalPass();
			if (_Visibility != null)
			{
				_Visibility.FinalPass();
				if (_Visibility.ToggleItem != null)
				{
					_ToggleTextbox = (Textbox) (OwnerReport.LUReportItems[_Visibility.ToggleItem]);
					if (_ToggleTextbox != null)
						_ToggleTextbox.IsToggle = true;
				}
			}
			return;
		}

		internal float DefnHeight()
		{
			float height=0;
			if (_Header != null)
				height += _Header.TableRows.DefnHeight();

			if (_Footer != null)
				height += _Footer.TableRows.DefnHeight();

			return height;
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

		internal Header Header
		{
			get { return  _Header; }
			set {  _Header = value; }
		}

		internal int HeaderCount
		{
			get 
			{
				if (_Header == null)
					return 0;
				else
					return _Header.TableRows.Items.Count;
			}
		}

		internal Footer Footer
		{
			get { return  _Footer; }
			set {  _Footer = value; }
		}

		internal int FooterCount
		{
			get 
			{
				if (_Footer == null)
					return 0;
				else
					return _Footer.TableRows.Items.Count;
			}
		}

		internal Visibility Visibility
		{
			get { return  _Visibility; }
			set {  _Visibility = value; }
		}

		internal Textbox ToggleTextbox
		{
			get { return  _ToggleTextbox; }
		}
	}
}
