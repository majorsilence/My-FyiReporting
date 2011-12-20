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
using System.Collections;

namespace fyiReporting.RDL
{
	///<summary>
	/// In Matrix, the dynamic columns with what subtotal information is needed.
	///</summary>
	[Serializable]
	internal class DynamicColumns : ReportLink
	{
		Grouping _Grouping;	// The expressions to group the data by.
		Sorting _Sorting;	// The expressions to sort the columns by.
		Subtotal _Subtotal;	// Indicates an automatic subtotal column should be included
		ReportItems _ReportItems;	// The elements of the column header layout
							// This ReportItems collection must contain exactly one
							// ReportItem. The Top, Left, Height and Width for this
							// ReportItem are ignored. The position is taken to be 0,
							// 0 and the size to be 100%, 100%.
		Visibility _Visibility;	// Indicates if all of the dynamic columns for this
							// grouping should be hidden and replaced with a
							// subtotal column for this grouping scope		

		internal DynamicColumns(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Grouping=null;
			_Sorting=null;
			_Subtotal=null;
			_ReportItems=null;
			_Visibility=null;

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
					case "Subtotal":
						_Subtotal = new Subtotal(r, this, xNodeLoop);
						break;
					case "ReportItems":
						_ReportItems = new ReportItems(r, this, xNodeLoop);
						break;
					case "Visibility":
						_Visibility = new Visibility(r, this, xNodeLoop);
						break;
					default:
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown DynamicColumn element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
			if (_Grouping == null)
				OwnerReport.rl.LogError(8, "DynamicColumns requires the Grouping element.");
			if (_ReportItems == null || _ReportItems.Items.Count != 1)
				OwnerReport.rl.LogError(8, "DynamicColumns requires the ReportItems element defined with exactly one report item.");
		}
		
		override internal void FinalPass()
		{
			if (_Grouping != null)
				_Grouping.FinalPass();
			if (_Sorting != null)
				_Sorting.FinalPass();
			if (_Subtotal != null)
				_Subtotal.FinalPass();
			if (_ReportItems != null)
				_ReportItems.FinalPass();
			if (_Visibility != null)
				_Visibility.FinalPass();
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

		internal Subtotal Subtotal
		{
			get { return  _Subtotal; }
			set {  _Subtotal = value; }
		}

		internal ReportItems ReportItems
		{
			get { return  _ReportItems; }
			set {  _ReportItems = value; }
		}

		internal Visibility Visibility
		{
			get { return  _Visibility; }
			set {  _Visibility = value; }
		}
	}
}
