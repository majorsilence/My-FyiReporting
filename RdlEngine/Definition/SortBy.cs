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
	/// A single sort expression and direction.
	///</summary>
	[Serializable]
	internal class SortBy : ReportLink
	{
			Expression _SortExpression;	// (Variant) The expression to sort the groups by.
						// The functions RunningValue and RowNumber
						// are not allowed in SortExpression.
						// References to report items are not allowed.
			SortDirectionEnum _Direction;	// Indicates the direction of the sort
										// Ascending (Default) | Descending
	
		internal SortBy(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_SortExpression=null;
			_Direction=SortDirectionEnum.Ascending;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "SortExpression":
						_SortExpression = new Expression(r, this, xNodeLoop, ExpressionType.Variant);
						break;
					case "Direction":
						_Direction = SortDirection.GetStyle(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					default:
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown SortBy element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
			if (_SortExpression == null)
				OwnerReport.rl.LogError(8, "SortBy requires the SortExpression element.");
		}

		// Handle parsing of function in final pass
		override internal void FinalPass()
		{
			if (_SortExpression != null)
				_SortExpression.FinalPass();
			return;
		}

		internal Expression SortExpression
		{
			get { return  _SortExpression; }
			set {  _SortExpression = value; }
		}

		internal SortDirectionEnum Direction
		{
			get { return  _Direction; }
			set {  _Direction = value; }
		}
	}
}
