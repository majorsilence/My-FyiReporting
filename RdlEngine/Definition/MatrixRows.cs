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
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace fyiReporting.RDL
{
	///<summary>
	/// Collection of matrix rows
	///</summary>
	[Serializable]
	internal class MatrixRows : ReportLink
	{
        List<MatrixRow> _Items;			// list of MatrixRow

		internal MatrixRows(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			MatrixRow m;
            _Items = new List<MatrixRow>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "MatrixRow":
						m = new MatrixRow(r, this, xNodeLoop);
						break;
					default:	
						m=null;		// don't know what this is
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown MatrixRows element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
				if (m != null)
					_Items.Add(m);
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For MatrixRows at least one MatrixRow is required.");
			else
                _Items.TrimExcess();
		}
		
		override internal void FinalPass()
		{
			foreach (MatrixRow m in _Items)
			{
				m.FinalPass();
			}
			return;
		}

		internal float DefnHeight()
		{
			float height=0;
			foreach (MatrixRow m in _Items)
			{
				height += m.Height.Points;
			}
			return height;
		}

        internal List<MatrixRow> Items
		{
			get { return  _Items; }
		}

		/// <summary>
		/// CellCount requires a correctly configured Matrix structure.  This is true at runtime
		/// but not necessarily true during Matrix parse.
		/// </summary>
		internal int CellCount
		{
			get 
			{
				MatrixRow mr = _Items[0] as MatrixRow;
				return mr.MatrixCells.Items.Count;
			}
		}
	}
}
